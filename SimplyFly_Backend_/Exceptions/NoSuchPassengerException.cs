using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchPassengerException:Exception
    {
        private readonly string message;
        public NoSuchPassengerException()
        {
            message = "No Passener found for given details";
        }

        public override string Message => message;
    }
}
