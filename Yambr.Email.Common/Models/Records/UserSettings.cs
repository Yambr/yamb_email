using System;
using System.Collections.ObjectModel;
using Yambr.Email.Common.Enums;

namespace Yambr.Email.Common.Models.Records
{
    /// <summary>
    /// Настройки пользователя (первоначальные настройки для коннекта к БД) 
    /// (для компании это первый зарегистрированный пользователь, все остальные 
    /// будут цепльяться к его настройкам чтобы пользоваться общей БД т.к. им нужен смежный доступ)
    /// </summary>
    [CollectionName(nameof(UserSettings))]
    public class UserSettings : Record, IUserSettings
    {
        public UserSettings()
        {
            Synchronizations = new ObservableCollection<CollectionSynchronize>();
        }
        private string _fio;
        private string _localDbName;
        private int _userId;
        private UserStatus _status;
        private DateTime? _lastSynchronize;
        private ObservableCollection<CollectionSynchronize> _synchronizations;

        public string Fio
        {
            get => _fio;
            set { _fio = value; OnPropertyChanged(); }
        }

        public string LocalDbName
        {
            get => _localDbName;
            set { _localDbName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Id внешний - ссылается на пользователя в веб приложении
        /// </summary>
        public int UserId
        {
            get => _userId;
            set { _userId = value; OnPropertyChanged(); }
        }

        public UserStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CollectionSynchronize> Synchronizations
        {
            get => _synchronizations;
            set
            {
                _synchronizations = value;
                _synchronizations.CollectionChanged += (sender, args) => OnPropertyChanged();
                OnPropertyChanged();
            }
        }
    }

    public class CollectionSynchronize
    {
        public DateTime? LastSynchronize { get; set; }
        public string CollectionName { get; set; }
    }
}
