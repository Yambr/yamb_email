using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using MimeKit;
using Yambr.Analyzer.Models;
using Yambr.Analyzer.Services;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.ExtensionPoints;
using Yambr.Email.Loader.Extensions;
using Yambr.Email.Loader.Services;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.Extensions;

namespace Yambr.Email.Loader.Components
{
    [Component]
    class DefaultEmailMessageHandler :IEmailMessageHandler
    {
        private readonly ILogger _logger;
        private readonly IMailAnalyzeService _mailAnalyzeService;
        private readonly IContactService _contactService;
        private readonly IHtmlConverterService _htmlConverterService;

        public IMailBox  MailBox { get; }

        public DefaultEmailMessageHandler(
            ILogger<DefaultEmailMessageHandler> logger,
            IMailBox mailBox,
            IMailAnalyzeService mailAnalyzeService,
            IContactService contactService,
            IHtmlConverterService htmlConverterService)
        {
            _logger = logger;
            _mailAnalyzeService = mailAnalyzeService;
            _contactService = contactService;
            _htmlConverterService = htmlConverterService;
            MailBox = mailBox;
        }


        public async Task<EmailMessage> OnCreate(MimeMessage message, EmailMessage emailMessage)
        {
            //TODO Разбить на части
            AddOwner(emailMessage);
            FillBody(message, emailMessage);
            var text = GetText(emailMessage);
            FillHeaders(emailMessage, text);
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
            FillPersons(emailMessage, text);
            return emailMessage;
        }

        private void FillPersons(EmailMessage emailMessage, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            var persons = _mailAnalyzeService.Persons(text);
            if (!persons.Any()) return;
            foreach (var contactSummary in emailMessage.From)
            {
                UpdateContact(persons, contactSummary);
            }
            foreach (var contactSummary in emailMessage.To)
            {
                UpdateContact(persons, contactSummary);
            }
        }

        private static void UpdateContact(IEnumerable<IPersonReferrent> persons, ContactSummary contactSummary)
        {
            var person = persons.FirstOrDefault(c => c.Emails.Contains(contactSummary.Email));
            if (person != null)
            {
                contactSummary.Contact = person.ToContact();
            }
        }

        public Task OnSaveAsync(EmailMessage emailMessage)
        {
            _logger.Info($"{emailMessage.Hash} {emailMessage.Subject}");

            return Task.CompletedTask;
        }

        /// <summary>
        /// Заполним заголовок (основное содержимое тела)
        /// </summary>
        /// <param name="emailMessage"></param>
        private void FillHeaders(IBodyPart emailMessage, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            var bodyHeaders = _mailAnalyzeService.CommonHeaders(text);
            var headerMain = bodyHeaders.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(headerMain?.Text)) return;
            //сохраним
            emailMessage.MainHeader = headerMain.Text;
            bodyHeaders.Remove(headerMain);
            if (bodyHeaders.Any())
            {
                foreach (var mailReferent in bodyHeaders)
                {
                    if (!string.IsNullOrWhiteSpace(mailReferent.Text))
                    {
                        emailMessage.CommonHeaders.Add(new HeaderSummary(mailReferent.Text));
                    }
                }
            }

        }

        private string GetText(IBodyPart emailMessage)
        {
            if (string.IsNullOrWhiteSpace(emailMessage.Body)) return null;
            //Достанем текст письма и если оно в html то приведем его в нормальный вид
            var text = emailMessage.IsBodyHtml
                ? _htmlConverterService.ConvertHtml(emailMessage.Body)
                : emailMessage.Body;
            return text;
        }

        #region Заполнение сообщения
        /// <summary>
        /// Добавить владельца
        /// </summary>
        /// <param name="emailMessage"></param>
        private void AddOwner(IContentItem emailMessage)
        {
            var mailOwnerSummary = new MailOwnerSummary(MailBox.Login, MailBox.User?.Fio);
            if (!emailMessage.Owners.Any(c => c.Equals(mailOwnerSummary)))
            {
                emailMessage.Owners.Add(mailOwnerSummary);
            }
        }
        /// <summary>
        /// заполнить направление
        /// </summary>
        /// <param name="emailMessage"></param>
        private void FillDirection(IMessagePart emailMessage)
        {
            //если в поле от есть наш пользователь значит письмо исходящее
            if (HasInMailBox(emailMessage.From))
            {
                emailMessage.Direction = Direction.Outcoming;
            }
            else
            {
                emailMessage.Direction
                    = HasInMailBox(emailMessage.To) //если в поле кому есть наша почта значит входящее во всех остальных случаях оно входящее
                        ? Direction.Incoming
                        : Direction.Outcoming;
            }
        }

        private bool HasInMailBox(ICollection<ContactSummary> contactSummaries)
        {
            var any = contactSummaries.Any(c => c.Email.Equals(MailBox.Login, StringComparison.InvariantCultureIgnoreCase)) ||
                      contactSummaries.Any(c =>
                          MailBox.User.Aliases?.Any(a => a.Equals(c.Email, StringComparison.InvariantCultureIgnoreCase)) ?? false);
            return any;
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
                if (emailMessage.Tags == null)
                {
                    emailMessage.Tags = new List<HashTag>();
                }
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
            var textParts = mimeEntities.Select(c => c.GetTextInUtf8()).ToList();
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
            //TODO скачивание файла
            /*
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
            }*/

        }

        /// <summary>
        /// Сохранить вложения
        /// </summary>
        /// <param name="message"></param>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        private async Task SaveAttachmentsAsync(MimeMessage message, EmailMessage emailMessage)
        {
            //TODO скачивание файла
            /*
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
            }*/
        }

        #endregion
    }
}
