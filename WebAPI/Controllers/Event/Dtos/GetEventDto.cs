using WebAPI.Models.Enums;

namespace WebAPI.Controllers.Event.Dtos
{
    public class GetEventDto
    {
        public long EventId { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
        public string? OnlineMeetingUrl { get; set; }
        public bool FreePlaces  { get; set; }
        public int NumberOfParticipants { get; set; }
        public int NumberOfRegisteredParticipants { get; set; }
        public string Description { get; set; }
        public bool CurrentUserAlreadyOnEvent { get; set; }
        public EUserType EventUserType { get; set; }
        public List<GetTrainerOnEventDto> Trainers { get; internal set; }

        public List<GetUserOnEventDto> UsersOnEvent { get; internal set; }
        public DateTime EventDateTime { get; internal set; }
    }
}
