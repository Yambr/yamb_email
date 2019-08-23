namespace Yambr.Email.Common.Models
{
    public interface IMailBox : ILoadingState
    {
        string LastTrigger { get; set; }
        int LoadingIntervalMinutes { get; set; }
        string Login { get; set; }
        string Password { get; set; }
        bool IsAlias { get; set; }
        IServer Server { get; set; }
        ILocalUser User { get; set; }
      
    }
}