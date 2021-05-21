using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models;
namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        private ICustomerData customerData;
        private readonly IGreeter greeter;

        public HomeController(ICustomerData customerData, IGreeter greeter)
        {
            this.customerData = customerData;
            this.greeter = greeter;
        }

        public IActionResult Index()
        {
             var model = new HomePageViewModel()
            {
                CurrentMessage = greeter.GetGreeting(),
                Customers = customerData.GetAll()
            };
            return View(model);
        }
    }
}
