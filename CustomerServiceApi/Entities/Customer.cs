using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerServiceApi.Entities
{
    [Table("Customer", Schema = "Reservo")]
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string FullName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required, MinLength(9), MaxLength(13)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
