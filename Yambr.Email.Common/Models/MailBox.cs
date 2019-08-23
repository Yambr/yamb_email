using System;
using Yambr.Email.Common.Enums;

namespace Yambr.Email.Common.Models
{
    public class MailBox : IMailBox
    {
        public MailBox(IServer server, ILocalUser user)
        {
            Server = server;
            User = user;
        }

        public DateTime LastStartTimeUtc { get; set; }
        public DateTime NextFireTimeUtc { get; set; }
        public EmailLoadingStatus Status { get; set; }
        public string LoaderError { get; set; }
        public string LastTrigger { get; set; }
        public int LoadingIntervalMinutes { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAlias { get; set; }
        public IServer Server { get; set; }
        public ILocalUser User { get; set; }
    }
}