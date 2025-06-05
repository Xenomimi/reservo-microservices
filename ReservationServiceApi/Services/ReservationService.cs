using ReservationServiceApi.Entities;
using ReservationServiceApi;
using Microsoft.EntityFrameworkCore;

namespace ReservationServiceApi.Services
{
    public class ReservationService
    {
        private ReservationDbContext _context;

        public ReservationService(ReservationDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation> GetById(int id)
        {
            return await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Reservation>> Get()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task Add(Reservation entity)
        {
            _context.Set<Reservation>()
                    .Add(entity);

            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Update(Reservation updatedReservation)
        {
            var existing = await _context.Reservations.FindAsync(updatedReservation.Id);
            if (existing != null)
            {
                existing.ReservationStartDate = updatedReservation.ReservationStartDate;
                existing.ReservationEndDate = updatedReservation.ReservationEndDate;
                existing.Status = updatedReservation.Status;
                existing.TotalPrice = updatedReservation.TotalPrice;

                await _context.SaveChangesAsync();
            }
        }
    }
}
