using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yambr.Email.Loader.Exceptions
{
    public class EmptyMessageException : EmailLoaderException
    {
        public EmptyMessageException()
        {
        }

        protected EmptyMessageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public EmptyMessageException(string message) : base(message)
        {
        }

        public EmptyMessageException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
