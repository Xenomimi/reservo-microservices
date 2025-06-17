using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationServiceApi.Entities
{
    [Table("ReservationCart", Schema = "Reservo")]
    public class ReservationCart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerExternalId { get; set; }

        public string? PromoCode { get; set; }

        public decimal DiscountApplied { get; set; } = 0.0m;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
