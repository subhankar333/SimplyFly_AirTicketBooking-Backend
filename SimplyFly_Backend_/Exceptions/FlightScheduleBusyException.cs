using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class FlightScheduleBusyException:Exception
    {
        private readonly string _message;
        public FlightScheduleBusyException()
        {
            _message = "Flight is busy for this schedule.";
        }
        public override string Message => _message;
    }
}
