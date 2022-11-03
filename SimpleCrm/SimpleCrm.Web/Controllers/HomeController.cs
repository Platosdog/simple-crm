using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models;
using SimpleCrm.Web.Models.Account;
using SimpleCrm.Web.Models.Home;
using System.Linq;
using CustomerEditViewModel = SimpleCrm.Web.Models.Home.CustomerEditViewModel;

namespace SimpleCrm.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICustomerData customerData;
        

        public HomeController(ICustomerData customerData)
        {
            this.customerData = customerData;
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

        [HttpGet()]
        public IActionResult Edit(int id)
        {
            var customer = customerData.Get(id);
            var model = new CustomerEditViewModel
            {
                FirstName= customer.FirstName,
                LastName= customer.LastName,
                PhoneNumber= customer.PhoneNumber,
                OptInNewsletter= customer.OptInNewsletter
            };
            return View(model);
        }
        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public IActionResult Edit(CustomerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = customerData.Get(model.Id);

                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName; 
                customer.PhoneNumber = model.PhoneNumber; 
                customer.OptInNewsletter = model.OptInNewsletter;
                customer.Type = model.Type;
              
                customerData.Commit();
            }

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = new HomePageViewModel()
            {
                
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
        [ValidateAntiForgeryToken()]
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
                customerData.Commit();

                return RedirectToAction(nameof(Details), new { id = customer.Id });
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(RegisterUserViewModel model)
        {
            var register = new RegisterUserViewModel
            {
                DisplayName = model.DisplayName,
                UserName = model.UserName,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword

            };
            return View(model);

        }
    }
}
