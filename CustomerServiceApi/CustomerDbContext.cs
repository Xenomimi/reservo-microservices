using CustomerServiceApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerServiceApi
{
    public class CustomerContextDb : DbContext
    {
        private IConfiguration _configuration { get; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerInfo> CustomerInfos { get; set; }

        public CustomerContextDb(IConfiguration configuration) : base() { _configuration = configuration; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(@"Server=localhost;Port=5432;database=ReservoCustomer;User ID=dev;Password=devpass",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Reservo"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Info)
                .WithOne()
                .HasForeignKey<CustomerInfo>(ci => ci.CustomerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}