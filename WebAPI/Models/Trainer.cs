using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Trainer
    {
        public  long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int CountryId { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }




    }
}
