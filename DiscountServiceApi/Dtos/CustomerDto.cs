using System.ComponentModel.DataAnnotations;

namespace DiscountServiceApi.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required, MinLength(9), MaxLength(13)]
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsVIP { get; set; } = false;
    }
}
