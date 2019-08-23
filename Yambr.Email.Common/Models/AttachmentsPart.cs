using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public class AttachmentsPart : IAttachmentsPart
    {
        public ICollection<AttachmentSummary> Attachments { get; set; }
    }
}
