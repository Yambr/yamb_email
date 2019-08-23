namespace Yambr.Email.Common.Models.Default
{
    public interface ILocalUser:IRecord
    {
        string Fio { get; set; }
        int UserId { get; set; }
    }
}