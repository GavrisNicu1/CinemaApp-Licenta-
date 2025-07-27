using Microsoft.EntityFrameworkCore;
using CinemaApp.Models;

namespace CinemaApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<HallMovie> HallMovies { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // MovieActor: compus (MovieId + ActorId)
            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(ma => ma.ActorId);

            // HallMovie: compus (HallId + MovieId)
            modelBuilder.Entity<HallMovie>()
                .HasKey(hm => new { hm.HallId, hm.MovieId });

            modelBuilder.Entity<HallMovie>()
                .HasOne(hm => hm.Movie)
                .WithMany(m => m.HallMovies)
                .HasForeignKey(hm => hm.MovieId);

            modelBuilder.Entity<HallMovie>()
                .HasOne(hm => hm.Hall)
                .WithMany(h => h.HallMovies)
                .HasForeignKey(hm => hm.HallId);

            // Genre → Movie (FK)
            modelBuilder.Entity<Genre>()
                .HasOne(g => g.Movie)
                .WithMany(m => m.Genres)
                .HasForeignKey(g => g.MovieId);

            // Reservation → Hall & Movie (FK)
           
            modelBuilder.Entity<Reservation>()
                .HasKey(r => r.ReservationId);

            // Relația cu HallMovie (cheie compusă)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.HallMovie)
                .WithMany()
                .HasForeignKey(r => new { r.HallId, r.MovieId });



        }
    }
}
