namespace CinemaApp.Models
{
    public class SeatViewModel
    {
        public string Row { get; set; } = string.Empty;       // Ex: "A", "B", "C"
        public int Column { get; set; }                       // Ex: 1, 2, 3
        public string SeatNumber => $"{Row}{Column}";         // Ex: "A1", "B3"
        public bool IsReserved { get; set; }
        public decimal Price { get; set; }                    // Prețul biletului
    }
}
