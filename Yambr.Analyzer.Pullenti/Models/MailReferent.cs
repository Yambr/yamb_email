using Yambr.Analyzer.Models;

namespace Yambr.Analyzer.Pullenti.Models
{
    internal class MailReferent : IMailReferent
    {
        private readonly EP.Ner.Mail.MailReferent _mailReferent;

        public MailReferent(EP.Ner.Mail.MailReferent mailReferent)
        {
            _mailReferent = mailReferent;
        }
        public string Text => _mailReferent.Text;
    }
}
