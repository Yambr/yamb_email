using System;
using Yambr.SDK.ComponentModel.Enums;

namespace Yambr.SDK.ComponentModel
{
    /// <summary>
    /// Атрибут точки расширения
    /// </summary>
    public class ExtensionPointAttribute : Attribute
    {
        public ExtensionPointAttribute()
        { }

        public ExtensionPointAttribute(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; set; }
    }
}
