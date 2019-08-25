using MimeKit;
using Yambr.Email.Common.Models;
using Yambr.SDK.Extensions;

namespace Yambr.Email.Loader.Extensions
{
    public static class MimeMessageExtensions
    {
        public static string MessageHash(this MimeMessage message)
        {
            // вычислим хэш сообщения
            var messageSummary = new EmailMessageSummary
            {
                IsBodyHtml = !string.IsNullOrWhiteSpace(message.HtmlBody)
            };

            messageSummary.Body = messageSummary.IsBodyHtml ? message.HtmlBody : message.TextBody;
            messageSummary.DateUtc = message.Date.UtcDateTime;
            messageSummary.Subject = message.Subject;
            var messageHash = messageSummary.GetHashByJson();
            return messageHash;
        }
    }
}
