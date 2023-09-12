using WebAPI.Models.Enums;

namespace WebAPI.Controllers.Auth.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public EUserType? UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public int CountryId { get; set; }
    }
}
