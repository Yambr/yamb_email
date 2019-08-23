using System.Threading.Tasks;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models;
using Yambr.SDK.ComponentModel;

namespace Yambr.Email.Loader.ExtensionPoints
{
    [ExtensionPoint]
    public interface IEmailLoader
    {
        ConnectionType ConnectionType { get; }
        Task<EmailLoadingStatus> LoadFromEmailAsync(IMailBox mailBox);
    }
}