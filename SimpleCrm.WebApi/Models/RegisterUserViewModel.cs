﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleCrm.WebApi.Models
{
    public class RegisterUserViewModel
    {
        [Required(), DisplayName("Email"), MaxLength(256), DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(), MaxLength(256), DataType(DataType.Text), DisplayName("Name")]
        public string DisplayName { get; set; }

        [Required(), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(), DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}