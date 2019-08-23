using System;

namespace Yambr.Email.Common.Models
{
    public class DataSource
    {
        public DataSource()
        {}

        public DataSource(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; set; }
    }
}