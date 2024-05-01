using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchBookingsException:Exception
    {
        private readonly string message;
        public NoSuchBookingsException()
        {
            message = "No booking found for given details";
        }

        public override string Message => message;
    }
}
