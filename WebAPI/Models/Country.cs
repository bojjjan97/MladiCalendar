namespace WebAPI.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public virtual ICollection<Trainer> Trainers { get; set; }
        public virtual ICollection<Event> Events { get; set; }


    }
}
