using Yambr.Analyzer.Models;

namespace Yambr.Analyzer.Pullenti.Models
{
    internal class PhoneReferent : IPhoneReferent
    {
        public PhoneReferent(EP.Ner.Phone.PhoneReferent phoneReferent)
        {
            PhoneString = phoneReferent.ToString();
            FormattedPhoneString = phoneReferent.Number;
        }

        public string FormattedPhoneString { get; set; }

        public string PhoneString { get; set; }
    }
}