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
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        public DateTime ReservationStartDate { get; set; }
        public DateTime ReservationEndDate { get; set; }
        public decimal TotalPrice { get; set; } = 0.0m;
        public Status Status { get; set; } = Status.New;

        // Kolekcja pokoi przypisanych do rezerwacji
        public ICollection<Room> Rooms { get; set; } = new List<Room>();

        // Klucz obcy do koszyka rezerwacji
        public int? ReservationCartId { get; set; }

        [ForeignKey(nameof(ReservationCartId))]
        public ReservationCart? Cart { get; set; }
    }

    public enum Status
    {
        New,
        InCart,
        Confirmed,
        Completed,
        Cancelled
    }
}