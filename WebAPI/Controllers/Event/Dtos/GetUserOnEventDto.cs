using WebAPI.Models;

namespace WebAPI.Controllers.Event.Dtos
{
    public class GetUserOnEventDto
    {

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email{ get; set; }

        public bool EmailSent { get; set; }
        public Country Country { get; internal set; }
    }
}
