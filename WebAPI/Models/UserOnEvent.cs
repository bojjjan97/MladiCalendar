using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    [PrimaryKey(nameof(UserId), nameof(EventId))]
    public class UserOnEvent
    {
        [Column(Order = 0)]
        public string UserId { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Column(Order = 1)]
        public long EventId { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ForeignKey("EventId")]
        public Event Event { get; set; }
        public bool EmailSent { get; set; } = false;
    }
}
