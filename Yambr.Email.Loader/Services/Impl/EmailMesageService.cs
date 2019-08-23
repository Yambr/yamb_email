﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MimeKit;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.Extensions;
using Yambr.Email.Loader.Services.Default;

namespace Yambr.Email.Loader.Services
{
    public class EmailMesageService : IEmailMessageService
    {
        private readonly IMailBox _mailBox;
        private readonly ILogger _logger;
        private readonly IRecordCollection<EmailMessage> _mailMessageCollection;
        private readonly IContactService _contactService;
        private readonly IHtmlConverterService _htmlConverterService;
        private readonly IRecordDatabase _recordDatabase;

        public EmailMesageService(
            ILogger<IEmailMessageService> logger,
            IMailBox mailBox,
            IRecordCollection<EmailMessage> mailMessageCollection,
            IContactService contactService,
            IHtmlConverterService htmlConverterService,
            IRecordDatabase recordDatabase)
        {
            _contactService = contactService;
            _htmlConverterService = htmlConverterService;
            _recordDatabase = recordDatabase;
            _mailBox = mailBox;
            _logger = logger;
            _mailMessageCollection = mailMessageCollection;
        }

        /// <summary>
        /// можно ли сохранить 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task<bool> MustBeSavedAsync(MimeMessage message)
        {
            return Task.FromResult(message.Date.UtcDateTime > _mailBox.LastStartTimeUtc);
        }

        /// <summary>
        /// Сохранить сообщение
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SaveMessageAsync(MimeMessage message)
        {
            var emailMessage = await GetOrCreateMessageAsync(message);
            AddOwner(emailMessage);
            if (emailMessage.HasUnsaved)
            {
                await CreateOrUpdateOneAsync(emailMessage);
            }
        }

        #region Сохранение

        /// <summary>
        /// Сохранить или обновить запись
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        private async Task CreateOrUpdateOneAsync(EmailMessage emailMessage)
        {
            var filterDefinitionBuilder = new FilterDefinitionBuilder<EmailMessage>();
            //запишем что по Email определяем
            await _mailMessageCollection.CreateOrUpdateOneAsync(emailMessage, filterDefinition: filterDefinitionBuilder.Eq(c => c.Hash, emailMessage.Hash));
        }
       
        /// <summary>
        /// Получить или создать сообщения (без сохранения в бд)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<EmailMessage> GetOrCreateMessageAsync(MimeMessage message)
        {
            // вычислим хэш сообщения
            var messageHash = new EmailMessageSummary(message).GetHashByJson();
            _logger.Info($"Сообщение от {message.Date} хэш {messageHash}");
            //попробуем получить сообщение по хеш или создадим его
            return await GetMessageByHashAsync(messageHash)
                   ?? await CreateMessageAsync(message, messageHash);
        }

        /// <summary>
        /// Получить сообщение по Хэш
        /// </summary>
        /// <param name="messageHash"></param>
        /// <returns></returns>
        private async Task<EmailMessage> GetMessageByHashAsync([NotNull] string messageHash)
        {
            if (string.IsNullOrWhiteSpace(messageHash)) throw new ArgumentNullException(nameof(messageHash));
            var cursor = await _mailMessageCollection.FindAsync(mes => mes.Hash == messageHash,
                new FindOptions<EmailMessage, EmailMessage>
                {
                    Limit = 1
                });

            while (cursor.MoveNext())
            {
                var em = cursor.Current.FirstOrDefault();
                em?.ClearChangedProperties();
                return em;
            }
            return null;
        }

        /// <summary>
        /// создать сообщения (без сохранения в бд)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageHash"></param>
        /// <returns></returns>
        private async Task<EmailMessage> CreateMessageAsync(MimeMessage message, string messageHash)
        {
            //заполним основные поля
            var emailMessage = new EmailMessage
            {
                Subject = message.Subject,
                DateUtc = message.Date.UtcDateTime,
                Hash = messageHash
            };
            AddOwner(emailMessage);
            //заполнив основные поля сохраним сообщение чтобы если паралельно будет еще обрабатываться где то это сообщение то мы его уже нашли 
            // т.к. обработка дальнейшая может занять какое то время
            await CreateOrUpdateOneAsync(emailMessage);
            emailMessage.ClearChangedProperties();

            _logger.Info($"Создано сообщение {messageHash}");
            FillBody(message, emailMessage);
            FillHeaders(emailMessage);

            //заполним кому и от кого
            await FillAddresses(message, emailMessage);
            //заполним направление 
            FillDirection(emailMessage);
            //сохраним вложения
            await SaveAttachmentsAsync(message, emailMessage);
            //Сохраним встроенный контент
            await SaveEmbeddedAsync(message, emailMessage);
            //сохраним хэш теги из темы
            FillTagsFromSubject(emailMessage);

            return emailMessage;
        }
        /// <summary>
        /// Заполним заголовок (основное содержимое тела)
        /// </summary>
        /// <param name="emailMessage"></param>
        private void FillHeaders(IBodyPart emailMessage)
        {
            if (string.IsNullOrWhiteSpace(emailMessage.Body)) return;
            //Достанем текст письма и если оно в html то приведем его в нормальный вид
            var text = emailMessage.IsBodyHtml
                ? _htmlConverterService.ConvertHtml(emailMessage.Body)
                : emailMessage.Body;
            if (string.IsNullOrWhiteSpace(text)) return;
            //анализатор письма
            var processor = new Processor(MailAnalyzer.ANALYZER_NAME);
            var result = processor.Process(new SourceOfAnalysis(text));
            //достанем только почтовые блоки
            var emailBlocks = result.Entities.OfType<MailReferent>();
            //достанем блок с телом
            var bodyHeaders = emailBlocks.Where(c => c.Kind == MailKind.Body).ToList();
            var headerMain = bodyHeaders.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(headerMain?.Text)) return;
            //сохраним
            emailMessage.MainHeader = headerMain.Text;
            bodyHeaders.Remove(headerMain);
            if (bodyHeaders.Any())
            {
                bodyHeaders.ForEach(c =>
                {
                    if (!string.IsNullOrWhiteSpace(c.Text))
                    {
                        emailMessage.CommonHeaders.Add(new HeaderSummaryPart(c.Text));
                    }
                });
            }

        }

        #region Заполнение сообщения
        /// <summary>
        /// Добавить владельца
        /// </summary>
        /// <param name="emailMessage"></param>
        private void AddOwner(IContentItem emailMessage)
        {
            var mailOwnerSummary = new MailOwnerSummary(_mailBox.Login, _mailBox.User?.Fio);
            if (!emailMessage.Owners.Any(c => c.Equals(mailOwnerSummary)))
            {
                emailMessage.Owners.Add(mailOwnerSummary);
            }
        }
        /// <summary>
        /// заполнить направление
        /// </summary>
        /// <param name="emailMessage"></param>
        private static void FillDirection(IMessagePart emailMessage)
        {
            //если в поле от есть наш пользователь значит письмо исходящее
            if (emailMessage.From.Any(c => c.Contact != null && c.Contact.User != null))
            {
                emailMessage.Direction = Direction.Outcoming;
            }
            else
            {
                emailMessage.Direction
                    = //если в поле кому есть наша почта значит входящее во всех остальных случаях оно входящее
                    emailMessage.To.Any(c => c.Contact != null && c.Contact.User != null)
                        ? Direction.Incoming
                        : Direction.Outcoming;
            }
        }
        /// <summary>
        /// Заполнить теги
        /// </summary>
        /// <param name="emailMessage"></param>
        private void FillTagsFromSubject(EmailMessage emailMessage)
        {
            if (emailMessage == null) throw new ArgumentNullException(nameof(emailMessage));
            if (emailMessage.Subject == null) return;
            string textWithoutTags;
            foreach (var tag in emailMessage.Subject.GetAllTags(out textWithoutTags))
            {
                emailMessage.Tags.Add(new HashTag
                {
                    Name = tag,
                    Type = $"{nameof(EmailMessage.Subject)}"
                });
            }
            emailMessage.SubjectWithoutTags = textWithoutTags;
        }

        /// <summary>
        /// заполнить тело
        /// </summary>
        /// <param name="message"></param>
        /// <param name="emailMessage"></param>
        private void FillBody(MimeMessage message, IBodyPart emailMessage)
        {
            emailMessage.IsBodyHtml = !string.IsNullOrWhiteSpace(message.HtmlBody);
            emailMessage.Body = emailMessage.IsBodyHtml ? 
                RemoveBadNodes(ExtractTextBody(message)) : 
                ExtractTextBody(message);
            
        }

        /// <summary>
        /// Заполнить адреса в сообщении
        /// </summary>
        /// <param name="message"></param>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        private async Task FillAddresses(MimeMessage message, IMessagePart emailMessage)
        {
            if (message.From?.Mailboxes != null)
            {
                foreach (var mailbox in message.From.Mailboxes)
                {
                    emailMessage.From.Add(await _contactService.GetOrCreateContactSummaryAsync(mailbox));
                }
            }

            if (message.To?.Mailboxes != null)
            {
                foreach (var mailbox in message.To.Mailboxes)
                {
                    emailMessage.To.Add(await _contactService.GetOrCreateContactSummaryAsync(mailbox));
                }
            }

            if (message.Cc?.Mailboxes != null)
            {
                foreach (var mailbox in message.Cc.Mailboxes)
                {
                    emailMessage.To.Add(await _contactService.GetOrCreateContactSummaryAsync(mailbox));
                }
            }

            if (message.Bcc?.Mailboxes != null)
            {
                foreach (var mailbox in message.Bcc.Mailboxes)
                {
                    emailMessage.To.Add(await _contactService.GetOrCreateContactSummaryAsync(mailbox));
                }
            }
        }

        #endregion

        #region Работа с телом сообщения

        /// <summary>
        /// Удалить из html не нужные теги
        /// </summary>
        /// <param name="messageHtmlBody"></param>
        /// <returns></returns>
        private string RemoveBadNodes(string messageHtmlBody)
        {
            if (messageHtmlBody == null) throw new ArgumentNullException(nameof(messageHtmlBody));
            //возмем документ 
            var html = new HtmlDocument();
            html.LoadHtml(messageHtmlBody);
            var document = html.DocumentNode;
            //вырежем из него все лишние теги
            //TODO в настройки
            var list = document.QuerySelectorAll("script,iframe,base,frame,frameset,source,audio,video,meta,link,title,noframes").ToList();
            foreach (var htmlNode in list)
            {
                htmlNode.Remove();
            }
            return document.WriteTo();
        }
     
        /// <summary>
        /// Достать текстовое тело
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string ExtractTextBody(MimeMessage message)
        {
          
            //если это текст то нужно достать текст из TextPart
            var mimeEntities = message.BodyParts.OfType<TextPart>().ToList();
            //пока смотрим в первую очередь первую html часть
            var htmlPart = mimeEntities.FirstOrDefault(c => c.IsHtml);
            if (htmlPart != null)
            {
                return htmlPart.GetTextInUtf8();
            }
            //далее возьмем остальные части , если не получилось получить части пихнем что есть
            if (!mimeEntities.Any()) return message.TextBody;
            var textParts = mimeEntities.Select(c=>c.GetTextInUtf8()).ToList();
            //достав все части сложим все в 
            return textParts.Any() ? string.Join("\r\n", textParts) : message.TextBody;
        }

        #endregion

        #region Сохранение файлов

        /// <summary>
        /// Сохранить встроенные вложения (картинки)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        private async Task SaveEmbeddedAsync(MimeMessage message, EmailMessage emailMessage)
        {
            var mimeEntities = message.BodyParts.OfType<MimePart>()
                .Where(c =>
                    !string.IsNullOrWhiteSpace(c.FileName) &&
                    !string.IsNullOrWhiteSpace(c.ContentId) &&
                    !c.IsAttachment)
                .ToList();

            var gridFsBucket = new GridFSBucket(_recordDatabase, new GridFSBucketOptions { BucketName = nameof(Embedded) });
            foreach (var embeddedMimePart in mimeEntities)
            {
               
                var fileName = embeddedMimePart.FileName;
                try
                {
                    using (var stream = await gridFsBucket.OpenUploadStreamAsync(fileName))
                    {
                        if (embeddedMimePart.ContentObject != null)
                        {
                            embeddedMimePart.ContentObject.DecodeTo(stream);
                            var embeddedSummarySummary = new EmbeddedSummary
                            {
                                FileName = fileName,
                                Size = stream.Length,
                                Ref = new MongoDBRef(gridFsBucket.Options.BucketName, stream.Id),
                                ContentId = embeddedMimePart.ContentId
                            };
                            emailMessage.Embedded.Add(embeddedSummarySummary);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Ошибка сохранения встроенного вложения {emailMessage.DateUtc} {emailMessage.Hash}", ex);
                    throw;
                }
            }

        }

        /// <summary>
        /// Сохранить вложения
        /// </summary>
        /// <param name="message"></param>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        private async Task SaveAttachmentsAsync(MimeMessage message, EmailMessage emailMessage)
        {
            var gridFsBucket = new GridFSBucket(_recordDatabase, new GridFSBucketOptions { BucketName = nameof(Attachment) });
            foreach (var attachment in message.Attachments)
            {
               
                var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name ?? "file.txt";
                try
                {
                    using (var stream = await gridFsBucket.OpenUploadStreamAsync(fileName))
                    {
                        // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
                        if (attachment is MessagePart)
                        {
                            var rfc822 = (MessagePart)attachment;
                            rfc822.Message.WriteTo(stream);
                        }
                        else
                        {
                            var part = (MimePart)attachment;
                            part.ContentObject.DecodeTo(stream);
                        }
                        var attachmentSummary = new AttachmentSummary
                        {
                            FileName = fileName,
                            Size = stream.Length,
                            Ref = new MongoDBRef(gridFsBucket.Options.BucketName, stream.Id)
                        };

                        emailMessage.Attachments.Add(attachmentSummary);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Ошибка сохранения вложения {emailMessage.DateUtc} {emailMessage.Hash}", ex);
                    throw;
                }
            }
        }

        #endregion

        #endregion
    }
}