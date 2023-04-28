using System;
namespace SimpleCrm.WebApi.Models
{
    public class UserSummaryViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Jwt { get; set; }
        public string Password { get; set; }
        public System.Collections.Generic.IList<string> Roles { get; set; }
    }
}