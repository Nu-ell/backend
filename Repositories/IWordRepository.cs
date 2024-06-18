using TechDictionaryApi.DTOs;

namespace TechDictionaryApi.Repositories
{
    public interface IWordRepository
    {
        Task<int> CreateWord(CreateWordDTO createWord);
        Task<int> UpdateWord(UpdateWordDTO updateWord);
        Task<int> DeleteWord(DeleteWordDTO deleteWord);
        Task<WordDTO> GetWordById(long wordId);
        Task<WordDTO> GetWordByWord(string word);
        Task<IEnumerable<WordDTO>> GetAllWords();
        Task<DashboardResponseDTO> GetAdminDashboard();
        Task<int> InsertWordSearch(long wordId);
        Task<WordDTO> GetWordByWord2(string word);
    }
}
