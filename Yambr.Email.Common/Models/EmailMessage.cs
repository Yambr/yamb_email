using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Yambr.Email.Common.Enums;

namespace Yambr.Email.Common.Models
{
    public class EmailMessage :  IContentItem, IBodyPart, IMessagePart, IAttachmentsPart, ITagsPart, IEmbeddedPart
    {
     
        [JsonConstructor]
        public EmailMessage(
            List<MailOwnerSummary> owners,
            List<HeaderSummary> commonHeaders,
            List<ContactSummary> @from,
            List<ContactSummary> to,
            List<AttachmentSummary> attachments,
            List<EmbeddedSummary> embedded) :this()
        {
            Owners = owners;
            CommonHeaders = commonHeaders;
            From = @from;
            To = to;
            Attachments = attachments;
            Embedded = embedded;
        }

        public EmailMessage()
        {
            Owners = new List<MailOwnerSummary>();
            CommonHeaders = new List<HeaderSummary>();
            From = new List<ContactSummary>();
            To = new List<ContactSummary>();
            Attachments = new List<AttachmentSummary>();
            Embedded = new List<EmbeddedSummary>();
        }

        public DateTime DateUtc { get; set; }
        public string Hash { get; set; }
        public ICollection<MailOwnerSummary> Owners { get; set; }
        public string MainHeader { get; set; }
        public ICollection<HeaderSummary> CommonHeaders { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public Direction Direction { get; set; }
        public ICollection<ContactSummary> From { get; set; }
        public string Subject { get; set; }
        public ICollection<ContactSummary> To { get; set; }
        public string SubjectWithoutTags { get; set; }
        public ICollection<AttachmentSummary> Attachments { get; set; }
        public ICollection<HashTag> Tags { get; set; }
        public ICollection<EmbeddedSummary> Embedded { get; set; }
    }
}
