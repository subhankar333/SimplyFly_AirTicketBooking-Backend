using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchAirportException:Exception
    {
        private readonly string message;
        public NoSuchAirportException()
        {
            message = "Airport not found for given details";
        }

        public override string Message => message;
    }
}
