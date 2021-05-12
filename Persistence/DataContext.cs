using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheatreApi.Entities;

namespace TheatreApi.Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
       public DataContext(DbContextOptions options) : base(options){

        }
        
        public DbSet<Auditorium> Auditoriums { get; set; }
        public DbSet<Seat> Seats {get; set;}


        public DbSet<Play> Plays { get; set; }
        
        public DbSet<Reservation> Reservations { get; set; }
        protected override void OnModelCreating(ModelBuilder builder){

            base.OnModelCreating(builder);

            // builder.Entity<Reservation>()
            // .HasOne(p => p.User)
            // .WithMany(b => b.Reservations);

            // builder.Entity<Reservation>()
            // .HasOne(p => p.Play)
            // .WithMany(b => b.Reservations);

            // builder.Entity<Play>()
            // .HasOne(p => p.Auditorium)
            // .WithMany(b => b.Plays);





        }
    }
    
}