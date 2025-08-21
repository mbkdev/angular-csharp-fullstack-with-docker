using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace core.Exceptions
{
    public class InvalidEmailOrPasswordException : Exception
    {
        public InvalidEmailOrPasswordException()
        {
        }

        public InvalidEmailOrPasswordException(string? message) : base(message)
        {
        }

        public InvalidEmailOrPasswordException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidEmailOrPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
