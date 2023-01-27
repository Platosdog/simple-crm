using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SimpleCrm.Web.ViewComponents
{
    public class LoginLogoutViewComponent : ViewComponent
    {
        private readonly UserManager<CrmUser> _userManager;

        public LoginLogoutViewComponent(UserManager<CrmUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.Name == null)
                return View(new CrmUser());

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }

    }
}
