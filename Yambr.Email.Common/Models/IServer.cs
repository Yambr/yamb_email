using Yambr.Email.Common.Enums;

namespace Yambr.Email.Common.Models
{
    public interface IServer
    {
        string Host { get; set; }
        int Port { get; set; }
        bool UseSsl { get; set; }

        ConnectionType ConnectionType { get; set; }
    }
}