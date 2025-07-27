using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models
{
    public class HallMovie
    {
        public int HallId { get; set; }
        public Hall? Hall { get; set; }

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        [Display(Name = "Dată început")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Dată sfârșit")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

    }
}
