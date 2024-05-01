using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchCustomerException:Exception
    {
        private readonly string message;
        public NoSuchCustomerException()
        {
            message = "No Customer found for given details";
        }

        public override string Message => message;
    }
}
