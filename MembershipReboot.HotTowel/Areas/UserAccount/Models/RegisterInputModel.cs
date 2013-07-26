﻿using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MembershipReboot.HotTowel.Areas.UserAccount.Models
{
    [MetadataType(typeof(RegisterInputModel))]
    public class RegisterInputModel : IValidatableObject
    {
        [ScaffoldColumn(false)]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage="Password confirmation must match password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirmation")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!SecuritySettings.Instance.EmailIsUsername && 
                String.IsNullOrWhiteSpace(Username))
            {
                yield return new ValidationResult("Username is required", new string[] { "Username" });
            }
        }
    }
}