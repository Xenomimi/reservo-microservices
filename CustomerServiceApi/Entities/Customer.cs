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
        public CustomerInfo Info { get; set; } = null!;
    }
}
