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
        public string Fio { get; set; }
        public int UserId { get; set; }
    }
}