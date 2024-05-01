using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchPaymentException:Exception
    {
        private readonly string message;
        public NoSuchPaymentException()
        {
            message = "No Payment found for given details";
        }

        public override string Message => message;
    }
}
