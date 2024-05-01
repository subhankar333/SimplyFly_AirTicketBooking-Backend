using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class UserAlreadyExistsException:Exception
    {
        private readonly string message;
        public UserAlreadyExistsException()
        {
            message = "User with username already exists";
        }
        public override string Message => message;
    }
}
