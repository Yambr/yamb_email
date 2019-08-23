using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Yambr.Email.Common.Enums;
using Yambr.Email.Common.Models.Default;
using Yambr.Email.Common.Models.Parts;
using Yambr.Email.Common.Models.Parts.Default;
using Yambr.Email.Common.Models.Records;
using Yambr.Email.Common.Models.Records.Nested;

namespace Yambr.Email.Common.Models
{
    public class EmailMessage : ContentItemRecord, IContentItem, IBodyPart, IMessagePart, IAttachmentsPart, ITagsPart, IEmbeddedPart
    {
        private readonly IContentItem _contentItemImplementation;
        private readonly IBodyPart _bodyPartImplementation;
        private readonly IMessagePart _messagePartImplementation;
        private readonly ReadOnlyCollection<IRecord> _records;
        private readonly IAttachmentsPart _attachmentsPartImplementation;
        private readonly ITagsPart _tagsPartImplementation;
        private readonly IEmbeddedPart _embeddedPartImplementation;

        public EmailMessage()
        {
            _contentItemImplementation = new ContentItem(nameof(EmailMessage));
            _contentItemImplementation.PropertyChanged += ImplementationOnPropertyChanged;

            _bodyPartImplementation = new BodyPart();
            _bodyPartImplementation.PropertyChanged += ImplementationOnPropertyChanged;

            _messagePartImplementation = new MessagePart();
            _messagePartImplementation.PropertyChanged += ImplementationOnPropertyChanged;

            _attachmentsPartImplementation = new AttachmentsPart();
            _attachmentsPartImplementation.PropertyChanged += ImplementationOnPropertyChanged;

            _embeddedPartImplementation = new EmbeddedPart();
            _embeddedPartImplementation.PropertyChanged += ImplementationOnPropertyChanged;

            _tagsPartImplementation = new TagsPart();
            _tagsPartImplementation.PropertyChanged += ImplementationOnPropertyChanged;

            _records = new ReadOnlyCollection<IRecord>(new List<IRecord>
            {
                _contentItemImplementation,
                _bodyPartImplementation,
                _messagePartImplementation,
                _attachmentsPartImplementation,
                _embeddedPartImplementation,
                _tagsPartImplementation
            });
        }

        #region Служебное

        private void ImplementationOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(propertyChangedEventArgs.PropertyName);
        }

        public override void ClearChangedProperties()
        {
            foreach (var record in _records)
            {
                record.ClearChangedProperties();
            }
            base.ClearChangedProperties();
        }

        public string ContentType => _contentItemImplementation.ContentType;

        #endregion

        public DateTime DateUtc
        {
            get => _contentItemImplementation.DateUtc;
            set => _contentItemImplementation.DateUtc = value;
        }

        public string Hash
        {
            get => _contentItemImplementation.Hash;
            set => _contentItemImplementation.Hash = value;
        }

        public ObservableCollection<MailOwnerSummary> Owners
        {
            get => _contentItemImplementation.Owners;
            set => _contentItemImplementation.Owners = value;
        }

        public string Body
        {
            get => _bodyPartImplementation.Body;
            set => _bodyPartImplementation.Body = value;
        }
        public ObservableCollection<HeaderSummaryPart> CommonHeaders
        {
            get => _bodyPartImplementation.CommonHeaders;
            set => _bodyPartImplementation.CommonHeaders = value;
        }

        public string MainHeader
        {
            get => _bodyPartImplementation.MainHeader;
            set => _bodyPartImplementation.MainHeader = value;
        }

        public bool IsBodyHtml
        {
            get => _bodyPartImplementation.IsBodyHtml;
            set => _bodyPartImplementation.IsBodyHtml = value;
        }

        public Direction Direction
        {
            get => _messagePartImplementation.Direction;
            set => _messagePartImplementation.Direction = value;
        }

        public ObservableCollection<ContactSummary> From
        {
            get => _messagePartImplementation.From;
            set => _messagePartImplementation.From = value;
        }

        public string Subject
        {
            get => _messagePartImplementation.Subject;
            set => _messagePartImplementation.Subject = value;
        }
        public string SubjectWithoutTags
        {
            get => _messagePartImplementation.SubjectWithoutTags;
            set => _messagePartImplementation.SubjectWithoutTags = value;
        }

        public ObservableCollection<ContactSummary> To
        {
            get => _messagePartImplementation.To;
            set => _messagePartImplementation.To = value;
        }

        public ObservableCollection<AttachmentSummary> Attachments
        {
            get => _attachmentsPartImplementation.Attachments;
            set => _attachmentsPartImplementation.Attachments = value;
        }

        public ObservableCollection<HashTag> Tags
        {
            get => _tagsPartImplementation.Tags;
            set => _tagsPartImplementation.Tags = value;
        }

        public ObservableCollection<EmbeddedSummary> Embedded
        {
            get => _embeddedPartImplementation.Embedded;
            set => _embeddedPartImplementation.Embedded = value;
        }

        
    }
}
