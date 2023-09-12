using WebAPI.Models;
using WebAPI.Models.Enums;

namespace WebAPI.Controllers.Event.Dtos
{
    public class GetEventByIdDto
    {
        public long EventId { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? OnlineMeetingUrl { get; set; }
        public bool FreePlaces { get; set; }
        public int NumberOfParticipants { get; set; }
        public int NumberOfRegisteredParticipants { get; set; }
        public string Description { get; set; }

        public List<GetUserOnEventDto> UsersOnEvent { get; set; }
        public EUserType EventUserType { get; set; }
        public List<string> Trainers { get; internal set; }
        public DateTime EventDateTime { get; internal set; }
    }
}
