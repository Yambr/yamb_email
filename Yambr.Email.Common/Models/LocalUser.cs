using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yambr.Email.Common.Models
{
    /// <summary>
    /// Пользователь локальной БД
    /// (к нему привязаны почтовые ящики - это реальный пользователь системы
    /// и пользватель с ящиками относительно него получаем связь с ящиками, контактами и т.д.)
    /// (этот пользователь есть также в системе основной)
    /// </summary>
    public class LocalUser :  ILocalUser
    {
        [JsonConstructor]
        public LocalUser(List<string> aliases)
        {
            Aliases = aliases;
        }

        public LocalUser()
        {
        }

        public string Fio { get; set; }
        public int UserId { get; set; }
        public ICollection<string> Aliases { get; set; }
        public string OwnerQueue { get; set; }
    }
}