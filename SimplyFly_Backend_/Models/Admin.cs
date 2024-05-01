using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Models
{
    [ExcludeFromCodeCoverage]
    public class Admin:IEquatable<Admin>
    {
        [Key]
        public int AdminId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string Username { get; set; }
        [ForeignKey("Username")]
        public User User { get; set; }

        public Admin()
        {
            AdminId = 0;
        }

        public Admin(int adminId, string name, string email, string position, string? contactNumber, string? address, string password)
        {
            AdminId = adminId;
            Name = name;
            Email = email;
            Position = position;
            ContactNumber = contactNumber;
            Address = address;

        }
        public Admin(string name, string email, string position, string? contactNumber, string? address, string password)
        {
            Name = name;
            Email = email;
            Position = position;
            ContactNumber = contactNumber;
            Address = address;

        }

        public bool Equals(Admin? other)
        {
            var Admin = other ?? new Admin();
            return this.AdminId.Equals(Admin.AdminId) && this.Email.Equals(Admin.Email);
        }
    }
}
