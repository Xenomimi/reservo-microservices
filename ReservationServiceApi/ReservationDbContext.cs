﻿using Microsoft.EntityFrameworkCore;
using ReservationServiceApi.Entities;

namespace ReservationServiceApi
{
    public class ReservationDbContext : DbContext
    {
        private IConfiguration _configuration { get; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ReservationCart> ReservationCarts { get; set; }

        public ReservationDbContext(IConfiguration configuration) : base() { _configuration = configuration; }

        public ReservationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(@"Server=localhost;Port=5432;database=ReservoReservation;User ID=dev;Password=devpass",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Reservo"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
