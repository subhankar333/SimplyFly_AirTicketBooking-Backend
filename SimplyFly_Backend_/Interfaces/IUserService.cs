using SimplyFly_Project.DTO;

namespace SimplyFly_Project.Interfaces
{
    public interface IUserService
    {
        public Task<LoginUserDTO> Login(LoginUserDTO user);
        public Task<LoginUserDTO> RegisterAdmin(RegisterAdninUserDTO user);
        public Task<LoginUserDTO> RegisterCustomer(RegisterCustomerUserDTO user);
        public Task<LoginUserDTO> RegisterFlightOwner(RegisterFlightOwnerUserDTO user);
        public Task<LoginUserDTO> UpdateUserPassword(ForgotPasswordDTO userDTO);
    }
}
