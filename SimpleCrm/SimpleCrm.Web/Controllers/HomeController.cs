using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string id)
        { 
            if (id == "8")
            {
                return Forbid();
            }
            if (id == "5")
            {
                return NotFound();
            }
            var model = new
            {
                Id = 1, FirstName = "John", LastName = "Doe", PhoneNumber = ("123-456-7890") 
                
            };


            return new ObjectResult(model);
        }
    }
}
