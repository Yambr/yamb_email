using System;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models.Entities;

namespace Yambr.Email.Common.Models.Records
{
    [CollectionName(nameof(MailBox))]
    [UploadType(UploadType.SynchronizeAll)]
    public class MailBoxRecord : Record, ISynchronizedRecord
    {
        private MongoDBRef _server;
        private string _login;
        private string _password;
        private DateTime _lastStartTimeUtc;
        private DateTime _nextFireTimeUtc;
        private int _loadingIntervalMinutes;
        private EmailLoadingStatus _emailLoadingStatus;
        private string _lastTriggerName;
        private string _loaderError;
        private MongoDBRef _user;
        private bool _isAlias;

        public MongoDBRef Server
        {
            get => _server;
            set { _server = value; OnPropertyChanged(); }
        }

        public MongoDBRef User
        {
            get => _user;
            set { _user = value;OnPropertyChanged(); }
        }

        public string Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }
        
        public DateTime LastStartTimeUtc
        {
            get => _lastStartTimeUtc;
            set { _lastStartTimeUtc = value; OnPropertyChanged(); }
        }

        public DateTime NextFireTimeUtc
        {
            get => _nextFireTimeUtc;
            set { _nextFireTimeUtc = value; OnPropertyChanged(); }
        }

        public string LastTriggerName
        {
            get => _lastTriggerName;
            set { _lastTriggerName = value; OnPropertyChanged(); }
        }

        public int LoadingIntervalMinutes
        {
            get => _loadingIntervalMinutes;
            set { _loadingIntervalMinutes = value; OnPropertyChanged(); }
        }

        public EmailLoadingStatus EmailLoadingStatus
        {
            get => _emailLoadingStatus;
            set { _emailLoadingStatus = value; OnPropertyChanged(); }
        }

        [BsonIgnoreIfNull]
        public string LoaderError
        {
            get => _loaderError;
            set { _loaderError = value; OnPropertyChanged(); }
        }

        [BsonIgnoreIfDefault]
        public bool IsAlias
        {
            get => _isAlias;
            set { _isAlias = value; OnPropertyChanged(); }
        }

        public override string ToString()
        {
            return Login ?? base.ToString();
        }
    }
}