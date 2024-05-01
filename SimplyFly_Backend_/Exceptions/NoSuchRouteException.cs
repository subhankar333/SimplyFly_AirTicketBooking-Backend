using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchRouteException:Exception
    {
        private readonly string message;
        public NoSuchRouteException()
        {
            message = "No Route found for given details";
        }

        public override string Message => message;
    }
}
