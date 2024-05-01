using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class FlightAlreadyExistsException:Exception
    {
        private readonly string message;
        public FlightAlreadyExistsException()
        {
            message = "Flight already exists.";
        }

        public override string Message => message;
    }
}
