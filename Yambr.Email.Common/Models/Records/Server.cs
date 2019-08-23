using Yambr.Email.Common.Enums;

namespace Yambr.Email.Common.Models.Records
{
    [CollectionName(nameof(Server))]
    public class Server : Record, IServer
    {
        private string _host;
        private int _port;
        private bool _useSsl;
        private ConnectionType _connectionType;

        public string Host
        {
            get => _host;
            set { _host = value; OnPropertyChanged(); }
        }

        public int Port
        {
            get => _port;
            set { _port = value; OnPropertyChanged(); }
        }

        public bool UseSsl
        {
            get => _useSsl;
            set { _useSsl = value; OnPropertyChanged(); }
        }

        public ConnectionType ConnectionType
        {
            get => _connectionType;
            set { _connectionType = value; OnPropertyChanged(); }
        }
    }
}