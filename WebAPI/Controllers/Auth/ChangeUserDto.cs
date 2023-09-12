using WebAPI.Models.Enums;

namespace WebAPI.Controllers.Auth
{
    public class ChangeUserDto
    {
        public EUserType? UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public int CountryId { get; set; }
    }
}