using TechDictionaryApi.DTOs;

namespace TechDictionaryApi.Services
{
    public interface IWordService
    {
        Task<string> CreateWordAndExamples(CreateWordAndExamplesDTO request, string createdBy);
        Task<string> UpdateWordAndExamples(UpdateWordAndExamplesDTO request, string updatedBy);
        Task<string> DeleteWordAndExamples(DeleteWordAndExamplesDTO request, string deletedBy);
        Task<List<WordAndExamplesDTO>> GetAllWordsAndExamples();
        Task<WordAndExamplesDTO> GetWordByWord(string word);
        Task<DashboardResponseDTO> GetAdminDashboard();
    }
}
