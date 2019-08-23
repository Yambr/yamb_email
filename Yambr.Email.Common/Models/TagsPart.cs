using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public class TagsPart : ITagsPart
    {
        public ICollection<HashTag> Tags { get; set; }
    }
}
