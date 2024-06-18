using Azure.Core;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System;
using TechDictionaryApi.DTOs;

namespace TechDictionaryApi.Repositories
{
    public class UserRequestRepository : IUserRequestRepository
    {
        private readonly string _connectionString;

        public UserRequestRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> RequestChangeToWordorRequestNewWord(UserRequestDTO request)
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = @"INSERT INTO [dbo].[UserRequest] 
                                ([Word], [RequestDefinitionOrDescription], [UserRequestStatusId], [UserRequestTypeId], [CreatedBy]) 
                                VALUES 
                                (@Word, @RequestDefinitionOrDescription, @UserRequestStatusId, @UserRequestTypeId, @CreatedBy)";

                    var param = new DynamicParameters();
                    param.Add("@Word", request.Word);
                    param.Add("@RequestDefinitionOrDescription", request.RequestDefinitionOrDescription);
                    param.Add("@UserRequestStatusId", 1); //UserRequestStatusId: 1 is "Open/Pending Request" while 2 is "ResolvedRequest" (this will be 1 for both "RequestChangeToWord and RequestNewWord"...the words will later be marked as 2 via the ResolveRequest api).
                    param.Add("@UserRequestTypeId", request.UserRequestTypeId); //UserRequestTypeId: 1 is "RequestChangeToWord" while 2 is "RequestNewWord".
                    param.Add("@CreatedBy", request.CreatedBy);

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

        public async Task<int> ResolveRequest(long userRequestId, string resolvedBy)
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = @"UPDATE [dbo].[UserRequest] 
                                SET [UserRequestStatusId] = @UserRequestStatusId, 
                                    [IsUpdated] = @IsUpdated, 
                                    [UpdatedDate] = @UpdatedDate, 
                                    [UpdatedBy] = @UpdatedBy 
                                WHERE [UserRequestId] = @UserRequestId";

                    var param = new DynamicParameters();
                    param.Add("@UserRequestId", userRequestId);
                    param.Add("@UserRequestStatusId", 2); // Resolve Request
                    param.Add("@IsUpdated", true);
                    param.Add("@UpdatedDate", DateTime.Now);
                    param.Add("@UpdatedBy", resolvedBy);

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

        public async Task<IEnumerable<UserRequestListDTO>> GetAllUserRequests()
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    //Reminder: this commented same result as the current query being used.

                    //string sqlQuery = "SELECT a.UserRequestId, a.Word, a.RequestDefinitionOrDescription, a.UserRequestStatusId, b.UserRequestStatusName, a.UserRequestTypeId, c.UserRequestType, " +
                    //                    "a.CreatedDate, a.CreatedBy, a.IsUpdated, a.UpdatedDate, a.UpdatedBy, a.IsDeleted, a.DeletedDate, a.DeletedBy FROM[dbo].[UserRequest] a " +
                    //                    "INNER JOIN[dbo].[UserRequestStatus] b ON b.UserRequestStatusId = a.UserRequestStatusId " +
                    //                    "INNER JOIN[dbo].[UserRequestType] c ON c.UserRequestTypeId = a.UserRequestTypeId Where a.[IsDeleted] = 0";

                    string sqlQuery = "  SELECT a.*, b.UserRequestStatusName, c.UserRequestType FROM [dbo].[UserRequest] a " +
                                        "INNER JOIN [dbo].[UserRequestStatus] b ON b.UserRequestStatusId = a.UserRequestStatusId " +
                                        "INNER JOIN [dbo].[UserRequestType] c ON c.UserRequestTypeId = a.UserRequestTypeId " +
                                        "ORDER BY a.UserRequestId DESC"; //--adding orderby is to order in descending mannner, meaning last record inserted will be the first record returned.

                    var userRequests = await dapper.QueryAsync<UserRequestListDTO>(sqlQuery);
                    return userRequests;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<GeneralUserDashboardResponseDTO> GetGeneralUserDashboard()
        {
            try
            {
                var dashboardData = new GeneralUserDashboardResponseDTO();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    // Word of the day(randomly selected) //ORDER BY NEWID() will help select the word randomly
                    string wordOfTheDayQuery = @"SELECT TOP 1 *
                                                FROM [dbo].[Words]
                                                WHERE StatusId = 2 AND IsDeleted = 0
                                                ORDER BY NEWID()";

                    // Top 3 most searched words (last hour)
                    string topSearchedWordsQuery = @"SELECT TOP 3 a.*
                                                    FROM [dbo].[Words] a
                                                    JOIN [dbo].[WordSearches] b ON a.WordId = b.WordId
                                                    WHERE b.SearchDate > DATEADD(HOUR, -1, GETDATE())  AND a.IsDeleted = 0
                                                    GROUP BY a.WordId, a.Word, a.Class, a.Defination, a.Pronounciation, a.History, a.StatusId, a.CreatedDate, a.CreatedBy, 
                                                             a.IsUpdated, a.UpdatedDate, a.UpdatedBy, a.IsDeleted, a.DeletedDate, a.DeletedBy
                                                    ORDER BY COUNT(b.SearchId) DESC";

                    // Top 3 recently added words
                    string recentlyAddedWordsQuery = @"SELECT TOP 3 *
                                                     FROM [dbo].[Words]
                                                     WHERE StatusId = 2  AND IsDeleted = 0
                                                     ORDER BY CreatedDate DESC";

                    // Top 3 recently updated words
                    string recentlyUpdatedWordsQuery = @"SELECT TOP 3 *
                                                        FROM [dbo].[Words]
                                                        WHERE StatusId = 2 AND IsUpdated = 1 AND IsDeleted = 0
                                                        ORDER BY UpdatedDate DESC";

                    dashboardData.WordOfTheDay = await connection.QueryFirstOrDefaultAsync<WordDTO>(wordOfTheDayQuery);

                    dashboardData.TopSearchedWords = (await connection.QueryAsync<WordDTO>(topSearchedWordsQuery)).ToList();                

                    dashboardData.RecentlyAddedWords = (await connection.QueryAsync<WordDTO>(recentlyAddedWordsQuery)).ToList();                

                    dashboardData.RecentlyUpdatedWords = (await connection.QueryAsync<WordDTO>(recentlyUpdatedWordsQuery)).ToList();

                    return dashboardData;
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
