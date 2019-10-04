using System;

namespace Yambr.Email.Common.Models
{
    public class Phone : IPhone
    {
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