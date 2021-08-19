using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models;
using SimpleCrm.Web.Models.Home;
using System.Linq;
using CustomerEditViewModel = SimpleCrm.Web.Models.Home.CustomerEditViewModel;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerData customerData;
        private readonly IGreeter _greeter;

        public HomeController(ICustomerData customerData, IGreeter greeter)
        {
            this.customerData = customerData;
            _greeter = greeter;
        }

        public IActionResult Details(int id)
        {
            var customer = customerData.Get(id);
            if (customer == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public IActionResult Index()
        {
            var model = new HomePageViewModel()
            {
                CurrentMessage = _greeter.GetGreeting(),
                Customers = customerData.GetAll().Select(x => new CustomerModel {
                    FirstName= x.FirstName, 
                    LastName= x.LastName, 
                    Id= x.Id, 
                    PhoneNumber= x.PhoneNumber
                })
            };
            return View(model);
        }

        [HttpGet()]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost()]
        public IActionResult Create(CustomerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    OptInNewsletter = model.OptInNewsletter,
                    Type = model.Type
                };
                customerData.Save(customer);

                return RedirectToAction(nameof(Details), new { id = customer.Id });
            }
            return View();
        }
    }
}
