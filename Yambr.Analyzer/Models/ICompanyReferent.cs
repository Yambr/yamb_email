namespace Yambr.Analyzer.Models
{
    public interface ICompanyReferent
    {
        string Name { get; set; }
        string INN { get; set; }
        string OGRN { get; set; }
        string Description { get; set; }
        string Site { get; set; }

    }
}