using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchUserException:Exception
    {
        private readonly string message;
        public NoSuchUserException()
        {
            message = "No User found for given details";
        }

        public override string Message => message;
    }
}
