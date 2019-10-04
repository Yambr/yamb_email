using System.Collections.Generic;

namespace Yambr.Analyzer.Models
{
    public interface IPersonReferrent 
    {
        string Gender { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string Position { get; set; }
        string Description { get; set; }
        string Site { get; set; }
        ICompanyReferent Company { get; set; }
        ICollection<IPhoneReferent> Phones { get; set; }
        ICollection<string> Emails { get; set; }
    }
}