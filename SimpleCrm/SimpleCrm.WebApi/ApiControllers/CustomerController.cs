using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace SimpleCrm.WebApi.ApiControllers
{
    [Route("api/customer")]
    public class CustomerController : Controller
    {
          
    private readonly ICustomerData _customerData;

        public CustomerController(ICustomerData customerData)
        {
            _customerData = customerData;
        }

        [HttpGet("")] 
        public IActionResult GetAll()
        {
            var customers = _customerData.GetAll(0, 50, "");
            // Note: renaming GetStatus(...) to ^^GetAll(...) and removing Status parameter
            return Ok(customers); //200
        }

        [HttpGet("{id}")] 
        public IActionResult Get(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound(); // 404
            }
            return Ok(customer); // 200
        }

        [HttpPost("")] 
        public IActionResult Create([FromBody] Customer model)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")] 
        public IActionResult Update(int id, [FromBody] Customer model)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")] 
        public IActionResult Delete(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound(); // 404
            }
            _customerData.Delete();
            _customerData.Commit();
            return NoContent(); // 204
        }

    }
}
