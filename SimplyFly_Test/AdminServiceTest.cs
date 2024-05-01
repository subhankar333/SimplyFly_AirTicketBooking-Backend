using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SimplyFly_Project.Context;
using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using SimplyFly_Project.Repositories;
using SimplyFly_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyFly_Test
{
    internal class AdminServiceTest
    {
        private SimplyFlyDbContext context;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SimplyFlyDbContext>().UseInMemoryDatabase("dummyDatabase").Options;
            context = new SimplyFlyDbContext(options);
        }

        [Test]
        public async Task DeleteUserTest()
        {
            var mockAdminRepositoryLogeer = new Mock<ILogger<AdminRepository>>();
            var mockUserRepositoryLogeer = new Mock<ILogger<UserRepository>>();
            var mockAdminServiceLogger = new Mock<ILogger<AdminService>>();

            IRepository<int,Admin> adminRepository = new AdminRepository(context, mockAdminRepositoryLogeer.Object);
            IRepository<string,User> userRepository = new UserRepository(context, mockUserRepositoryLogeer.Object);

            IAdminService adminService = new AdminService(adminRepository, mockAdminServiceLogger.Object, userRepository);

            User user = new User
            {
                Username = "testuser",
                Password = new byte[] { 0x01, 0x02, 0x03, 0x04 },
                Role = "admin",
                Key = new byte[] { 0xAA, 0xBB, 0xCC, 0xDD }
            };

            await userRepository.Add(user);

            // Act
            var deletedUser = await adminService.DeleteUser("testuser");

            // Assert
            Assert.That(deletedUser, Is.EqualTo(user));
        }


        [Test]
        public void DeleteUserNoSuchUserExceptionTest()
        {
            var mockAdminRepositoryLogeer = new Mock<ILogger<AdminRepository>>();
            var mockUserRepositoryLogeer = new Mock<ILogger<UserRepository>>();
            var mockAdminServiceLogger = new Mock<ILogger<AdminService>>();

            IRepository<int, Admin> adminRepository = new AdminRepository(context, mockAdminRepositoryLogeer.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogeer.Object);

            IAdminService adminService = new AdminService(adminRepository, mockAdminServiceLogger.Object, userRepository);

            Assert.ThrowsAsync<NoSuchUserException>(async () => await adminService.DeleteUser("Nayan"));
        }


        [Test]
        public async Task GetAdminByUsernameTest()
        {
            var mockAdminRepositoryLogeer = new Mock<ILogger<AdminRepository>>();
            var mockUserRepositoryLogeer = new Mock<ILogger<UserRepository>>();
            var mockAdminServiceLogger = new Mock<ILogger<AdminService>>();

            IRepository<int, Admin> adminRepository = new AdminRepository(context, mockAdminRepositoryLogeer.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogeer.Object);

            IAdminService adminService = new AdminService(adminRepository, mockAdminServiceLogger.Object, userRepository);

            Admin admin = new Admin
            {
                Username = "praveen_94",
                Name = "Praveen Kumar",
                Email = "pk@gmail.com"
            };

            await adminRepository.Add(admin);

            var retrievedAdmin = await adminService.GetAdminByUsername("praveen_94");
            Assert.That(retrievedAdmin,Is.EqualTo(admin));
        }


        [Test]
        public void GetAdminByUsernameNoSuchAdminExceptionTest()
        {
            var mockAdminRepositoryLogeer = new Mock<ILogger<AdminRepository>>();
            var mockUserRepositoryLogeer = new Mock<ILogger<UserRepository>>();
            var mockAdminServiceLogger = new Mock<ILogger<AdminService>>();

            IRepository<int, Admin> adminRepository = new AdminRepository(context, mockAdminRepositoryLogeer.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogeer.Object);

            IAdminService adminService = new AdminService(adminRepository, mockAdminServiceLogger.Object, userRepository);

            Assert.ThrowsAsync<NoSuchAdminException>(async () => await adminService.GetAdminByUsername("Kagiso"));
        }

        [Test]
        public async Task UpdateAdminTest()
        {
            var mockAdminRepositoryLogeer = new Mock<ILogger<AdminRepository>>();
            var mockUserRepositoryLogeer = new Mock<ILogger<UserRepository>>();
            var mockAdminServiceLogger = new Mock<ILogger<AdminService>>();

            IRepository<int, Admin> adminRepository = new AdminRepository(context, mockAdminRepositoryLogeer.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogeer.Object);

            IAdminService adminService = new AdminService(adminRepository, mockAdminServiceLogger.Object, userRepository);

            Admin admin = new Admin
            {
                Username = "testadmin",
                Name = "Test Admin",
                Email = "testadmin@example.com"
            };
            await adminRepository.Add(admin);

            UpdateAdminDTO updatedAdmin = new UpdateAdminDTO
            {
                AdminId = admin.AdminId,
                Name = "Kumar Sanga",
                Email = "ksanga@gmail.com",
                Address = "Mumbai",
                ContactNumber = "9843987639",
                Position = "Manager"
            };

            var result = await adminService.UpdateAdmin(updatedAdmin);
            Assert.That(result.Name,Is.EqualTo(updatedAdmin.Name));

        }


        [Test]
        public void UpdateAdminNoSuchAdminExceptionTest()
        {
            
            var mockAdminRepositoryLogger = new Mock<ILogger<AdminRepository>>();
            var mockUserRepositoryLogger = new Mock<ILogger<UserRepository>>();
            var mockAdminServiceLogger = new Mock<ILogger<AdminService>>();

            IRepository<int, Admin> adminRepository = new AdminRepository(context, mockAdminRepositoryLogger.Object);
            IRepository<string, User> userRepository = new UserRepository(context, mockUserRepositoryLogger.Object);

            IAdminService adminService = new AdminService(adminRepository, mockAdminServiceLogger.Object, userRepository);

            UpdateAdminDTO updatedAdmin = new UpdateAdminDTO
            {
                AdminId = 6969,
                Name = "Kane william",
                Email = "kane@gmail.com",
                Address = "Chennai",
                ContactNumber = "9876543219",
                Position = "Manager"
            };

            
            Assert.ThrowsAsync<NoSuchAdminException>(async () => await adminService.UpdateAdmin(updatedAdmin));
        }
    }
}
