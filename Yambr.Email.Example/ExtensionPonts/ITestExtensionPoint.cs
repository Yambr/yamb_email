using System;
using System.Collections.Generic;
using System.Text;
using Yambr.Email.SDK.ComponentModel;

namespace Yambr.Email.Example.ExtensionPonts
{
    [ExtensionPoint]
    interface ITestExtensionPoint
    {
        void Test(string name);
    }
}
