using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new CustomerModel
            {
                FirstName = "Jay", LastName = "Phillips"               
            };

            return View(model);
        }
    }
}
