using System;
using System.Collections.Generic;
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
            Contacts = new Dictionary<string, IContact>();
            Contractors = new Dictionary<string, IContractor>();
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
        [JsonIgnore]
        public Dictionary<string, IContact> Contacts { get; set; }
        [JsonIgnore]
        public Dictionary<string, IContractor> Contractors { get; set; }
        public string Error { get; set; }
        public object Id { get; set; }
    }
}