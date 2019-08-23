using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public class BodyPart : BodySummaryPart, IBodyPart
    {
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
    }

    public class BodySummaryPart :  IBodySummaryPart
    {
        public string MainHeader { get; set; }
        public ICollection<HeaderSummaryPart> CommonHeaders { get; set; }
    }

}
