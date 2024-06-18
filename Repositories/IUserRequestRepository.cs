using TechDictionaryApi.DTOs;

namespace TechDictionaryApi.Repositories
{
    public interface IUserRequestRepository
    {
        Task<int> RequestChangeToWordorRequestNewWord(UserRequestDTO request);
        Task<int> ResolveRequest(long userRequestId, string resolvedBy);
        Task<IEnumerable<UserRequestListDTO>> GetAllUserRequests();
        Task<GeneralUserDashboardResponseDTO> GetGeneralUserDashboard();
    }
}
