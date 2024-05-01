using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoSuchAdminException:Exception
    {
        private readonly string message;
        public NoSuchAdminException()
        {
            message = "No Admin found for the given details";
        }

        public override string Message => message;
    }
}
