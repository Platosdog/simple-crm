using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models.Account;

namespace SimpleCrm.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        private readonly UserManager<CrmUser> userManager;
        public AccountController(UserManager<CrmUser> userManager)
        {  
            this.userManager = userManager;
        }
        private readonly SignInManager<CrmUser> signInManager;
        public AccountController(SignInManager<CrmUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new CrmUser
                {
                    UserName = model.UserName,
                    DisplayName = model.DisplayName,
                    Email = model.UserName
                };
                var createResult = await this.userManager.CreateAsync(user, model.Password);
                if (createResult.Succeeded)
                {
                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var result in createResult.Errors)
                {
                    ModelState.AddModelError("", result.Description);
                }
                return NoContent(); 
            }
            return View();
        }
    }
}
