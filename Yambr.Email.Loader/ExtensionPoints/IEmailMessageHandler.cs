using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using Yambr.Email.Common.Models;
using Yambr.SDK.ComponentModel;
using Yambr.SDK.ComponentModel.Enums;

namespace Yambr.Email.Loader.ExtensionPoints
{
    [ExtensionPoint(Scope.InstancePerLifetimeScope)]
    public interface IEmailMessageHandler
    {
        IMailBox MailBox { get;}
        Task<EmailMessage> OnCreate(MimeMessage message, EmailMessage emailMessage);
        Task OnSaveAsync(EmailMessage emailMessage);
    }
}
