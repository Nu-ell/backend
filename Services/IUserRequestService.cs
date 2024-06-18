using TechDictionaryApi.DTOs;

namespace TechDictionaryApi.Services
{
    public interface IUserRequestService
    {
        Task<string> RequestChangeToWordorRequestNewWord(UserRequestDTO request);
        Task<string> ResolveRequest(long userRequestId, string resolvedBy);
        Task<IEnumerable<UserRequestListDTO>> GetAllUserRequests();
        Task<GeneralUserDashboardResponseDTO> GetGeneralUserDashboard();
    }
}
