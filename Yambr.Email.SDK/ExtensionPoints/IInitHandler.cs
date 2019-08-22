using System;
using System.Collections.Generic;
using System.Text;
using Yambr.Email.SDK.ComponentModel;

namespace Yambr.Email.SDK.ExtensionPoints
{
    [ExtensionPoint]
    public interface IInitHandler
    {
        void Init();
        void InitComplete();
    }
}
