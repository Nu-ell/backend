using TechDictionaryApi.DTOs;

namespace TechDictionaryApi.Repositories
{
    public interface IExampleRepository
    {
        Task<int> CreateExample(CreateExampleDTO createExample);
        Task<int> UpdateExample(UpdateExampleDTO updateExample);
        Task<int> DeleteExample(DeleteExampleDTO deleteExample);
        Task<IEnumerable<ExampleDTO>> GetExamplesByWordId(long wordId);
        Task<ExampleDTO> GetExampleById(long exampleId);
    }
}
