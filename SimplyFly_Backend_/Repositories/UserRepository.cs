using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimplyFly_Project.Context;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserRepository : IRepository<string, User>
    {

        private readonly SimplyFlyDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(SimplyFlyDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> Add(User item)
        {
            _context.Users.Add(item);
            _context.SaveChanges();
            _logger.LogInformation($"User added with username {item.Username}");
            return item;
        }

        public async Task<User> Delete(string userName)
        {
            var user = await GetAsync(userName);
            if(user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                _logger.LogInformation($"User deleted with username {userName}");
                return user;
            }

            throw new NoSuchUserException();
        }

        public async Task<User> GetAsync(string userName)
        {
            var users = _context.Users.ToList();
            var user = users.FirstOrDefault(u => u.Username == userName);
            if(user != null)
            {
                return user;
            }
            throw new NoSuchUserException();
        }

        public async Task<List<User>> GetAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> Update(User item)
        {
            var user = await GetAsync(item.Username);
            if (user != null)
            {
                _context.Entry<User>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"User updated with username {item.Username}");
                return item;
            }
            throw new NoSuchUserException();
        }
    }
}
