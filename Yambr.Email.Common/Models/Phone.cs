using System;

namespace Yambr.Email.Common.Models
{
    public class Phone : IPhone
    {
        protected bool Equals(Phone other)
        {
            return string.Equals(FormattedPhoneString, other.FormattedPhoneString);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Phone) obj);
        }

        public override int GetHashCode()
        {
            return (FormattedPhoneString != null ? FormattedPhoneString.GetHashCode() : 0);
        }

        public Phone()
        {}

        public Phone(IPhone phone)
        {
            if (phone == null) throw new ArgumentNullException(nameof(phone));
            PhoneString = phone.PhoneString;
        }
        public Phone(string phone)
        {
            PhoneString = phone;
        }
        public Phone(string phone, string formattedPhoneString)
        {
            PhoneString = phone;
            FormattedPhoneString = formattedPhoneString;
        }
        public string PhoneString
        {
            get;
            set;
        }

        public string FormattedPhoneString
        {
            get;
            set;
        }
    }
}