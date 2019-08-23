namespace Yambr.Email.Common.Models
{
    
    public interface IUserSettings
    {
        string Fio { get; set; }
        string LocalDbName { get; set; }
        int UserId { get; set; }
    }
}