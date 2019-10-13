using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yambr.Email.Loader.Exceptions
{
    public class EmailLoaderException : Exception
    {
        public EmailLoaderException()
        {
        }

        protected EmailLoaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public EmailLoaderException(string message) : base(message)
        {
        }

        public EmailLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
