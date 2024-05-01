using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchScheduleException:Exception
    {
        private readonly string message;
        public NoSuchScheduleException()
        {
            message = "No Schedule found for given details";
        }

        public override string Message => message;
    }
}
