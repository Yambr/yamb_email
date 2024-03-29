﻿using System.Collections.Generic;

namespace Yambr.Email.Common.Models
{
    public interface IMailBox : ILoadingState
    {
        string Login { get; set; }
        string Password { get; set; }
        IServer Server { get; set; }
        ILocalUser User { get; set; }

        Dictionary<string, IContact> Contacts { get; set; }
        Dictionary<string, IContractor> Contractors { get; set; }
    }
}