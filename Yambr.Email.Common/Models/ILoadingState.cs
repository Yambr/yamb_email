using System;
using Yambr.Email.Common.Enums;

namespace Yambr.Email.Common.Models
{
    public interface ILoadingState
    {
      
        DateTime LastStartTimeUtc { get; set; }
        EmailLoadingStatus Status { get; set; }
    }
}