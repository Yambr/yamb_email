using System;

namespace Yambr.Email.Common.Models
{
    /// <summary>
    /// Настройки пользователя (первоначальные настройки для коннекта к БД) 
    /// (для компании это первый зарегистрированный пользователь, все остальные 
    /// будут цепльяться к его настройкам чтобы пользоваться общей БД т.к. им нужен смежный доступ)
    /// </summary>
    public class UserSettings : IUserSettings
    {
        public string Fio { get; set; }
        public string LocalDbName { get; set; }
        public int UserId { get; set; }
    }

    //TODO удалить
    public class CollectionSynchronize
    {
        public DateTime? LastSynchronize { get; set; }
        public string CollectionName { get; set; }
    }
}
