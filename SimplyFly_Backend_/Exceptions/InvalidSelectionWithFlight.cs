using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidSelectionWithFlight:Exception
    {
        private readonly string message;
        public InvalidSelectionWithFlight()
        {
            message = "The selection is invalid, please try again";
        }

        public override string Message => message;
    }
}
