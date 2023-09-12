using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Enums;

namespace WebAPI.Models
{
    public class User : IdentityUser
    {
        public EUserType UserType { get; set; }
        public ICollection<UserOnEvent> AttendedEvents { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CountryId { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }    
    }
}
