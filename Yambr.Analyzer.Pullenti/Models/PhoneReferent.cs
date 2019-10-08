using Yambr.Analyzer.Models;

namespace Yambr.Analyzer.Pullenti.Models
{
    internal class PhoneReferent : IPhoneReferent
    {
        protected bool Equals(PhoneReferent other)
        {
            return string.Equals(FormattedPhoneString, other.FormattedPhoneString);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PhoneReferent) obj);
        }

        public override int GetHashCode()
        {
            return (FormattedPhoneString != null ? FormattedPhoneString.GetHashCode() : 0);
        }
        
        public PhoneReferent(EP.Ner.Phone.PhoneReferent phoneReferent)
        {
            PhoneString = phoneReferent.ToString();
            FormattedPhoneString = phoneReferent.Number;
        }

        public string FormattedPhoneString { get; set; }

        public string PhoneString { get; set; }
    }
}