using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
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
        private readonly LinkGenerator _linkGenerator;

        public CustomerController(ICustomerData customerData, LinkGenerator linkGenerator)
        {
            _customerData = customerData;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("", Name ="GetCustomers")] 
        public IActionResult GetAll([FromQuery] int page = 1,[FromQuery] int take = 50)
        {
            var customers = _customerData.GetAll(page -1, take, "");
            var next = GetCustomerResourceUri(page + 1, take);
            var pagination = new PaginationModel
            {
                Next = customers.Count < take ? null : GetCustomerResourceUri(page + 1, take),
                Previous = page <= 1? null : GetCustomerResourceUri(page - 1, take)
            }; 

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));
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
        private string GetCustomerResourceUri(int page, int take)
        {
            return _linkGenerator.GetPathByName(this.HttpContext, "GetCustomers", values: new {
                page = page,
                take = take
            });
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
        public IActionResult Create([FromBody] CustomerCreateViewModel model)
        {
            
            if (model == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailAddress = model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
                PeferredContactMethod = model.PeferredContactMethod
            };
            _customerData.Add(customer);
            _customerData.Commit();
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CustomerUpdateViewModel model)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
          
            {
                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.EmailAddress = model.EmailAddress;
                customer.PhoneNumber = model.PhoneNumber;
                customer.PeferredContactMethod = model.PeferredContactMethod;
            };
            _customerData.Update(customer);
            _customerData.Commit();
            return Ok(customer);
        }

        [HttpDelete("{id}")] 
        public IActionResult Delete(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return NotFound(); // 404
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            _customerData.Delete();
            _customerData.Commit();
            return NoContent(); // 204
        }

    }
}
