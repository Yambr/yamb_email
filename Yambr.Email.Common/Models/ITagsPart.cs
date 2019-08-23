using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public interface ITagsPart
    {
        ICollection<HashTag> Tags { get; set; }
    }
}