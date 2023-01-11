
using System.ComponentModel.DataAnnotations;

namespace SimpleCrm.WebApi.Models
{
    public class CustomerCreateViewModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MinLength(1), MaxLength(50)]
        [Required()]
        public string LastName { get; set; }
        [MinLength(7), MaxLength(12)]
        public string PhoneNumber { get; set; }
        public bool OptInNewsletter { get; set; }
        public CustomerType Type { get; set; }
        public string EmailAddress { get; set; }
        public InteractionMethod PeferredContactMethod { get; set; }
    }
}
