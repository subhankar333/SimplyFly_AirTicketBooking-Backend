using SimplyFly_Project.DTO;
using SimplyFly_Project.Exceptions;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Mappers;
using SimplyFly_Project.Models;
using SimplyFly_Project.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace SimplyFly_Project.Services
{
    public class UserService : IUserService
    {

        private readonly IRepository<string, User> _userRepository;
        private readonly IRepository<int, Admin> _adminRepository;
        private readonly IRepository<int, Customer> _customerRepository;
        private readonly IRepository<int, FlightOwner> _flightOwnerRepository;

        private readonly ILogger<UserService> _logger;
        private readonly ITokenService _tokenService;


        public UserService(IRepository<string, User> userRepository, IRepository<int, Admin> adminRepository, IRepository<int, Customer> customerRepository, IRepository<int, FlightOwner> flightOwnerRepository, ILogger<UserService> logger, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _customerRepository = customerRepository;
            _flightOwnerRepository = flightOwnerRepository;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<LoginUserDTO> Login(LoginUserDTO user)
        {
            var myUser = await _userRepository.GetAsync(user.Username);

            if(myUser  == null)
            {
                throw new InvalidUserException();
            }

            var userPassword = GetEncryptedPassword(user.Password, myUser.Key);
            var checkPasswordMatch = ComparePasswords(myUser.Password, userPassword);
            if(checkPasswordMatch)
            {
                user.Password = "";
                user.Role = myUser.Role;
                user.Token = await _tokenService.GenerateToken(user);
                return user;
            }

            throw new InvalidUserException();
        }


        public bool ComparePasswords(byte[] password, byte[] userPassword)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] != userPassword[i])
                    return false;
            }

            return true;
        }

        private byte[] GetEncryptedPassword(string password, byte[] key)
        {
            HMACSHA512 hmac = new HMACSHA512(key);
            var userPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return userPassword;
        }

        public async Task<LoginUserDTO> RegisterAdmin(RegisterAdninUserDTO user)
        {
            var users = await _userRepository.GetAsync();
            var checkUser = users.FirstOrDefault(u => u.Username == user.Username);

            if(checkUser == null)
            {
                User myUser = new RegisterToUser(user).GetUser();
                myUser = await _userRepository.Add(myUser);
                Admin admin = new RegisterToAdmin(user).GetAdmin();
                await _adminRepository.Add(admin);
                LoginUserDTO result = new LoginUserDTO
                {
                    Username = user.Username,
                    Role = user.Role,
                };

                return result;
            }

            throw new UserAlreadyExistsException();
        }

        public async Task<LoginUserDTO> RegisterCustomer(RegisterCustomerUserDTO user)
        {
            var users = await _userRepository.GetAsync();
            var checkUser = users?.FirstOrDefault(u => u.Username == user.Username);

            if(checkUser == null)
            {
                User myuser = new RegisterToUser(user).GetUser();
                myuser = await _userRepository.Add(myuser);
                Customer customer = new RegisterToCustomer(user).GetCustomer();
                customer = await _customerRepository.Add(customer);
                LoginUserDTO result = new LoginUserDTO
                {
                    Username = myuser.Username,
                    Role = myuser.Role,

                };
                return result;
            }

            throw new UserAlreadyExistsException();
        }

        public async Task<LoginUserDTO> RegisterFlightOwner(RegisterFlightOwnerUserDTO user)
        {
            var users = await _userRepository.GetAsync();
            var checkUser = users?.FirstOrDefault(u => u.Username == user.Username);

            if(checkUser == null)
            {
                User myuser = new RegisterToUser(user).GetUser();
                myuser = await _userRepository.Add(myuser);
                FlightOwner flightOwner = new RegisterToFlightOwner(user).GetFlightOwner();
                await _flightOwnerRepository.Add(flightOwner);
                LoginUserDTO result = new LoginUserDTO
                {
                    Username = myuser.Username,
                    Role = myuser.Role,

                };
                return result;
            }

            throw new UserAlreadyExistsException();
        }

        public async Task<LoginUserDTO> UpdateUserPassword(ForgotPasswordDTO userDTO)
        {
            User user = new RegisterToUser(userDTO).GetUser();
            var findUser = await _userRepository.GetAsync(userDTO.Username);

            if(findUser != null)
            {
                findUser.Password = user.Password;
                findUser.Key = user.Key;
                findUser = await _userRepository.Update(findUser);
                LoginUserDTO result = new LoginUserDTO
                {
                    Username = findUser.Username,
                    Role = findUser.Role,
                };

                return result;
            }

            throw new NoSuchUserException();  
        }
    }
}
