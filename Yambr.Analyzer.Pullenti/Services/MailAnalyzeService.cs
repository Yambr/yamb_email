﻿using System.Collections.Generic;
using System.Linq;
using EP.Ner;
using EP.Ner.Address;
using EP.Ner.Bank;
using EP.Ner.Geo;
using EP.Ner.Mail;
using EP.Ner.Named;
using EP.Ner.Org;
using EP.Ner.Person;
using EP.Ner.Phone;
using EP.Ner.Uri;
using Yambr.Analyzer.Models;
using Yambr.Analyzer.Pullenti.Models;
using Yambr.Analyzer.Services;
using Yambr.SDK.ComponentModel;
using MailReferent = EP.Ner.Mail.MailReferent;

namespace Yambr.Analyzer.Pullenti.Services
{
    [Service]
    public class MailAnalyzeService : IMailAnalyzeService
    {
        public ICollection<IMailReferent> CommonHeaders(string text)
        {
            using (var proc = ProcessorService.CreateSpecificProcessor(MailAnalyzer.ANALYZER_NAME))
            {
                // анализируем текст
                var result = proc.Process(new SourceOfAnalysis(text));
                //достанем только почтовые блоки
                var emailBlocks = result.Entities.OfType<MailReferent>();
                //достанем блок с телом
                return emailBlocks
                    .Where(c => c.Kind == MailKind.Body)
                    .Select(c => (IMailReferent)new Models.MailReferent(c))
                    .ToList();
            }
        }

        public ICollection<IPersonReferrent> Persons(string text)
        {
            var persons = new List<IPersonReferrent>();
            using (var proc = ProcessorService.CreateSpecificProcessor(
                string.Join(",", new List<string>()
                {
                    UriAnalyzer.ANALYZER_NAME,
                    PhoneAnalyzer.ANALYZER_NAME,
                    BankAnalyzer.ANALYZER_NAME,
                    GeoAnalyzer.ANALYZER_NAME,
                    AddressAnalyzer.ANALYZER_NAME,
                    OrganizationAnalyzer.ANALYZER_NAME,
                    PersonAnalyzer.ANALYZER_NAME,
                    NamedEntityAnalyzer.ANALYZER_NAME
                })))
            {
                // анализируем текст
                var analysisResult = proc.Process(new SourceOfAnalysis(CleanText(text)));
                persons.AddRange(
                    analysisResult.Entities.OfType<PersonReferent>()
                        .Select(personReferent => new PersonReferrent(personReferent))
                        .Cast<IPersonReferrent>());
            }

            return persons;
        }
        private string CleanText(string s)
        {
            s = s.Replace("<", " ");
            s = s.Replace(">", " ");
            return s;
        }
    }
}
