namespace WebAPI.Controllers.Trainers.Dtos
{
    public class CreateTranierDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int CountryId { get; set; }
    }
}
