using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleCrm.Web.Models.Account
{
    public class LoginUserViewModel
    {
        [Required, MaxLength(256), DisplayName("Email Address")]
        public string UserName { get; set; }

        [DisplayName("Name")]
        public string DisplayName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Incorrect Password")]
        public string ConfirmPassword { get; set; }
        public bool RememberMe { get; internal set; }
        public string ReturnUrl { get; internal set; }
    }
}
