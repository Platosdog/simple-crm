using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimpleCrm
{
    public class Customer
    {
        public int Id { get; set; }
        [MaxLength(50)] 
        [Required()] 
        public string FirstName { get; set; }
        [MinLength(1), MaxLength(50)] 
        [Required()] 
        public string LastName { get; set; }
        [MinLength(7), MaxLength(12)] 
        public string PhoneNumber { get; set; }
        public bool OptInNewsletter { get; set; } 
        public CustomerType Type { get; set; }
        [Required()]
        public string EmailAddress { get; set; }
        public InteractionMethod PeferredContactMethod { get; set; }
        public CustomerStatus StatusCode { get; set; }
        public CustomerStatus Status { get; set; }
    }
    public enum InteractionMethod
    {
        None = 0,
        Email = 1,
        Phone = 2
    }
    public enum CustomerStatus
    {
        initial = 0,
        purchased = 1,
        prospect = 2,
        unknown = 3
    }
}