using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
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

        public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }
        }

        [DoesNotReturn]
        internal static void Throw(string? paramName) =>
           throw new CreateUserException(paramName);
    }
}
