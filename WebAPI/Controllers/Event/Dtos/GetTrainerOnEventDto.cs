using WebAPI.Models;

namespace WebAPI.Controllers.Event.Dtos
{
    public class GetTrainerOnEventDto
    {
        public long TrainerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Country Country { get; set; }
    }
}
