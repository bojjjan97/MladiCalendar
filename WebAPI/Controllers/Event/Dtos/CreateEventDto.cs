using WebAPI.Models.Enums;

namespace WebAPI.Controllers.Event.Dtos
{
    public class CreateEventDto
    {
        public string? Description { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
        public string? OnlineMeetingUrl { get; set; }
        public int NumberOfParticipants { get; set; } = 18;
        public int? CountryId { get; set; }
        public DateTime EventDateTime { get; set; }
        public EUserType EventForUserType { get; set; }
        public ICollection<long> TrainerIds { get; set; }
    }
}
