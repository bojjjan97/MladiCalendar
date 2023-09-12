using WebAPI.Controllers.Countries.Dtos;
using WebAPI.Models;

namespace WebAPI.Controllers.Trainers.Dtos
{
    public class GetTrainerByIdDto
    {
            public long Id { get; set; }
            public string FirstName { get; set; }
             public string LastName { get; set; }
             public string Email { get; set; }
            public string PhoneNumber { get; set; }

            public GetCountryDto Country { get; set; }
        
    }
}
