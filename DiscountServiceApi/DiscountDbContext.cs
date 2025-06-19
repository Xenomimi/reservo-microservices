using DiscountServiceApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscountServiceApi
{
    public class DiscountDbContext : DbContext
    {
        private IConfiguration _configuration { get; }
        public DbSet<Discount> Discounts { get; set; }

        public DiscountDbContext(IConfiguration configuration) : base() { _configuration = configuration; }
        public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(@"Server=localhost;Port=5432;database=ReservoDiscount;User ID=dev;Password=devpass",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Reservo"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
