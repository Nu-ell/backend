using TechDictionaryApi.DTOs;
using TechDictionaryApi.Repositories;

namespace TechDictionaryApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponseDTO> Login(LoginDTO loginDTO)
        {
            try
            {
                if (String.IsNullOrEmpty(loginDTO.UserName) || String.IsNullOrEmpty(loginDTO.Password))
                {
                    throw new UnauthorizedAccessException("Invalid credentials");
                }
                var resp = await _userRepository.Login(loginDTO);
                return resp;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> LogOut(string userName)
        {
            try
            {
                if (String.IsNullOrEmpty(userName))
                {
                    return "Username is required!";
                }

                var validUser = await _userRepository.GetUserByUserName(userName);
                if (validUser == null)
                {
                    return "Invalid User";
                }

                int logoutResp = await _userRepository.UpdateLogOut(userName);
                if (logoutResp > 0)
                {
                    return "User logged out successfully";
                }
                else
                {
                    return "An Error occured while logging user out. Kindly contact admin";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
