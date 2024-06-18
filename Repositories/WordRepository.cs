using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TechDictionaryApi.DTOs;

namespace TechDictionaryApi.Repositories
{
    public class WordRepository : IWordRepository
    {

        private readonly string _connectionString;

        public WordRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateWord(CreateWordDTO createWord)
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "INSERT INTO Words(Word, Class, Defination, Pronounciation, History, StatusId, CreatedDate, CreatedBy) " +
                        "VALUES(@Word, @Class, @Defination, @Pronounciation, @History, @StatusId, @CreatedDate, @CreatedBy)";
                    var param = new DynamicParameters();
                    param.Add("@Word", createWord.Word);
                    param.Add("@Class", createWord.Class);
                    param.Add("@Defination", createWord.Defination);
                    param.Add("@Pronounciation", createWord.Pronounciation);
                    param.Add("@History", createWord.History);
                    param.Add("@StatusId", createWord.StatusId);
                    param.Add("@CreatedDate", DateTime.Now);
                    param.Add("@CreatedBy", createWord.CreatedBy);

                    int resp = await dapper.ExecuteAsync(sqlQuery, param: param, commandType: CommandType.Text);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<int> UpdateWord(UpdateWordDTO updateWord)
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "UPDATE Words SET Word = @Word, Class = @Class, Defination = @Defination, Pronounciation = @Pronounciation, " +
                                      "History = @History, StatusId = @StatusId, IsUpdated = 1, UpdatedDate = @UpdatedDate, UpdatedBy = @UpdatedBy " +
                                      "WHERE WordId = @WordId AND IsDeleted = 0";
                    var param = new DynamicParameters();
                    param.Add("@WordId", updateWord.WordId);
                    param.Add("@Word", updateWord.Word);
                    param.Add("@Class", updateWord.Class);
                    param.Add("@Defination", updateWord.Defination);
                    param.Add("@Pronounciation", updateWord.Pronounciation);
                    param.Add("@History", updateWord.History);
                    param.Add("@StatusId", updateWord.StatusId);
                    param.Add("@UpdatedDate", DateTime.Now);
                    param.Add("@UpdatedBy", updateWord.UpdatedBy);

                    int resp = await dapper.ExecuteAsync(sqlQuery, param: param, commandType: CommandType.Text);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteWord(DeleteWordDTO deleteWord)
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "UPDATE Words SET IsDeleted = 1, DeletedDate = @DeletedDate, DeletedBy = @DeletedBy WHERE WordId = @WordId";
                    var param = new DynamicParameters();
                    param.Add("@WordId", deleteWord.WordId);
                    param.Add("@DeletedDate", DateTime.Now);
                    param.Add("@DeletedBy", deleteWord.DeletedBy);

                    int resp = await dapper.ExecuteAsync(sqlQuery, param: param, commandType: CommandType.Text);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }


        public async Task<WordDTO> GetWordById(long wordId)
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "SELECT * FROM Words WHERE WordId = @WordId AND IsDeleted = 0";

                    var param = new DynamicParameters();
                    param.Add("@WordId", wordId);

                    var resp = await dapper.QueryFirstOrDefaultAsync<WordDTO>(sqlQuery, param);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        //this method will fetch only published words with statusId 2. This is because GetWordByWord is the SearchForWord api in the controller and we can only search for published words
        public async Task<WordDTO> GetWordByWord(string word)
        {
            try
            {
           
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    
                    string sqlQuery = "SELECT * FROM Words WHERE Word = @Word AND IsDeleted = 0 AND StatusId = 2";

                    var param = new DynamicParameters();
                    param.Add("@Word", word);

                    var resp = await dapper.QueryFirstOrDefaultAsync<WordDTO>(sqlQuery, param);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<WordDTO>> GetAllWords()
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    //Reminder: this commented same result as the current query being used.

                    //string sqlQuery = "SELECT a.WordId, a.Word, a.Class, a.Defination, a.Pronounciation, a.History, " +
                    //    "a.StatusId, b.StatusName, a.CreatedDate, a.CreatedBy, a.IsUpdated, a.UpdatedDate, a.UpdatedBy, " +
                    //    "a.IsDeleted, a.DeletedDate, a.DeletedBy FROM [dbo].[Words] a " +
                    //    "INNER JOIN [dbo].[Status] b ON b.StatusId = a.StatusId Where a.[IsDeleted] = 0";

                    string sqlQuery = "SELECT a.*, b.StatusName FROM [dbo].[Words] a INNER JOIN [dbo].[Status] b ON b.StatusId = a.StatusId Where a.[IsDeleted] = 0";

                    var words = await dapper.QueryAsync<WordDTO>(sqlQuery);
                    return words;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<DashboardResponseDTO> GetAdminDashboard()
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    DashboardResponseDTO dashboardData = new DashboardResponseDTO();

                    string publishedWordsQuery = "SELECT COUNT(*) FROM Words WHERE StatusId = 1 AND [IsDeleted] = 0"; //StatusId = 1 for active words
                    string pendingWordsQuery = "SELECT COUNT(*) FROM Words WHERE StatusId = 2 AND [IsDeleted] = 0"; //StatusId = 2 for pending words

                    string dailySearchesQuery = @"SELECT COUNT(*) FROM WordSearches WHERE SearchDate >= CAST(GETDATE() AS DATE)"; //Query for daily searches
                                                                                                                                  
                    string mostlySearchedWordsQuery = @"SELECT TOP 3 b.Word FROM WordSearches a JOIN Words b ON a.WordId = b.WordId
                                                        GROUP BY b.Word ORDER BY COUNT(a.SearchId) DESC"; //Query for top 3 mostly searched words

                    
                    string activeUserRequestsQuery = @"SELECT COUNT(*) FROM UserRequest WHERE UserRequestStatusId = 1 AND [IsDeleted] = 0"; //Query for active user requests //1 is for Open/PendingRequest

                    dashboardData.PublishedWords = await dapper.QueryFirstOrDefaultAsync<int>(publishedWordsQuery);
                    dashboardData.PendingWords = await dapper.QueryFirstOrDefaultAsync<int>(pendingWordsQuery);
                    dashboardData.DailySearches = await dapper.QueryFirstOrDefaultAsync<int>(dailySearchesQuery);
                    dashboardData.MostlySearchedWords = (await dapper.QueryAsync<string>(mostlySearchedWordsQuery)).ToList();
                    dashboardData.ActiveUserRequests = await dapper.QueryFirstOrDefaultAsync<int>(activeUserRequestsQuery);

                    return dashboardData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<int> InsertWordSearch(long wordId)
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = @"
                INSERT INTO [dbo].[WordSearches] (WordId, SearchDate)
                VALUES (@WordId, @SearchDate)";

                    var param = new DynamicParameters();
                    param.Add("@WordId", wordId);
                    param.Add("@SearchDate", DateTime.Now);

                    int resp = await dapper.ExecuteAsync(sqlQuery, param);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<WordDTO> GetWordByWord2(string word)
        {
            try
            {

                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {

                    string sqlQuery = "SELECT * FROM Words WHERE Word = @Word AND IsDeleted = 0";

                    var param = new DynamicParameters();
                    param.Add("@Word", word);

                    var resp = await dapper.QueryFirstOrDefaultAsync<WordDTO>(sqlQuery, param);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

    }
}
