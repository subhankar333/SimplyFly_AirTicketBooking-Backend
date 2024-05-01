using SimplyFly_Project.DTO;

namespace SimplyFly_Project.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(LoginUserDTO userDTO);
    }
}
