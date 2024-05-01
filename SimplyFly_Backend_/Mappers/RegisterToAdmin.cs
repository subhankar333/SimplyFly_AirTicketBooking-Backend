using SimplyFly_Project.DTO;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Mappers
{
    [ExcludeFromCodeCoverage]
    public class RegisterToAdmin
    {
        Admin admin;
        public RegisterToAdmin(RegisterAdninUserDTO register)
        {
            admin = new Admin();
            admin.Name = register.Name;
            admin.Email = register.Email;
            admin.Position = register.Position;
            admin.ContactNumber = register.ContactNumber;
            admin.Address = register.Address;
            admin.Username = register.Username;
        }

        public Admin GetAdmin()
        {
            return admin;
        }
    }
}
