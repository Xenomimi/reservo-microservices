using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace ReservationServiceApi.Entities
{
    [Table("Room", Schema = "Reservo")]
    [Index(nameof(RoomNumber), IsUnique = true)]
    public class Room
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RoomNumber { get; set; }
        public RoomStatus RoomStatus { get; set; } = RoomStatus.Available;
        public decimal PricePerNight { get; set; } = 0.0m;
        public int? ReservationId { get; set; }

        [ForeignKey(nameof(ReservationId))]
        public Reservation? Reservation { get; set; }
    }

    public enum RoomStatus
    {
        Available,
        Reserved,
        UnderMaintenance
    }
}
