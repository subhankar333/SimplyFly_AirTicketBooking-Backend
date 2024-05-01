using Microsoft.Extensions.Configuration;
using Moq;
using SimplyFly_Project.DTO;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyFly_Test
{
    internal class TokenServiceTest
    {
        [Test]
        public async Task GenerateToken_Test()
        {
            //arrange
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(e => e["SecretKey"]).Returns("This is a Dummy key created for authentication purpose");
            ITokenService tokenService = new TokenService((Microsoft.Extensions.Configuration.IConfiguration)mockConfiguration.Object);

            var user = new LoginUserDTO
            {
                Username = "Manohar",
                Role = "customer"
            };

            // Act
            var token = await tokenService.GenerateToken(user);

            // Assert
            Assert.IsNotNull(token);
        }
    }
}
