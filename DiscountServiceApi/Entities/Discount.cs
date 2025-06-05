using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscountServiceApi.Entities
{
    [Table("Discount", Schema = "Reservo")]
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomerExternalId { get; set; }
        [Required, MaxLength(200)]
        public string CustomerName { get; set; } = null!;
        [Required]
        public int DiscountPercent { get; set; }
        [Required]
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

    }
}
