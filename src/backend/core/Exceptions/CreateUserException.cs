using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace core.Exceptions
{
    public class CreateUserException : Exception
    {
        public CreateUserException()
        {
        }

        public CreateUserException(string? message) : base(message)
        {
        }

        public CreateUserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CreateUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
