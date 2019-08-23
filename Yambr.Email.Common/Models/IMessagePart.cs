using System.Collections.Generic;
using Yambr.Email.Common.Enums;

namespace Yambr.Email.Common.Models
{
    public interface IMessagePart
    {
        Direction Direction { get; set; }
        ICollection<ContactSummary> From { get; set; }
        string Subject { get; set; }
        ICollection<ContactSummary> To { get; set; }
        string SubjectWithoutTags { get; set; }
    }
}