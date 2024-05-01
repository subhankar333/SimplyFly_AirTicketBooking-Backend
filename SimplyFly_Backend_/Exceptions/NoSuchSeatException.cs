using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchSeatException:Exception
    {
        private readonly string message;
        public NoSuchSeatException()
        {
            message = "No seat found for given details";
        }

        public override string Message => message;
    }
}
