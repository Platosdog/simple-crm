using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrm.Web.Controllers
{
    [Route("about")]
    public class AboutController
    {
        [Route("phone"), Route("")]
        public string Phone()
        {
            return "555-555-1234";
        }
        [Route("Address")]
        public string Address()
        {
            return "USA";
        }
    }
}
