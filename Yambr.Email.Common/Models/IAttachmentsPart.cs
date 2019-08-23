using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public interface IAttachmentsPart
    {
        ICollection<AttachmentSummary> Attachments { get; set; }
    }
}