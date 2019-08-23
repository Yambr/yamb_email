using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Yambr.Email.Common.Enums;

namespace Yambr.Email.Common.Models
{
    public class EmailMessage :  IContentItem, IBodyPart, IMessagePart, IAttachmentsPart, ITagsPart, IEmbeddedPart
    {
        public EmailMessage(ICollection<MailOwnerSummary> owners, ICollection<HeaderSummaryPart> commonHeaders, ICollection<ContactSummary> @from, ICollection<ContactSummary> to, ICollection<AttachmentSummary> attachments, ICollection<EmbeddedSummary> embedded)
        {
            Owners = owners;
            CommonHeaders = commonHeaders;
            From = @from;
            To = to;
            Attachments = attachments;
            Embedded = embedded;
        }
        public EmailMessage() { }

        public DateTime DateUtc { get; set; }
        public string Hash { get; set; }
        public ICollection<MailOwnerSummary> Owners { get; set; }
        public string MainHeader { get; set; }
        public ICollection<HeaderSummaryPart> CommonHeaders { get; set; }
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
