using CustomerServiceApi.Dtos;
using CustomerServiceApi.Entities;
using CustomerServiceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerServiceApi.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("customers")]
        public async Task<IEnumerable<Customer>> Read() => await _customerService.Get();

        [HttpGet("customers/{id}")]
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            var customer = await _customerService.GetById(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpPost("customer")]
        public async Task<IActionResult> Create([FromBody] CustomerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _customerService.Add(dto);
            return Ok();
        }

        [HttpDelete("customers/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetById(id);

            if (customer == null)
            {
                return NotFound();
            }

            await _customerService.Delete(id);
            return NoContent();
        }

        [HttpPatch("customers/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCustomer = await _customerService.GetById(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            await _customerService.Update(id, dto);

            return Ok();
        }
    }
}