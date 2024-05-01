using Microsoft.EntityFrameworkCore;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class AdminRepository : IRepository<int, Admin>
    {

        private readonly SimplyFlyDbContext _context;
        ILogger<AdminRepository> _logger;

        public AdminRepository(SimplyFlyDbContext context,ILogger<AdminRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Admin> Add(Admin item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation($"Admin added with id-{item.AdminId}");
            return item;
        }

        public Task<Admin> Delete(int adminId)
        {
            var admin = GetAsync(adminId);
            if(admin != null)
            {
                _context.Remove(admin);
                _context.SaveChanges();
                _logger.LogInformation($"Admin added with id-{adminId}");
                return admin;
            }

            throw new NoSuchAdminException();
        }

        public async Task<Admin>  GetAsync(int adminId)
        {
            var admins = await GetAsync();
            var admin = admins.FirstOrDefault(item => item.AdminId == adminId);

            if(admin != null)
            {
                return admin;
            }

            throw new NoSuchAdminException();
        }

        public async Task<List<Admin>> GetAsync()
        {
            var admins = _context.Admins.ToList();
            return admins;
        }

        public async Task<Admin>  Update(Admin item)
        {
            var admin = await GetAsync(item.AdminId);
            if(admin != null)
            {
                _context.Entry<Admin>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Admin updated with id-{item.AdminId}");
                return admin;
            }

            throw new NoSuchAdminException();
        }
    }
}
