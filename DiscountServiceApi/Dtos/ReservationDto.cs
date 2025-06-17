using System.ComponentModel.DataAnnotations;


namespace DiscountServiceApi.Dtos
{
    public class ReservationDto
    {
        [Required]
        public int CustomerExternalId { get; set; }

        [Required]
        public DateTime ReservationStartDate { get; set; }

        [Required]
        public DateTime ReservationEndDate { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa od zera")]
        public decimal TotalPrice { get; set; }
    }

    public enum Status
    {
        Nowa,
        Potwierdzona,
        Anulowana
    }
}
