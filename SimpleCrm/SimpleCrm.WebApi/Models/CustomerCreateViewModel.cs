
namespace SimpleCrm.WebApi.Models
{
    public class CustomerCreateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool OptInNewsletter { get; set; }
        public CustomerType Type { get; set; }
        public string EmailAddress { get; set; }
        public InteractionMethod PeferredContactMethod { get; set; }
    }
}
