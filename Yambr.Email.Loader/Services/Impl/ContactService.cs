using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MimeKit;
using Yambr.Email.Common.Models;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.Extensions;

namespace Yambr.Email.Loader.Services.Impl
{
    [Service]
    public class ContactService : IContactService
    {
      //  private const string ContactRegion = "Contact";
        private readonly ILogger _logger;
        private readonly IContractorService _contractorService;

        public ContactService(
            ILogger<ContactService> logger,
            IContractorService contractorService)
        {
            _contractorService = contractorService;
            _logger = logger;
        }
        /// <summary>
        /// Получить или создать контакт по ящику Email
        /// </summary>
        /// <param name="mailbox"></param>
        /// <returns></returns>
        public async Task<ContactSummary> CreateContactSummaryAsync(MailboxAddress mailbox)
        {
            var normalizedEmail = mailbox.Address.ToLowerInvariant();

            var contact = await GetOrCreateContactAsync(normalizedEmail, mailbox.Name);
            var contactSummary = new ContactSummary(normalizedEmail, contact);

          _logger.Info($"Сохранен контакт {contact.Fio} по {mailbox.Address}");
           

            return contactSummary;
        }
        /// <summary>
        /// Получить или создать контакт по ящику Email
        /// </summary>
        /// <returns></returns>
        private async Task<Contact> GetOrCreateContactAsync(string normalizedEmail, string name)
        {
            var isNew = false;
            // поискали в контактах
            var contact = CreateContact(normalizedEmail);//создаем контакт по ящику

            ExtractAndSetFio(contact, name);

            return contact;
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
        /// Достать фио
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="name"></param>
        private void ExtractAndSetFio(IContact contact, string name)
        {
           
            if (contact == null) throw new ArgumentNullException(nameof(contact));
            if (string.IsNullOrWhiteSpace(name)) return;
            name = CleanName(name);
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

        private static string CleanName(string name)
        {
            var chars = new List<char>();
            foreach (var c in name)
            {
                if(char.IsLetter(c) || char.IsWhiteSpace(c))
                    chars.Add(c);
            }

            return (new string(chars.ToArray())).Trim();
        }
    }
}
