using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int id)
        { 
            if (id == 8)
            {
                return Forbid();
            }
            if (id == 5)
            {
                return NotFound();
            }
            var model = new CustomerModel
            {
                Id = id, FirstName = "John", LastName = "Doe", PhoneNumber = "123-456-7890" 
                
            };


            return new ObjectResult(model);
        }
    }
}
