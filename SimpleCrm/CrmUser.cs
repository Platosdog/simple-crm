using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimpleCrm
{
    public class CrmUser : IdentityUser
    {
        [MaxLength(256)]
        public string DisplayName { get; set; }
    }
}
