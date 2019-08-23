namespace Yambr.Email.Common.Models
{
    public class AttachmentSummary : ISummary
    {
        public string FileName { get; set; }

        public long? Size { get; set; }

        public MongoDBRef Ref { get; set; }
    }
}
