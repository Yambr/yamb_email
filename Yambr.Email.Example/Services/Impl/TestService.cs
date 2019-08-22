using System;
using System.Collections.Generic;
using System.Text;
using Yambr.Email.SDK.ComponentModel;

namespace Yambr.Email.Example.Services.Impl
{
    [Service]
    public class TestService : ITestService
    {
        public void Test(string main)
        {
            Console.WriteLine(main);
        }
    }
}
