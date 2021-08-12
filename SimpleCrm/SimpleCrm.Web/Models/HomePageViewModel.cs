using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCrm.Web.Models
{
    public class HomePageViewModel
    {
        public string CurrentMessage { get; set; }
        public IEnumerable<CustomerModel> Customers { get; set; }
    }
}
