using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerServiceApi.Entities
{
    [Table("CustomerInfo", Schema = "Reservo")]
    public class CustomerInfo
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, MinLength(9), MaxLength(13)]
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsVIP { get; set; } = false;
    }
}
