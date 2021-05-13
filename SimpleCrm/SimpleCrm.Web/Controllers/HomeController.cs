using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        private ICustomerData ICustomerData;
        private readonly IGreeter greeter;

        public HomeController(ICustomerData customerData, IGreeter greeter)
        {
            ICustomerData = customerData;
            this.greeter = greeter;
        }

        public IActionResult Index()
        {
            var model = new HomePageViewModel()
            {
                CurrentMessage = greeter.GetGreeting(),
                Customers = ICustomerData.GetAll()
            };
            return View(model);
        }
    }
}
