using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;

namespace SimplyFly_Project.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<int,Admin> _adminRepository;
        private readonly IRepository<string, User> _userRepository;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IRepository<int, Admin> adminrepository, ILogger<AdminService> logger)
        {
            _adminRepository = adminrepository;
            _logger = logger;
        }
        public AdminService(IRepository<int, Admin> adminrepository, ILogger<AdminService> logger, IRepository<string, User> userRepository)
        {
            _adminRepository = adminrepository;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<User> DeleteUser(string userName)
        {
            var users = await _userRepository.GetAsync();
            var user = users.FirstOrDefault(u => u.Username == userName);   
            if (user != null)
            {
                user = await _userRepository.Delete(userName);
                return user;
            }

            throw new NoSuchUserException();
        }

        public async Task<Admin> GetAdminByUsername(string userName)
        {
            var admins = await _adminRepository.GetAsync();
            var admin = admins.FirstOrDefault(a => a.Username == userName);

            if(admin != null)
            {
                return admin;
            }

            throw new NoSuchAdminException();
        }

        public async Task<Admin> UpdateAdmin(UpdateAdminDTO admin)
        {
            var _admin = await _adminRepository.GetAsync(admin.AdminId);
            if (_admin != null)
            {
                _admin.Name = admin.Name;
                _admin.Email = admin.Email;
                _admin.Address = admin.Address;
                _admin.ContactNumber = admin.ContactNumber;
                _admin.Position = admin.Position;
                _admin = await _adminRepository.Update(_admin);
                return _admin;
            }
            throw new NoSuchAdminException();
        }
    }
}
