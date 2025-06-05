/*GET /api/customers – lista klientów

GET /api/customers/{id} – szczegóły klienta

POST /api/customers – utworzenie klienta

PUT /api/customers/{id} – aktualizacja klienta

DELETE /api/customers/{id} – usunięcie klienta*/

using CustomerServiceApi.Entities;
using CustomerServiceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Reservo.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        //[HttpGet("customers")]
        //public async Task<IEnumerable<Customer>> Read() => await _customerService.Get();

        //[HttpGet("customers/{id}")]
        //public async Task<IActionResult> ReadById(int id)
        //{
        //    var customer = await _customerService.GetById(id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(customer);
        //}

        [HttpPost("customer")]
        public async Task<IActionResult> Create([FromBody] Customer dto)
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

        [HttpPut("customers/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Customer updatedCustomer)
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

            updatedCustomer.Id = id;
            await _customerService.Update(updatedCustomer);

            return NoContent();
        }
    }
}