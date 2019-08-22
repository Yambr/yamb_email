using System;
using System.Collections.Generic;
using System.Text;
using Yambr.Email.Example.ExtensionPonts;
using Yambr.Email.SDK.ComponentModel;

namespace Yambr.Email.Example.Components
{
    [Component]
    class TestComponent : ITestExtensionPoint
    {
        public void Test(string name)
        {
            Console.WriteLine(name);
        }
    }
}
