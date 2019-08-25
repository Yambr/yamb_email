using System;
using Newtonsoft.Json;
using Yambr.Email.Common.Enums;

namespace Yambr.Email.Common.Models
{
    public class MailBox : IMailBox
    {
        [JsonConstructor]
        public MailBox(Server server, LocalUser user)
        {
            Server = server;
            User = user;
        }

        public MailBox()
        {
        }

        public DateTime LastStartTimeUtc { get; set; }
        public EmailLoadingStatus Status { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public IServer Server { get; set; }
        public ILocalUser User { get; set; }
    }
}