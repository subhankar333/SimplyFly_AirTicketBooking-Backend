using SimplyFly_Project.DTO;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Interfaces
{
    public interface IAdminService
    {
        public Task<Admin> GetAdminByUsername(string username);
        public Task<Admin> UpdateAdmin(UpdateAdminDTO admin);
        public Task<User> DeleteUser(string username);
    }
}
