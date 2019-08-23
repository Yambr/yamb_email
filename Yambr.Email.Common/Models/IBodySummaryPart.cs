using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public interface IBodySummaryPart
    {
        string MainHeader { get; set; }
        ICollection<HeaderSummaryPart> CommonHeaders { get; set; }
    }
}