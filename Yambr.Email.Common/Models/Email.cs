namespace Yambr.Email.Common.Models
{
    public class Email
    {
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