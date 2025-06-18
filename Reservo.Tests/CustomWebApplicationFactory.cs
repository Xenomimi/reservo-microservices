using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReservationServiceApi;
using ReservationServiceApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservo.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ReservationDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ReservationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("ReservoReservation");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
                    db.Database.EnsureCreated();

                    // dane testowe
                    db.Rooms.Add(new Room { RoomNumber = 5, PricePerNight = 100 });
                    db.SaveChanges();
                }
            });
        }
    }

}
