using ReservationServiceApi.Entities;

namespace ReservationServiceApi.Dtos
{
    public class GetRoomsDto
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public decimal PricePerNight { get; set; }
        public int? ReservationId { get; set; }
    }
}
