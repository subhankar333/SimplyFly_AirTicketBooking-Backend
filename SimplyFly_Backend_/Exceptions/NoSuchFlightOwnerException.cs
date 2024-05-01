using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchFlightOwnerException:Exception
    {
        private readonly string message;
        public NoSuchFlightOwnerException()
        {
            message = "No FlightOwner found for given details";
        }

        public override string Message => message;
    }
}
