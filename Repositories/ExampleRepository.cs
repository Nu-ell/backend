using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TechDictionaryApi.DTOs;

namespace TechDictionaryApi.Repositories
{
    public class ExampleRepository : IExampleRepository
    {
        private readonly string _connectionString;
        public ExampleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateExample(CreateExampleDTO createExample)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "INSERT INTO Example(WordExample, WordId, CreatedBy) " +
                                      "VALUES(@WordExample, @WordId, @CreatedBy)";
                    var param = new DynamicParameters();
                    param.Add("@WordExample", createExample.WordExample);
                    param.Add("@WordId", createExample.WordId);
                    param.Add("@CreatedBy", createExample.CreatedBy);

                    int resp = await connect.ExecuteAsync(sqlQuery, param: param, commandType: CommandType.Text);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<int> UpdateExample(UpdateExampleDTO updateExample)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "UPDATE Example SET WordExample = @WordExample, IsUpdated = 1, UpdatedDate = @UpdatedDate, UpdatedBy = @UpdatedBy " +
                                      "WHERE ExampleId = @ExampleId AND IsDeleted = 0";
                    var param = new DynamicParameters();
                    param.Add("@ExampleId", updateExample.ExampleId);
                    param.Add("@WordExample", updateExample.WordExample);
                    param.Add("@UpdatedDate", DateTime.Now);
                    param.Add("@UpdatedBy", updateExample.UpdatedBy);

                    int resp = await connect.ExecuteAsync(sqlQuery, param: param, commandType: CommandType.Text);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteExample(DeleteExampleDTO deleteExample)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "UPDATE Example SET IsDeleted = 1, DeletedDate = @DeletedDate, DeletedBy = @DeletedBy WHERE ExampleId = @ExampleId";
                    var param = new DynamicParameters();
                    param.Add("@ExampleId", deleteExample.ExampleId);
                    param.Add("@DeletedDate", DateTime.Now);
                    param.Add("@DeletedBy", deleteExample.DeletedBy);

                    int resp = await connect.ExecuteAsync(sqlQuery, param: param, commandType: CommandType.Text);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ExampleDTO>> GetExamplesByWordId(long wordId)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "SELECT * FROM Example WHERE WordId = @WordId AND IsDeleted = 0";

                    var param = new DynamicParameters();
                    param.Add("@WordId", wordId);

                    var examples = await connect.QueryAsync<ExampleDTO>(sqlQuery, param);
                    return examples;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<ExampleDTO> GetExampleById(long exampleId)
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "SELECT * FROM Example WHERE ExampleId = @ExampleId AND IsDeleted = 0";

                    var param = new DynamicParameters();
                    param.Add("@ExampleId", exampleId);

                    var resp = await dapper.QueryFirstOrDefaultAsync<ExampleDTO>(sqlQuery, param);
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
