using Microsoft.AspNetCore.Mvc;
using SimpleCrm.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var models = customers.Select(c => new CustomerDisplayViewModel
            {
                CustomerId = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                EmailAddress = c.EmailAddress,
                PhoneNumber = c.PhoneNumber,
                OptInNewsletter = c.OptInNewsletter,
                Type = c.Type,
                PeferredContactMethod = c.PeferredContactMethod,
                StatusCode = c.StatusCode
            });
            return Ok(models);
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
