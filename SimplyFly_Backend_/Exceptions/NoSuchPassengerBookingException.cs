using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchPassengerBookingException:Exception
    {
        private readonly string message;
        public NoSuchPassengerBookingException()
        {
            message = "No PassengerBooking found for given details";
        }
        public override string Message => message;
    }
}
