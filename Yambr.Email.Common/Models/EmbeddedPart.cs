using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public class EmbeddedPart :  IEmbeddedPart
    {
        public ICollection<EmbeddedSummary> Embedded { get; set; }
    }
}
