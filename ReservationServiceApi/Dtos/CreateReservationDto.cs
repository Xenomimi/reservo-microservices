namespace ReservationServiceApi.Dtos
{
    public class CreateReservationDto
    {
        public int CustomerExternalId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<int> RoomIds { get; set; } = new();
        public string? PromoCode { get; set; }
    }
}
