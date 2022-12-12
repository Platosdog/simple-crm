using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace SimpleCrm.WebApi.ApiControllers
{
    [Route("api/customer")]
    public class CustomerController : Controller
    {
        [HttpGet("")] 
        public IActionResult GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")] 
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

    }
}
