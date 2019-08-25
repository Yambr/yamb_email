using System.Collections.Generic;
using System.Linq;
using EP.Ner;
using EP.Ner.Mail;
using Yambr.Analyzer.Models;
using Yambr.Analyzer.Services;
using Yambr.SDK.ComponentModel;

namespace Yambr.Analyzer.Pullenti.Services
{
    [Service]
    class MailAnalyzeService : IMailAnalyzeService
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
    }
}
