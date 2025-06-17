namespace ReservationServiceApi.Dtos
{
    public class CustomerInfoDto
    {
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsVIP { get; set; }
    }
}
