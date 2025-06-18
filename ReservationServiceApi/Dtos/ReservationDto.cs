using ReservationServiceApi.Entities;

namespace ReservationServiceApi.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int CustomerExternalId { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReservationStartDate { get; set; }
        public DateTime ReservationEndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Status Status { get; set; }
        public int? ReservationCartId { get; set; }
        public List<Room> Rooms { get; set; }
    }
}
