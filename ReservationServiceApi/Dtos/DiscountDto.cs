namespace ReservationServiceApi.Dtos
{
    public class DiscountDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public DiscountStatus DiscountStatus { get; set; } = DiscountStatus.Used; // np. "Used" albo "NotUsed"

        public decimal Percentage { get; set; }

        public bool RequiresVipCustomer { get; set; }
    }

    public enum DiscountStatus
    {
        NotUsed,
        Used
    }
}
