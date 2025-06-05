using CustomerServiceApi.Entities;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationServiceApi.Entities
{
    [Table("Reservation", Schema = "Reservo")]
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomerExternalId { get; set; }
        [Required, MaxLength(200)]
        public string CustomerName { get; set; } = null!;
        [Required]
        public DateTime ReservationStartDate { get; set; }
        [Required]
        public DateTime ReservationEndDate { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
    }

    public enum Status
    {
        Nowa,
        Potwierdzona,
        Anulowana,
    }
}