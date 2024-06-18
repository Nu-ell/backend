using TechDictionaryApi.DTOs;

namespace TechDictionaryApi.Repositories
{
    public interface IUserRepository
    {
        Task<LoginResponseDTO> Login(LoginDTO loginDTO);
        Task<LoginResponseDTO> GetUserByUserName(string userName);
        Task<int> UpdateLogOut(string userName);
    }
}
