using System.Collections.Generic;
using Yambr.Email.Common.Enums;

namespace Yambr.Email.Common.Models
{
    public sealed class MessagePart : IMessagePart
    {
        public Direction Direction { get; set; }
        public ICollection<ContactSummary> From { get; set; }
        public string Subject { get; set; }
        public ICollection<ContactSummary> To { get; set; }
        public string SubjectWithoutTags { get; set; }
    }
}
