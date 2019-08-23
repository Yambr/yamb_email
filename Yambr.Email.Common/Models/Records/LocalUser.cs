using Yambr.Email.Common.Models.Default;

namespace Yambr.Email.Common.Models.Records
{
    /// <summary>
    /// Пользователь локальной БД
    /// (к нему привязаны почтовые ящики - это реальный пользователь системы
    /// и пользватель с ящиками относительно него получаем связь с ящиками, контактами и т.д.)
    /// (этот пользователь есть также в системе основной)
    /// </summary>
    [CollectionName(nameof(LocalUser))]
    public class LocalUser : Record, ILocalUser
    {
        private string _fio;
        private int _userId;

        public string Fio
        {
            get => _fio;
            set { _fio = value;OnPropertyChanged(); }
        }

        public int UserId
        {
            get => _userId;
            set { _userId = value;OnPropertyChanged(); }
        }
    }
}