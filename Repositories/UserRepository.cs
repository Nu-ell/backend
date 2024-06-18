using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechDictionaryApi.DTOs;


namespace TechDictionaryApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO> Login(LoginDTO loginDTO)
        {
            //var newPassword = HashNewPassword(loginDTO.Password);   

            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "SELECT UserId, FirstName, LastName, UserName, PasswordHash, Email, PhoneNumber, State, Country, LGA, Role, DateCreated, CreatedBy, IsActive, RefreshToken, Token, PasswordHash FROM dbo.Users WHERE UserName = @UserName";
                    var param = new DynamicParameters();
                    param.Add("@UserName", loginDTO.UserName);

                    var user = await dapper.QueryFirstOrDefaultAsync<UserDTO>(sqlQuery, param: param, commandType: CommandType.Text);

                    bool doesPasswordMatch = ValidatePassword(loginDTO.Password, user.PasswordHash);

                    if (doesPasswordMatch == false)
                    {
                        throw new UnauthorizedAccessException("Invalid credentials");
                    }

                    string token = GenerateJwtToken(user);

                    var response = new LoginResponseDTO
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        State = user.State,
                        Country = user.Country,
                        LGA = user.LGA,
                        Role = user.Role,
                        DateCreated = user.DateCreated,
                        CreatedBy = user.CreatedBy,
                        IsActive = user.IsActive,
                        RefreshToken = user.RefreshToken,
                        Token = token
                    };

                    //update logged in before returning response

                    int updateLoggedIn = await UpdateLogin(loginDTO.UserName);
                    if (updateLoggedIn > 0)
                    {
                        return response;
                    }
                    else
                    {
                        throw new Exception("Error Occured while trying to update log in");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        private bool ValidatePassword(string password, string storedPasswordHash)
        {
            // Password validation logic here
            // We can use BCrypt to verify the password hash
            bool doesPasswordMatch = BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);
            return doesPasswordMatch;
        }

        private string HashNewPassword(string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
            return passwordHash;
        }

        private string GenerateJwtToken(UserDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), //[0]
                new Claim(ClaimTypes.Name, user.UserName), //[1]
                new Claim(ClaimTypes.Email, user.Email), //[2]
                new Claim(ClaimTypes.Role, user.Role) //[3]
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<LoginResponseDTO> GetUserByUserName(string userName)
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = "SELECT UserId, FirstName, LastName, UserName, PasswordHash, Email, PhoneNumber, State, Country, LGA, Role, DateCreated, CreatedBy, IsActive, RefreshToken, Token, PasswordHash FROM dbo.Users WHERE UserName = @UserName";
                    var param = new DynamicParameters();
                    param.Add("@UserName", userName);

                    var user = await dapper.QueryFirstOrDefaultAsync<UserDTO>(sqlQuery, param: param, commandType: CommandType.Text);

                    var response = new LoginResponseDTO
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        State = user.State,
                        Country = user.Country,
                        LGA = user.LGA,
                        Role = user.Role,
                        DateCreated = user.DateCreated,
                        CreatedBy = user.CreatedBy,
                        IsActive = user.IsActive,
                        RefreshToken = user.RefreshToken,
                        Token = user.Token
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
        }

        public async Task<int> UpdateLogin(string userName)
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = @"Update [dbo].[Users] SET IsLoggedIn = 1 WHERE UserName = @Username";

                    var param = new DynamicParameters();
                    param.Add("@Username", userName);

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

        public async Task<int> UpdateLogOut(string userName)
        {
            try
            {
                using (SqlConnection dapper = new SqlConnection(_connectionString))
                {
                    string sqlQuery = @"Update [dbo].[Users] SET IsLoggedIn = 0 WHERE UserName = @Username";

                    var param = new DynamicParameters();
                    param.Add("@Username", userName);

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
    }    
}