namespace Yambr.Email.Common.Models
{
    public class Email
    {
        protected bool Equals(Email other)
        {
            return string.Equals(EmailString, other.EmailString);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Email) obj);
        }

        public override int GetHashCode()
        {
            return (EmailString != null ? EmailString.GetHashCode() : 0);
        }

        public Email()
        {}
        public Email(string email)
        {
            EmailString = email;
        }
        public string EmailString {
            get;
            set;
        }
    }
}