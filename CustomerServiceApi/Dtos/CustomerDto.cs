namespace CustomerServiceApi.Dtos
{
    public class CustomerDto
    {
        public string FullName { get; set; } = null!;
        public CustomerInfoDto Info { get; set; } = null!;
    }
}
