using System;
using Yambr.SDK.ComponentModel.Enums;

namespace Yambr.SDK.ComponentModel
{
    public class ServiceAttribute :Attribute
    {
        public ServiceAttribute()
        { }

        public ServiceAttribute(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; set; }
    }
}
