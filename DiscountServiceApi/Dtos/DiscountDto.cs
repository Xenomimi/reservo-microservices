namespace DiscountServiceApi.Dtos
{
    public class DiscountDto
    {
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Percentage { get; set; }
        public bool RequiresVipCustomer { get; set; }
    }

    public enum DiscountStatus
    {
        NotUsed,
        Used
    }
}
