using TechDictionaryApi.DTOs;

namespace TechDictionaryApi.Services
{
    public interface IUserService
    {
        Task<LoginResponseDTO> Login(LoginDTO loginDTO);
        Task<string> LogOut(string userName);
    }
}
