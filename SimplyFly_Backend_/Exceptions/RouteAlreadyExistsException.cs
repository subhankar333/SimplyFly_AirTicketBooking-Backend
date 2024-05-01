using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class RouteAlreadyExistsException:Exception
    {
        private readonly string message;
        public RouteAlreadyExistsException()
        {
            message = "The Route already exits";
        }
        public override string Message => message;
    }
}
