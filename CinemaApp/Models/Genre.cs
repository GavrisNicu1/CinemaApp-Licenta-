namespace CinemaApp.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public int MovieId { get; set; }
        public string GenreDescription { get; set; }

        public Movie? Movie { get; set; }
    }
}
