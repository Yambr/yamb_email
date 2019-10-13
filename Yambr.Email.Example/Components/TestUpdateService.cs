using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yambr.Analyzer.Models;
using Yambr.Analyzer.Services;
using Yambr.Email.Common.Models;
using Yambr.Email.Loader.Extensions;
using Yambr.Email.Loader.Services;
using Yambr.Email.Processor;
using Yambr.RabbitMQ.Models;
using Yambr.RabbitMQ.Services;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.ExtensionPoints;

namespace Yambr.Email.Example.Components
{
    [Component]
    class TestUpdateService : IInitHandler
    {
        private readonly IHtmlConverterService _htmlConverterService;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IMailAnalyzeService _mailAnalyzeService;

        private readonly MailBox MailBox = new MailBox()
        {
            Contractors = new Dictionary<string, IContractor>(),
            Contacts = new Dictionary<string, IContact>()
        };

        private  readonly Dictionary<long,long> Statictics = new Dictionary<long, long>();

        public TestUpdateService(
            IHtmlConverterService htmlConverterService,
            IRabbitMQService rabbitMQService,
            IMailAnalyzeService mailAnalyzeService)
        {
            _htmlConverterService = htmlConverterService;
            _rabbitMQService = rabbitMQService;
            _mailAnalyzeService = mailAnalyzeService;
        }

        public void Init()
        {

        }

        public void InitComplete()
        {
          
            var files = Directory.EnumerateFiles("EmailMessage").ToList();

          /*  var stat = JsonConvert.DeserializeObject<Dictionary<int, long>>(File.ReadAllText("Stat"));

            var toAnalyze = stat.OrderByDescending(c => c.Value).Take(100).Skip(10);

            foreach (var pair in toAnalyze)
            {
                var file = files[pair.Key];
                Convert(file, pair.Key);
            }*/

           //   var files = new List<string>(){ "EmailMessage\\002b8638d5c3b2f4417b6497244c9dd7.json" };
           for (int i = 0; i < files.Count(); i++)
           {
               var file = files[i];
               Convert(file, i);
           }


           Send();

           File.WriteAllText("Mailbox",JsonConvert.SerializeObject(MailBox, Formatting.Indented));
        }

        private void Send()
        {
            foreach (var contact in MailBox.Contacts)
            {
                _rabbitMQService.SendMessage(
                    RabbitMQConstants.ExchangeEmail,
                    new JsonQueueObject<IContact>(contact.Value, "Contact", RabbitMQConstants.RoutingKeyEmailEventCreated
                    ));
            }

            foreach (var contractor in MailBox.Contractors)
            {
                _rabbitMQService.SendMessage(
                    RabbitMQConstants.ExchangeEmail,
                    new JsonQueueObject<IContractor>(contractor.Value, "Contractor",
                        RabbitMQConstants.RoutingKeyEmailEventCreated
                    ));
            }
        }

        private void Convert(string file, int i)
        {
            var emailMessage = JsonConvert.DeserializeObject<EmailMessage>(File.ReadAllText(file));
            var doc = emailMessage.Body;
            var isHtml = emailMessage.IsBodyHtml;

            if (!string.IsNullOrWhiteSpace(doc))
            {
                File.WriteAllText("test.html", doc);
                var stopwatch = Stopwatch.StartNew();
                stopwatch.Start();
                const int max = 100000;
                if (!emailMessage.IsBodyHtml && emailMessage.Body.Length > max)
                {
                    return;
                }
                if (emailMessage.IsBodyHtml && emailMessage.Body.Length > (max*2))
                {
                    return;
                }
                var convertHtml = isHtml ? _htmlConverterService.ConvertHtml(doc) : doc;
                if (convertHtml.Length > max)
                {
                    return;
                }

                File.WriteAllText("test.txt", convertHtml);
                var persons = _mailAnalyzeService.Persons(convertHtml);
                if (persons.Any(c => c.Emails.Any(e =>
                {
                    var ok = (e.EndsWith(".ru") || e.EndsWith(".com") || e.EndsWith(".info") || e.EndsWith(".su") ||
                              e.EndsWith(".org") || e.EndsWith(".tech") || e.EndsWith("D297BED0"));
                    if (!ok)
                    {
                        Console.WriteLine(file);
                        Console.WriteLine(e);
                    }

                    return !ok;
                })))
                {
                    // throw new Exception("криво");
                }

                if (persons.Any())
                {
                    foreach (var contactSummary in emailMessage.From)
                    {
                        UpdateContact(persons, contactSummary);
                    }

                    foreach (var contactSummary in emailMessage.To)
                    {
                        UpdateContact(persons, contactSummary);
                    }
                }

                stopwatch.Stop();
                Statictics.Add(i, GC.GetTotalMemory(true));
                
                Console.WriteLine($"{i} {stopwatch.ElapsedMilliseconds}");
            }
        }

        private void UpdateContact(IEnumerable<IPersonReferrent> persons, ContactSummary contactSummary)
        {
            var contactSummaryEmail = contactSummary.Email;
            var person = persons.FirstOrDefault(c => c.Emails.Contains(contactSummaryEmail));
            if (person == null) return;
            var newContact = person.ToContact();
            MailBox.Contacts[contactSummaryEmail] = Merge(newContact, contactSummaryEmail);
            if (newContact.Contractor == null) return;
            var domain = Domain(contactSummaryEmail);
            MailBox.Contractors[domain] = Merge((Contractor)newContact.Contractor, domain);
        }

        private Contact Merge(Contact newContact, string contactSummaryEmail)
        {
            if (MailBox.Contacts.TryGetValue(contactSummaryEmail, out IContact oldContact))
            {
                var oldPhones = oldContact.Phones.Except(newContact.Phones).ToList();
                if (oldPhones.Any())
                {
                    foreach (var oldPhone in oldPhones)
                    {
                        newContact.Phones.Add(oldPhone);
                    }
                }
                //TODO email?
            }

            return newContact;
        }
        private Contractor Merge(Contractor contractor, string domain)
        {
            contractor.Domains = new List<Domain>()
            {
                new Domain()
                {
                    DomainString = domain
                }
            };
            return contractor;
        }

        private static string Domain(string email)
        {
            return email.Split(new[] { '@' }, StringSplitOptions.None)[1]?.ToLowerInvariant();
        }
    }
}
