using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.DTO
{
    [ExcludeFromCodeCoverage]
    public class ForgotPasswordDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
