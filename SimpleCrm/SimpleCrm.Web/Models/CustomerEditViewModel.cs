using System.ComponentModel.DataAnnotations;

namespace SimpleCrm.Web.Models.Home
{
    public class CustomerEditViewModel
    {
        
        public int Id { get; set; }
        [Display(Name = "First Name")] [MaxLength(12)] [Required] public string FirstName { get; set; } 
        [Display(Name = "Last Name")] [MaxLength(20)] [Required] public string LastName { get; set; }
        [Display(Name = "Phone")] [Required] [MinLength(10)] public string PhoneNumber { get; set; }
        [Display(Name = "Newsletter ?")] public bool OptInNewsletter { get; set; }
        public CustomerType Type { get; set; }
    }
}