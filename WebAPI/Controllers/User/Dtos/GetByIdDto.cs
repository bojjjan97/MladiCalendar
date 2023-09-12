using System.Diagnostics.Contracts;
using WebAPI.Models;

namespace WebAPI.Controllers.User.Dtos
{
    public class GetByIdDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserType { get; set; }

        public Country Country { get; set; }
        public string? PhoneNumber { get; internal set; }
    }
}
