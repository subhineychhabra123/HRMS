using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class RegistrationVM
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public CompanyVM Company { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email Id")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Registration.PasswordConfirmPasswordDoesNotMatches")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Note")]
        public string UserNote { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ActionType { get; set; }
        public string Comments { get; set; }
        public bool Active { get; set; }
    }
}