using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yambr.Email.Loader.Exceptions
{
    public class TooBigMessageException : EmailLoaderException
    {
        public TooBigMessageException()
        {
        }

        protected TooBigMessageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TooBigMessageException(string message) : base(message)
        {
        }

        public TooBigMessageException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
