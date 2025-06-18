using System.ComponentModel.DataAnnotations;

namespace ReservationServiceApi.Dtos
{
    public class CreateReservationDto
    {
        public int CustomerExternalId { get; set; }
        //public string CustomerName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<int> RoomNumbers { get; set; } = new();
        public string? PromoCode { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "Data zakończenia musi być późniejsza niż data rozpoczęcia.",
                    new[] { nameof(EndDate) });
            }
        }
    }
}
