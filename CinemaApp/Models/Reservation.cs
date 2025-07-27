using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string SeatNumber { get; set; }  // Ex: A5

        // Cheie străină compusă spre HallMovie
        public int HallId { get; set; }
        public int MovieId { get; set; }

        [ForeignKey(nameof(HallId) + "," + nameof(MovieId))]
        public HallMovie? HallMovie { get; set; }

        // Acces ușor la datele filmului sau salii fara sa intru in  HallMovie)
        public Hall? Hall { get; set; }
        public Movie? Movie { get; set; }

        public DateTime ReservationDate { get; set; } = DateTime.Now;
    }
}
