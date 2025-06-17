using DiscountServiceApi.Dtos;
using DiscountServiceApi.Entities;
using DiscountServiceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiscountServiceApi.Controllers
{
    public class DiscountController : ControllerBase
    {
        private readonly DiscountService _discountService;

        public DiscountController(DiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet("discounts/code/{code}")]
        public async Task<IActionResult> GetDiscountByPromoCode(string code)
        {
            var discount = await _discountService.GetByPromoCode(code);
            if (discount == null)
                return NotFound();
            return Ok(discount);
        }

        [HttpPost("discount")]
        public async Task<IActionResult> Create([FromBody] DiscountDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _discountService.Add(dto);
            return Ok();
        }

        [HttpPatch("discounts/{id}/mark-as-used")]
        public async Task<IActionResult> MarkAsUsed(int id)
        {
            try
            {
                await _discountService.MarkAsUsed(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Discount not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}