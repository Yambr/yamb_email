namespace Yambr.Email.Common.Models
{
    public class HeaderSummary
    {
        public HeaderSummary(){}

        public HeaderSummary(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
