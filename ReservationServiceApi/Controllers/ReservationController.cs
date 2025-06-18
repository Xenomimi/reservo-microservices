using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ReservationServiceApi.Dtos;
using ReservationServiceApi.Services;
using ReservationServiceApi.Entities;
using Newtonsoft.Json.Linq;

namespace ReservationServiceApi.Controllers
{
    public class ReservationController : ControllerBase
    {
        private ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("reservations")]
        public async Task<IActionResult> Get()
        {
            var discount = await _reservationService.Get();
            if (discount == null)
                return NotFound();
            return Ok(discount);
        }


        [HttpPost("reservation")]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _reservationService.Add(dto);
            return Ok();
        }

        [HttpGet("cart/{customerId}")]
        public async Task<IActionResult> GetCartContent(int customerId)
        {
            var cartContent = await _reservationService.GetCartContent(customerId);
            if (cartContent == null)
                return NotFound();
            return Ok(cartContent);
        }

        [HttpPatch("cart/{customerId}/confirm")]
        public async Task<IActionResult> ConfirmCart(int customerId)
        {
            try
            {
                await _reservationService.ConfirmCart(customerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("reservation/{Id}/complete")]
        public async Task<IActionResult> CompleteReservation(int Id)
        {
            try
            {
                await _reservationService.CompleteReservation(Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("reservation/{Id}/cancel")]
        public async Task<IActionResult> CancelReservation(int Id)
        {
            try
            {
                await _reservationService.CancelReservation(Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("reservation/{Id}")]
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

        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _reservationService.GetRooms();
            if (rooms == null)
                return NotFound();
            return Ok(rooms);
        }

        [HttpPost("room")]
        public async Task<IActionResult> AddRoom([FromBody] CreateRoomDto dto)
        {
            try
            {
                await _reservationService.AddRoom(dto);
                return Ok("Pokój dodany.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("room/{roomId}")]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            try
            {
                await _reservationService.DeleteRoom(roomId);
                return Ok("Pokój usunięty.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPut("reservation/{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] Reservation updatedReservation)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var existingReservation = await _reservationService.GetById(id);
        //    if (existingReservation == null)
        //    {
        //        return NotFound();
        //    }

        //    updatedReservation.Id = id;
        //    await _reservationService.Update(updatedReservation);

        //    return NoContent();
        //}
    }
}