using System;
using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public class ContentItem :  IContentItem
    {
        public ContentItem(ICollection<MailOwnerSummary> owners)
        {
            Owners = owners;
        }

        public DateTime DateUtc { get; set; }
        public string Hash { get; set; }
        public ICollection<MailOwnerSummary> Owners { get; set; }
    }
}
