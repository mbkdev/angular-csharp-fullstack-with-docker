using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace core.Exceptions
{
    public class UserDeleteException : Exception
    {
        public UserDeleteException()
        {
        }

        public UserDeleteException(string? message) : base(message)
        {
        }

        public UserDeleteException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
