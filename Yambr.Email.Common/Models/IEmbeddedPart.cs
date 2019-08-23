using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public interface IEmbeddedPart
    {
        ICollection<EmbeddedSummary> Embedded { get; set; }
    }
}