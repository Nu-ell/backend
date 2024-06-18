namespace TechDictionaryApi.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? LGA { get; set; }
        public string? Role { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public string? Token { get; set; }
        public string? PasswordHash { get; set; }
    }

    public class LoginDTO
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? LGA { get; set; }
        public string? Role { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public string? Token { get; set; }
    }

}
