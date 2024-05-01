using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class InvalidUserException:Exception
    {
        private readonly string message;

        public InvalidUserException()
        {
            message = "Invalid Username or Password";
        }

        public override string Message => message;
    }
}
