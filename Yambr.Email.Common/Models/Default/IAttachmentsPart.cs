using System.Collections.ObjectModel;

namespace Yambr.Email.Common.Models.Default
{
    public interface IAttachmentsPart: IRecord
    {
        ObservableCollection<AttachmentSummary> Attachments { get; set; }
    }
}