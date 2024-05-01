using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class AirportAlreadyExistsException:Exception
    {
        private string message;
        public AirportAlreadyExistsException()
        {
            message = "Airport with given details already exists";
        }

        public override string Message => message;
    }
}
