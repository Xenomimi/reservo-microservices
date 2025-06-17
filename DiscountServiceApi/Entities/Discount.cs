using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscountServiceApi.Entities
{
    [Table("Discount", Schema = "Reservo")]
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Code { get; set; } = null!;

        public DiscountStatus DiscountStatus { get; set; } = DiscountStatus.NotUsed;

        [MaxLength(300)]
        public string? Description { get; set; }

        [Required]
        public decimal Percentage { get; set; }

        public bool RequiresVipCustomer { get; set; } = false;
    }

    public enum DiscountStatus
    {
        NotUsed,
        Used
    }
}
