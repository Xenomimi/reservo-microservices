namespace CustomerServiceApi.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public CustomerInfoDto Info { get; set; } = null!;
    }
}
