using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplyFly_Project.DTO;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.Diagnostics.CodeAnalysis;

namespace SimplyFly_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> RegisterCustomer(RegisterCustomerUserDTO register)
        {
            try
            {
                var result = await _userService.RegisterCustomer(register);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Route("RegisterFlightOwner")]
        [HttpPost]
        public async Task<LoginUserDTO> RegisterFlightOwner(RegisterFlightOwnerUserDTO register)
        {
            var result = await _userService.RegisterFlightOwner(register);
            return result;
        }

        [Route("RegisterAdmin")]
        [HttpPost]
        public async Task<LoginUserDTO> RegisterAdmin(RegisterAdninUserDTO register)
        {
            var result = await _userService.RegisterAdmin(register);
            return result;
        }


        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> Login(LoginUserDTO user)
        {
            try
            {
                var result = await _userService.Login(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Unauthorized("Invalid username or password");
            }
        }


        [Route("UpdatePassword")]
        [HttpPut]
        public async Task<ActionResult<LoginUserDTO>> UpdatePassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            try
            {
                var result = await _userService.UpdateUserPassword(forgotPasswordDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return Unauthorized("Invalid username");
            }
        }
    }
}
