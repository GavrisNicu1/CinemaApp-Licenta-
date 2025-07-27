namespace CinemaApp.Models
{
    public class Hall
    {
        public int HallId { get; set; }
        public string HallName { get; set; }

        public ICollection<HallMovie>? HallMovies { get; set; }
    }
}
