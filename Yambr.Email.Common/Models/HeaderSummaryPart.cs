namespace Yambr.Email.Common.Models
{
    public class HeaderSummaryPart
    {
        public HeaderSummaryPart(){}

        public HeaderSummaryPart(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
