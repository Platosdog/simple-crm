using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleCrm.WebApi.Models;
using System; 
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrm.WebApi.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 60 * 60 * 24 * 3, Location = ResponseCacheLocation.Client)]
        [Route("")]
        public IActionResult Index()
        {
            throw new ApiException("An exceptional test. :)");
            // return View(); // <- comment this out
        }

        [Route("about")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Route("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
