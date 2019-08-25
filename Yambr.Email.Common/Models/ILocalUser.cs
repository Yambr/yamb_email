using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public interface ILocalUser
    {
        string Fio { get; set; }
        int UserId { get; set; }
        ICollection<string> Aliases { get; set; }
        string OwnerQueue { get; set; }
    }
}