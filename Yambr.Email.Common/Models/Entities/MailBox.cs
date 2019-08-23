using System;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models.Default;
using Yambr.Email.Common.Models.Records;

namespace Yambr.Email.Common.Models.Entities
{
    public class MailBox : Entity<MailBoxRecord>, IMailBox
    {
        private readonly LazyField<IServer> _serverField = new LazyField<IServer>();
        private readonly LazyField<IRecordCollection<Server>> _serverRecordCollectionField = new LazyField<IRecordCollection<Server>>();
        private readonly LazyField<ILocalUser> _localUserField = new LazyField<ILocalUser>();
        private readonly LazyField<IRecordCollection<LocalUser>> _localUserRecordCollectionField = new LazyField<IRecordCollection<LocalUser>>();

        private readonly IComponentContext _context;

        public MailBox(IComponentContext context) 
        {
            _context = context;
            Activation();
        }

        public MailBox(IComponentContext context, MailBoxRecord record) : base(record)
        {
            _context = context;
            Activation();
        }
        
        protected void Activation()
        {
            _serverRecordCollectionField.Loader(() => _context.Resolve<IRecordCollection<Server>>());
            _serverField.Setter(c =>
            {
                if (c != null)
                {
                    Record.Server = c.CreateDBRef();
                }
                return c;
            });
            _serverField.Loader(() =>
            {
                if (Record?.Server?.Id == null || Record.Server.Id.Equals(ObjectId.Empty)) return null;
                return _serverRecordCollectionField.Value.LoadSync<Server>(Record.Server.Id);
            });
            _localUserRecordCollectionField.Loader(() => _context.Resolve<IRecordCollection<LocalUser>>());
            _localUserField.Setter(c =>
            {
                if (c != null)
                {
                    Record.User = c.CreateDBRef();
                }
                return c;
            });
            _localUserField.Loader(() =>
            {
                if (Record?.User?.Id == null || Record.User.Id.Equals(ObjectId.Empty)) return null;
                return _localUserRecordCollectionField.Value.LoadSync<LocalUser>(Record.User.Id);
            });
        }

       
        public IServer Server
        {
            get => _serverField.Value;
            set => _serverField.Value = value;
        }

        public ILocalUser User
        {
            get => _localUserField.Value;
            set => _localUserField.Value = value;
        }

        public bool IsAlias
        {
            get => Record.IsAlias;
            set => Record.IsAlias = value;
        }

        public string Login
        {
            get => Record.Login;
            set => Record.Login = value;
        }

        public string Password
        {
            get => Record.Password;
            set => Record.Password = value;
        }

        public int LoadingIntervalMinutes
        {
            get => Record.LoadingIntervalMinutes;
            set => Record.LoadingIntervalMinutes = value;
        }
        
        public DateTime LastStartTimeUtc
        {
            get => Record.LastStartTimeUtc;
            set => Record.LastStartTimeUtc = value;
        }
        public DateTime NextFireTimeUtc
        {
            get => Record.NextFireTimeUtc;
            set => Record.NextFireTimeUtc = value;
        }

        public EmailLoadingStatus Status
        {
            get => Record.EmailLoadingStatus;
            set => Record.EmailLoadingStatus = value;
        }

        public string LastTrigger
        {
            get => Record.LastTriggerName;
            set => Record.LastTriggerName = value;
        }
        public string LoaderError
        {
            get => Record.LoaderError;
            set => Record.LoaderError = value;
        }
    }
}