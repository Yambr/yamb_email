namespace Yambr.Email.Common.Models
{
    public interface IBodyPart:IBodySummaryPart
    {
        string Body { get; set; }
        string Text { get; set; }
        bool IsBodyHtml { get; set; }
    }
}