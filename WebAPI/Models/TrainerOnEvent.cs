using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
    [PrimaryKey(nameof(TrainerId), nameof(EventId))]
    public class TrainerOnEvent
    {

        [Column(Order = 0)]
        public long TrainerId { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ForeignKey("TrainerId")]
        public Trainer Trainer { get; set; }

        [Column(Order = 1)]
        public long EventId { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}
