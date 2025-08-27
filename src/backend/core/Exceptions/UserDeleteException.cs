using System.Runtime.Serialization;

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
    }
}
