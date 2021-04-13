using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController
    {
        public string Index(string id)
        {
            return "Hello from a Controller " + id;
        }
    }
}
