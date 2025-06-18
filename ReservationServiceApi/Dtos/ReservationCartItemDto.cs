namespace ReservationServiceApi.Dtos
{
    public class ReservationCartItemDto
    {
        public int ReservationId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public decimal Price { get; set; }
        public List<RoomDto> Rooms { get; set; } = new();
    }
}
