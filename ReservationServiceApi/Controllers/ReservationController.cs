/*GET / api / reservations – lista rezerwacji
GET /api/reservations/{id} – szczegóły rezerwacji
POST /api/reservations – utworzenie rezerwacji
PUT /api/reservations/{id}/ confirm – potwierdzenie
PUT /api/reservations/{id}/ cancel – anulowanie*/

using ReservationServiceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CustomerServiceApi.Entities;
using CustomerServiceApi.Services;

namespace ReservationServiceApi.Services
{
    public class ReservationController : ControllerBase
    {
        private ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        //[HttpGet("customers")]
        //public async Task<IEnumerable<Customer>> Read() => await _reservationService.Get();

        //[HttpGet("customers/{id}")]
        //public async Task<IActionResult> ReadById(int id)
        //{
        //    var customer = await _reservationService.GetById(id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(customer);
        //}

        [HttpPost("reservation")]
        public async Task<IActionResult> Create([FromBody] Reservation dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _reservationService.Add(dto);
            return Ok();
        }

        [HttpDelete("reservations/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _reservationService.GetById(id);

            if (customer == null)
            {
                return NotFound();
            }

            await _reservationService.Delete(id);
            return NoContent();
        }

        [HttpPut("reservations/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Reservation updatedReservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingReservation = await _reservationService.GetById(id);
            if (existingReservation == null)
            {
                return NotFound();
            }

            updatedReservation.Id = id;
            await _reservationService.Update(updatedReservation);

            return NoContent();
        }
    }
}