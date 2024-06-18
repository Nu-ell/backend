using TechDictionaryApi.DTOs;
using TechDictionaryApi.Repositories;

namespace TechDictionaryApi.Services
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _wordRepository;
        private readonly IExampleRepository _exampleRepository;
        public WordService(IWordRepository wordRepository, IExampleRepository exampleRepository)
        {
            _wordRepository = wordRepository;
            _exampleRepository = exampleRepository;
        }

        public async Task<string> CreateWordAndExamples(CreateWordAndExamplesDTO request, string createdBy)
        {
            //var response = new ApiResponses();
            string response = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(request.Word) || string.IsNullOrEmpty(request.Class) ||
                string.IsNullOrEmpty(request.Defination) || string.IsNullOrEmpty(createdBy))
                {
                    response = "Ensure all fields are inputted correctly";
                    return response;
                }
                if (request.StatusId <= 0 || request.StatusId > 2)
                {
                    response = "Invalid status was supplied";
                    return response;
                }

                if (request.Examples.Count <= 0)
                {
                    response = "Word Examples are required";
                    return response;
                }

                //if (string.IsNullOrEmpty(request.Word))
                //{
                //    response = "Word is required";
                //}

                //if (string.IsNullOrEmpty(request.Class))
                //{
                //    response = "Class is required";
                //}

                //if (string.IsNullOrEmpty(request.Defination))
                //{
                //    response = "Defination is required";
                //}

                //if (string.IsNullOrEmpty(request.CreatedBy))
                //{
                //    response = "CreatedBy is required";
                //}

                var wordExists = await _wordRepository.GetWordByWord2(request.Word);
                if (wordExists != null)
                {
                    response = "Word already exists in the system.";
                    return response;
                }

                var createWordDTOrequest = new CreateWordDTO
                {
                    Word = request.Word,
                    Class = request.Class,
                    Defination = request.Defination,
                    Pronounciation = request.Pronounciation,
                    History = request.History,
                    StatusId = request.StatusId,
                    CreatedBy = createdBy
                };

                int createWordResp = await _wordRepository.CreateWord(createWordDTOrequest);
                if (createWordResp > 0)
                {
                    //if createWordResp is greater than zero, it means its a success and it has saved
                    //Now we can then find the word and use its wordId to insert and create its examples
                    var fetchNewlyCreatedWord = await _wordRepository.GetWordByWord2(createWordDTOrequest.Word);
                    if (fetchNewlyCreatedWord == null)
                    {
                        response = "Error Occured: newly created Word could not be retrieved.";
                        return response;
                    }

                    //Insert the word examples here
                    foreach (var example in request.Examples)
                    {
                        var createExampleDTOrequest = new CreateExampleDTO
                        {
                            WordExample = example.WordExample,
                            WordId = fetchNewlyCreatedWord.WordId,
                            CreatedBy = createdBy
                        };

                        int createWordExampleResp = await _exampleRepository.CreateExample(createExampleDTOrequest);
                        if (createWordExampleResp <= 0)
                        {
                            response = $"An Error Occured while creating this word example:{example.WordExample}.";
                            return response;
                        }
                    }

                    response = "Word and its Example(s) created successfully";
                    return response;

                }
                else
                {
                    response = "An error occured while creating word. Kindly contact admin.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response = $"Exception Occured: {ex.Message}";
                return response;
            }
        }

        public async Task<string> UpdateWordAndExamples(UpdateWordAndExamplesDTO request, string updatedBy)
        {
            //var response = new ApiResponses();
            string response = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(request.Word) || string.IsNullOrEmpty(request.Class) ||
                string.IsNullOrEmpty(request.Defination) || string.IsNullOrEmpty(updatedBy))
                {
                    response = "Ensure all fields are inputted correctly";
                    return response;
                }
                if (request.StatusId <= 0 || request.StatusId > 2)
                {
                    response = "Invalid status was supplied";
                    return response;
                }

                if (request.Examples.Count <= 0)
                {
                    response = "Word Examples are required";
                    return response;
                }

                var wordExists = await _wordRepository.GetWordByWord(request.Word);
                if (wordExists == null)
                {
                    response = "Word to be updated is not found. Kindly confirm if this word has been created in the system";
                    return response;
                }

                foreach (var exampleReq in request.Examples)
                {
                    var exampleExists = await _exampleRepository.GetExampleById(exampleReq.ExampleId);
                    if (exampleExists == null)
                    {
                        response = $"Example ({exampleReq.WordExample}) to be updated is not found. Kindly confirm if this example has been created in the system";
                        return response;
                    }
                }

                var UpdateWordDTOrequest = new UpdateWordDTO
                {
                    WordId = request.WordId,
                    Word = request.Word,
                    Class = request.Class,
                    Defination = request.Defination,
                    Pronounciation = request.Pronounciation,
                    History = request.History,
                    StatusId = request.StatusId,
                    UpdatedBy = updatedBy
                };

                int updateWordResp = await _wordRepository.UpdateWord(UpdateWordDTOrequest);
                if (updateWordResp > 0)
                {
                    //if UpdateWordResp is greater than zero, it means its a success and it has saved
                    //Now we can then use each respective exampleIds to update its examples

                    //Update the word examples here
                    foreach (var example in request.Examples)
                    {

                        var UpdateExampleDTOrequest = new UpdateExampleDTO
                        {
                            ExampleId = example.ExampleId,
                            WordExample = example.WordExample,
                            UpdatedBy = updatedBy
                        };

                        int updateWordExampleResp = await _exampleRepository.UpdateExample(UpdateExampleDTOrequest);
                        if (updateWordExampleResp <= 0)
                        {
                            response = $"An Error Occured while updating this word example:{example.WordExample}.";
                            return response;
                        }
                    }

                    response = "Word and its Example(s) updated successfully";
                    return response;

                }
                else
                {
                    response = "An error occured while updating word. Kindly contact admin.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response = $"Exception Occured: {ex.Message}";
                return response;
            }
        }

        public async Task<string> DeleteWordAndExamples(DeleteWordAndExamplesDTO request, string deletedBy)
        {
            //var response = new ApiResponses();
            string response = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(deletedBy))
                {
                    response = "DeletedBy is required";
                    return response;
                }

                var wordExists = await _wordRepository.GetWordById(request.WordId);
                if (wordExists == null)
                {
                    response = "Word to be Deleted is not found. Kindly confirm if this word has been created in the system";
                    return response;
                }

                var findWordExamples = await _exampleRepository.GetExamplesByWordId(request.WordId);
                if (findWordExamples.Any())
                {
                    //Delete the word examples here
                    foreach (var example in findWordExamples)
                    {

                        var DeleteExampleDTOrequest = new DeleteExampleDTO
                        {
                            ExampleId = example.ExampleId,
                            DeletedBy = deletedBy
                        };

                        int DeleteWordExampleResp = await _exampleRepository.DeleteExample(DeleteExampleDTOrequest);
                        if (DeleteWordExampleResp <= 0)
                        {
                            response = $"An Error Occured while deleting this word example:{example.WordExample}.";
                            return response;
                        }
                    }

                }

                var DeleteWordDTOrequest = new DeleteWordDTO
                {
                    WordId = request.WordId,
                    DeletedBy = deletedBy
                };

                int DeleteWordResp = await _wordRepository.DeleteWord(DeleteWordDTOrequest);
                if (DeleteWordResp > 0)
                {
                    response = "Word and its Examole(s) deleted successfully";
                    return response;
                }
                else
                {
                    response = "An error occured while deleting word. Kindly contact admin.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response = $"Exception Occured: {ex.Message}";
                return response;
            }
        }

        public async Task<List<WordAndExamplesDTO>> GetAllWordsAndExamples()
        {
            //var response = new ApiResponses();
            var response = new List<WordAndExamplesDTO>();
            var examplesResp = new List<ExampleServiceDTO>();
            try
            {
                var getWordResp = await _wordRepository.GetAllWords();
                if (getWordResp.Any())
                {
                    foreach (var item in getWordResp)
                    {
                        var wordExamples = await _exampleRepository.GetExamplesByWordId(item.WordId);
                        foreach (var example in wordExamples)
                        {
                            var exampleItem = new ExampleServiceDTO
                            {
                                ExampleId = example.ExampleId,
                                WordExample = example.WordExample,
                                WordId = example.WordId
                            };
                            examplesResp.Add(exampleItem);
                        }


                        var wordWithExamples = new WordAndExamplesDTO
                        {
                            WordId = item.WordId,
                            Word = item.Word,
                            Class = item.Class,
                            Defination = item.Defination,
                            Pronounciation = item.Pronounciation,
                            History = item.History,
                            StatusId = item.StatusId,
                            StatusName = item.StatusName,
                            CreatedDate = item.CreatedDate,
                            CreatedBy = item.CreatedBy,
                            IsUpdated = item.IsUpdated,
                            UpdatedDate = item.UpdatedDate,
                            UpdatedBy = item.UpdatedBy,
                            IsDeleted = item.IsDeleted,
                            DeletedDate = item.DeletedDate,
                            DeletedBy = item.DeletedBy,
                            Examples = examplesResp
                        };

                        response.Add(wordWithExamples);
                        examplesResp = new List<ExampleServiceDTO>(); //these clears examples gathered to keep the list fresh for next loop
                    }

                    return response;
                }
                else
                {
                    //response = "No words and examples found.";
                    return null;
                }
            }
            catch (Exception ex)
            {
                var error = $"Exception Occured: {ex.Message}";
                return null;
            }
        }

        //GetWordByWord is the SearchForWord api in the controller, we will be inserting any successful search into the WordSerches table in this method
        public async Task<WordAndExamplesDTO> GetWordByWord(string word)
        {
            //var response = new ApiResponses();
            var response = new WordAndExamplesDTO();
            var examplesResp = new List<ExampleServiceDTO>();
            try
            {
                if (string.IsNullOrEmpty(word)) { return null; };

                var wordResp = await _wordRepository.GetWordByWord(word);
                if (wordResp != null)
                {
                    var wordExamples = await _exampleRepository.GetExamplesByWordId(wordResp.WordId);

                    foreach (var example in wordExamples)
                    {
                        var exampleItem = new ExampleServiceDTO
                        {
                            ExampleId = example.ExampleId,
                            WordExample = example.WordExample,
                            WordId = example.WordId
                        };
                        examplesResp.Add(exampleItem);
                    }

                    var wordWithExample = new WordAndExamplesDTO
                    {
                        WordId = wordResp.WordId,
                        Word = wordResp.Word,
                        Class = wordResp.Class,
                        Defination = wordResp.Defination,
                        Pronounciation = wordResp.Pronounciation,
                        History = wordResp.History,
                        StatusId = wordResp.StatusId,
                        StatusName = wordResp.StatusName,
                        CreatedDate = wordResp.CreatedDate,
                        CreatedBy = wordResp.CreatedBy,
                        IsUpdated = wordResp.IsUpdated,
                        UpdatedDate = wordResp.UpdatedDate,
                        UpdatedBy = wordResp.UpdatedBy,
                        IsDeleted = wordResp.IsDeleted,
                        DeletedDate = wordResp.DeletedDate,
                        DeletedBy = wordResp.DeletedBy,
                        Examples = examplesResp
                    };

                    response = wordWithExample;

                    //insert successful serch in WordSerches table before returning response
                    var insertWordSerchResp = await _wordRepository.InsertWordSearch(response.WordId);
                    if (insertWordSerchResp > 0)
                    {
                        return response;
                    }
                    else
                    {
                        //response = "No words and examples found.";
                        return null;
                    }
                }
                else
                {
                    //response = "No words and examples found.";
                    return null;
                }
            }
            catch (Exception ex)
            {
                var error = $"Exception Occured: {ex.Message}";
                return null;
            }
        }

        public async Task<DashboardResponseDTO> GetAdminDashboard()
        {
            try
            {
                var dashboardDetails = await _wordRepository.GetAdminDashboard();
                if (dashboardDetails != null)
                {
                    return dashboardDetails;
                }
                else
                {
                    //response = "No Dashboard Details found.";
                    return null;
                }
            }
            catch (Exception ex)
            {
                var error = $"Exception Occured: {ex.Message}";
                return null;
            }
        }
    }
}
