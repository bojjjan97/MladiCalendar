using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class MainContext : IdentityDbContext<User>
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<UserOnEvent> UsersOnEvents{get;set;}
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainerOnEvent> TrainerOnEvents { get; set; }
        public DbSet<Country> Countries { get; set; }

        public MainContext(DbContextOptions<MainContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
