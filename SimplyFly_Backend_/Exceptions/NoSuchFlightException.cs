using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchFlightException:Exception
    {
        private readonly string message;
        public NoSuchFlightException()
        {
            message = "No flight found for give details";
        }
        public override string Message => message;
    }
}
