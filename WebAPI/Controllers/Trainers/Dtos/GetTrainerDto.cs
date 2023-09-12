namespace WebAPI.Controllers.Trainers.Dtos
{
    public class GetTrainerDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Country { get; set; }
    }
}
