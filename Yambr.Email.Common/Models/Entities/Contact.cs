using System;
using System.Collections.ObjectModel;
using Yambr.Email.Common.Models.Default;
using Yambr.Email.Common.Models.Records;
using Yambr.Email.Common.Models.Records.Nested;

namespace Yambr.Email.Common.Models.Entities
{
    public class Contact : Entity<ContactRecord>, IContact
    {
        private readonly LazyField<ILocalUser> _localUserField = new LazyField<ILocalUser>();
        private readonly LazyField<IRecordCollection<LocalUser>> _localUserRecordCollectionField = new LazyField<IRecordCollection<LocalUser>>();

        private readonly IComponentContext _context;


        public Contact(IComponentContext context)
        {
            _context = context;
            Activation();
        }

        public Contact(IComponentContext context, [NotNull] ContactRecord record) : base(record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            _context = context;
            Activation();
        }

        protected void Activation()
        {
            _localUserRecordCollectionField.Loader(() => _context.Resolve<IRecordCollection<LocalUser>>());
            _localUserField.Setter(c =>
            {
                if (c != null)
                {
                    Record.User = c.CreateDBRef();
                }
                return c;
            });
            _localUserField.Loader(() =>
            {
                if (Record?.User?.Id == null || Record.User.Id.Equals(ObjectId.Empty)) return null;
                return _localUserRecordCollectionField.Value.LoadSync<LocalUser>(Record.User.Id);
            });
        }

        public ObservableCollection<Email> Emails
        {
            get => Record.Emails;
            set => Record.Emails = value;
        }

        public string Fio
        {
            get => Record.Fio;
            set => Record.Fio = value;
        }

        public ObservableCollection<Phone> Phones
        {
            get => Record.Phones;
            set => Record.Phones = value;
        }

        public ILocalUser User
        {
            get => _localUserField.Value;
            set => _localUserField.Value = value;
        }
        public ObservableCollection<ContractorSummary> Contractors
        {
            get => Record.Contractors;
            set => Record.Contractors = value;
        }
    }
}
