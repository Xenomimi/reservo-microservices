using ReservationServiceApi;
using Microsoft.EntityFrameworkCore;
using ReservationServiceApi.Resolver;
using ReservationServiceApi.Dtos;
using ReservationServiceApi.Entities;
using System.Runtime.InteropServices.Marshalling;

namespace ReservationServiceApi.Services
{
    public class ReservationService
    {
        private ReservationDbContext _context;
        private readonly CustomerResolver _customerResolver;
        private readonly DiscountResolver _discountResolver;

        public ReservationService(ReservationDbContext context, CustomerResolver customerResolver, DiscountResolver discountResolver)
        {
            _context = context;
            _customerResolver = customerResolver;
            _discountResolver = discountResolver;
        }

        public async Task<Reservation?> GetById(int id)
        {
            return await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<ReservationDto>> Get()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Rooms)
                .ToListAsync();

            return reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                CustomerExternalId = r.CustomerExternalId,
                CustomerName = r.CustomerName,
                ReservationStartDate = r.ReservationStartDate,
                ReservationEndDate = r.ReservationEndDate,
                TotalPrice = r.TotalPrice,
                Status = r.Status,
                ReservationCartId = r.ReservationCartId,
                Rooms = r.Rooms.ToList()
            });
        }

        public async Task Delete(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Rooms)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                throw new Exception("Nie znaleziono rezerwacji.");

            foreach (var room in reservation.Rooms)
            {
                room.RoomStatus = RoomStatus.Available;
                room.ReservationId = null;
            }

            _context.Reservations.Remove(reservation);

            await _context.SaveChangesAsync();
        }

        public async Task<int> Add(CreateReservationDto dto)
        {
            var selectedRooms = await _context.Rooms
                .Where(r => dto.RoomNumbers.Contains(r.RoomNumber) && r.RoomStatus == RoomStatus.Available)
                .ToListAsync();

            if (selectedRooms.Count != dto.RoomNumbers.Count)
                throw new Exception("Niektóre pokoje są już zarezerwowane.");

            var totalDays = (dto.EndDate - dto.StartDate).Days;
            var basePrice = selectedRooms.Sum(r => r.PricePerNight * totalDays);

            var customer = await _customerResolver.ResolveCustomer(dto.CustomerExternalId);
            if (customer == null)
                throw new Exception("Nie znaleziono klienta.");

            var reservation = new Reservation
            {
                CustomerExternalId = dto.CustomerExternalId,
                CustomerName = customer.FullName,
                ReservationStartDate = dto.StartDate,
                ReservationEndDate = dto.EndDate,
                Rooms = selectedRooms,
                TotalPrice = basePrice,
                Status = Status.InCart
            };

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            var existingCart = await _context.ReservationCarts
                .Include(c => c.Reservations)
                .FirstOrDefaultAsync(c => c.CustomerExternalId == customer.Id);

            if (existingCart != null)
            {
                existingCart.Reservations.Add(reservation);
                if (dto.PromoCode != null)
                {
                    existingCart.PromoCode = dto.PromoCode;
                }
            }
            else
            {
                var newCart = new ReservationCart
                {
                    CustomerExternalId = customer.Id,
                    PromoCode = dto.PromoCode,
                    DiscountApplied = 0,
                    Reservations = new List<Reservation> { reservation }
                };

                await _context.ReservationCarts.AddAsync(newCart);
            }

            await _context.SaveChangesAsync();
            return reservation.Id;
        }

        public async Task<IEnumerable<ReservationCartItemDto>> GetCartContent(int customerId)
        {
            var cart = await _context.ReservationCarts
                .Include(c => c.Reservations)
                    .ThenInclude(r => r.Rooms)
                .FirstOrDefaultAsync(c => c.CustomerExternalId == customerId);

            if (cart == null)
                return Enumerable.Empty<ReservationCartItemDto>();

            return cart.Reservations
                .Where(r => r.Status == Status.InCart)
                .Select(r => new ReservationCartItemDto
                {
                    ReservationId = r.Id,
                    CustomerName = r.CustomerName,
                    Start = r.ReservationStartDate,
                    End = r.ReservationEndDate,
                    Price = r.TotalPrice,
                    Rooms = r.Rooms.Select(room => new RoomDto
                    {
                        RoomNumber = room.RoomNumber,
                        PricePerNight = room.PricePerNight
                    }).ToList()
                }).ToList();
        }



        public async Task ConfirmCart(int customerId)
        {
            var customer = await _customerResolver.ResolveCustomer(customerId);
            if (customer == null)
                throw new Exception("Nie znaleziono klienta.");

            var cart = await _context.ReservationCarts
                .Include(c => c.Reservations)
                    .ThenInclude(r => r.Rooms)
                .FirstOrDefaultAsync(c => c.CustomerExternalId == customerId);

            if (cart == null)
                throw new Exception("Koszyk nie istnieje.");

            var reservationsToConfirm = cart.Reservations
                .Where(r => r.Status == Status.InCart)
                .ToList();

            if (!reservationsToConfirm.Any())
                throw new Exception("Koszyk nie zawiera żadnych aktywnych rezerwacji.");

            decimal discountPercentage = 0.0m;

            if (!string.IsNullOrWhiteSpace(cart.PromoCode))
            {
                var discountData = await _discountResolver.ResolveDiscount(cart.PromoCode);

                if (discountData != null && discountData.DiscountStatus == DiscountStatus.NotUsed && (!discountData.RequiresVipCustomer || customer.Info.IsVIP))
                {
                    discountPercentage = discountData.Percentage;
                    await _discountResolver.MarkDiscountAsUsed(discountData.Id);
                }
            }

            decimal totalDiscount = 0.0m;

            foreach (var reservation in reservationsToConfirm)
            {
                var days = (reservation.ReservationEndDate - reservation.ReservationStartDate).Days;
                var basePrice = reservation.Rooms.Sum(r => r.PricePerNight);
                basePrice = basePrice * days;
                Console.WriteLine("basePrice: ", basePrice);
                Console.WriteLine("discountPrecentage: ", discountPercentage);
                var reservationDiscount = basePrice * discountPercentage / 100;


                reservation.TotalPrice = basePrice - reservationDiscount;
                reservation.Status = Status.Confirmed;
                reservation.ReservationCartId = cart.Id;

                foreach (var room in reservation.Rooms)
                {
                    room.RoomStatus = RoomStatus.Reserved;
                }

                totalDiscount += reservationDiscount;
                Console.WriteLine(totalDiscount);
            }

            cart.DiscountApplied = totalDiscount;

            await _context.SaveChangesAsync();
        }

        public async Task CompleteReservation(int reservationId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Rooms)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
                throw new Exception("Nie znaleziono rezerwacji.");

            var now = DateTime.UtcNow;

            if (now < reservation.ReservationEndDate)
                throw new Exception("Nie można zakończyć rezerwacji przed jej końcem.");

            reservation.Status = Status.Completed;

            foreach (var room in reservation.Rooms)
            {
                room.RoomStatus = RoomStatus.Available;
            }

            reservation.Rooms.Clear();

            await _context.SaveChangesAsync();
        }

        public async Task CancelReservation(int reservationId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Rooms)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
                throw new Exception("Rezerwacja nie istnieje.");

            if (reservation.Status != Status.Confirmed)
                throw new Exception("Tylko potwierdzone rezerwacje mogą zostać anulowane.");

            var now = DateTime.UtcNow;
            if ((reservation.ReservationStartDate - now).TotalDays < 7)
                throw new Exception("Rezerwację można anulować tylko na minimum 7 dni przed rozpoczęciem.");

            reservation.Status = Status.Cancelled;

            foreach (var room in reservation.Rooms)
            {
                if (room.RoomStatus == RoomStatus.Reserved)
                    room.RoomStatus = RoomStatus.Available;
            }

            reservation.Rooms.Clear();

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetRoomsDto>> GetRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();

            return rooms.Select(r => new GetRoomsDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                PricePerNight = r.PricePerNight,
                RoomStatus = r.RoomStatus,
                ReservationId = r.ReservationId
            });
        }

        public async Task AddRoom(RoomDto dto)
        {
            var roomExists = await _context.Rooms.AnyAsync(r => r.RoomNumber == dto.RoomNumber);
            if (roomExists)
                throw new Exception("Pokój o takim numerze już istnieje.");

            var room = new Room
            {
                RoomNumber = dto.RoomNumber,
                PricePerNight = dto.PricePerNight,
                RoomStatus = RoomStatus.Available
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoom(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                throw new Exception("Pokój nie istnieje.");

            if (room.RoomStatus == RoomStatus.Reserved)
                throw new Exception("Nie można usunąć zarezerwowanego pokoju.");

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }

    }
}
