using System.Collections.Generic;
using Yambr.Analyzer.Models;

namespace Yambr.Analyzer.Pullenti.Models
{
    internal interface IPersonStat
    {
        string Gender { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string Position { get; set; }
        string Description { get; set; }
        string Site { get; set; }
    }
}