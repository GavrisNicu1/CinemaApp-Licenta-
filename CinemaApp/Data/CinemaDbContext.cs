using Microsoft.EntityFrameworkCore; // <- Fără asta, DbContext și DbSet nu sunt recunoscute

using CinemaApp.Models;

namespace CinemaApp.Data
{
    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
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
            // Configurare many-to-many Movie <-> Actor
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

            // Configurare many-to-many Hall <-> Movie
            modelBuilder.Entity<HallMovie>()
                .HasKey(hm => new { hm.HallId, hm.MovieId });

            modelBuilder.Entity<HallMovie>()
                .HasOne(hm => hm.Hall)
                .WithMany(h => h.HallMovies)
                .HasForeignKey(hm => hm.HallId);
                
            modelBuilder.Entity<HallMovie>()
                .HasOne(hm => hm.Movie)
                .WithMany(m => m.HallMovies)
                .HasForeignKey(hm => hm.MovieId);

            // Relație către Hall
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Hall)
                .WithMany()
                .HasForeignKey(r => r.HallId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relație către Movie
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Movie)
                .WithMany()
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relație compusă către HallMovie
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.HallMovie)
                .WithMany()
                .HasForeignKey(r => new { r.HallId, r.MovieId })
                .OnDelete(DeleteBehavior.Restrict); // Sau NoAction


            base.OnModelCreating(modelBuilder);
        }
    }
}
