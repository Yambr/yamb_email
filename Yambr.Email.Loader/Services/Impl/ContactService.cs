using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MimeKit;
using Yambr.DistributedCache.Services;
using Yambr.Email.Common.Models;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.Extensions;

namespace Yambr.Email.Loader.Services.Impl
{
    [Service]
    public class ContactService : IContactService
    {
        private const string ContactRegion = "Contact";
        private readonly ILogger _logger;
        private readonly IContractorService _contractorService;
        private readonly ICacheService _cacheService;

        public ContactService(
            ILogger<ContactService> logger,
            IContractorService contractorService,
            ICacheService cacheService)
        {
            _contractorService = contractorService;
            _cacheService = cacheService;
            _logger = logger;
        }
        /// <summary>
        /// Получить или создать контакт по ящику Email
        /// </summary>
        /// <param name="mailbox"></param>
        /// <returns></returns>
        public async Task<ContactSummary> GetOrCreateContactSummaryAsync(MailboxAddress mailbox)
        {
            var normalizedEmail = mailbox.Address.ToLowerInvariant();

            var contact = await GetOrCreateContactAsync(normalizedEmail, mailbox.Name);
            var contactSummary = new ContactSummary(normalizedEmail, contact);

          
            await _cacheService.InsertAsync(normalizedEmail,ContactRegion, TimeSpan.FromDays(30));
                _logger.Info($"Сохранен контакт {contact.Fio} по {mailbox.Address}");
           

            return contactSummary;
        }
        /// <summary>
        /// Получить или создать контакт по ящику Email
        /// </summary>
        /// <returns></returns>
        private async Task<Contact> GetOrCreateContactAsync(string normalizedEmail, string name)
        {
            // поискали в контактах
            var contact = await GetContactByEmailAsync(normalizedEmail);
            if (contact == null)
            {
                // создаем на основе ящика или email
                contact = CreateContact(normalizedEmail);//создаем контакт по ящику
                //т.к. контакт найден среди наших или был создан 
                //то по домену попробуем найти контрагента
                await FillContractorAsync(normalizedEmail, contact);
            }
            ExtractAndSetFio(contact, name);

            return contact;
        }
        /// <summary>
        /// Получить контакт по ящику Email
        /// </summary>
        /// <returns></returns>
        private async Task<Contact> GetContactByEmailAsync(string normalizedEmail)
        {
            return await _cacheService.GetAsync<Contact>(normalizedEmail, ContactRegion);
        }
        /// <summary>
        /// Создать контакт на основе ящика
        /// </summary>
        /// <param name="normalizedEmail"></param>
        /// <returns></returns>
        private  Contact CreateContact(string normalizedEmail)
        {
            if (normalizedEmail == null) throw new ArgumentNullException(nameof(normalizedEmail));
            var contact = new Contact();
            contact.Emails.Add(new Common.Models.Email(normalizedEmail));
            return contact;
        }

        /// <summary>
        /// Заполнить контрагента если не заполнен
        /// </summary>
        /// <param name="normalizedEmail"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        private async Task FillContractorAsync(string normalizedEmail, Contact contact)
        {
            if (contact == null) throw new ArgumentNullException(nameof(contact));
            var mailAddress = normalizedEmail.Split(new[] { "@" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            var contractor = await _contractorService.GetOrCreateContractorSummaryByDomainAsync(mailAddress);
            AddContractor(contact, contractor);
        }

        private static void AddContractor(Contact contact, ContractorSummary contractor)
        {
            if (contractor == null) return;
            if (contact.Contractors.All(c => !c.Equals(contractor)))
            {
                contact.Contractors.Add(contractor);
            }
        }

        /// <summary>
        /// Достать фио
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="name"></param>
        private void ExtractAndSetFio(IContact contact, string name)
        {
            if (contact == null) throw new ArgumentNullException(nameof(contact));
            if (string.IsNullOrWhiteSpace(name)) return;
            if (string.IsNullOrWhiteSpace(contact.Fio))
            {
                contact.Fio = name;
            }

            //TODO проверка и доставание из подписи фио а пока по размеру сравниваем
            if (contact.Fio.Length - name.Length < 0)
            {
                contact.Fio = name;
            }
        }

    }
}
