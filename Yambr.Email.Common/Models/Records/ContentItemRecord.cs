namespace Yambr.Email.Common.Models.Records
{
    [CollectionName("Content")]
    [UploadType(UploadType.UploadAndDelete)]
    public class ContentItemRecord:Record, ISynchronizedRecord
    { }
}
