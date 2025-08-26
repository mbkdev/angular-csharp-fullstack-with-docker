using System.Runtime.Serialization;

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
