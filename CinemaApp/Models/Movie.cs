using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public string Language { get; set; }
        public int Duration { get; set; }

        [NotMapped]
        public List<int>? SelectedActorIds { get; set; }


        public ICollection<Genre>? Genres { get; set; }
        public ICollection<MovieActor>? MovieActors { get; set; }
        public ICollection<HallMovie>? HallMovies { get; set; }
    }
}
