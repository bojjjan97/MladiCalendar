using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Enums;

namespace WebAPI.Models
{
    public class Event
    {
        public long Id { get; set; }
        public DateTime EventDateTime { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
        public string? OnlineMeetingUrl { get; set; }
        public int NumberOfParticipants { get; set; } = 18;
        public EUserType EventForUserType { get; set; }

        [DeleteBehavior(DeleteBehavior.Cascade)]
        public virtual ICollection<UserOnEvent>? UsersOnEvent { get; set; }

        [DeleteBehavior(DeleteBehavior.Cascade)]
        public virtual ICollection<TrainerOnEvent>? TrainersOnEvent { get; set; }


    }
}
