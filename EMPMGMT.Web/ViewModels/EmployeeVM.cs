using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class EmployeeVM
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

        [Required(ErrorMessage = "Date of Joining is required")]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> DateOfJoining { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Registration.PasswordConfirmPasswordDoesNotMatches")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Note")]
        public string UserNote { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ActionType { get; set; }
        public string Comments { get; set; }
        public int Status { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }

        public string ReportName { get; set; }
        public string NewPassword { get; set; }
        public Nullable<System.DateTime> LinkSendDate { get; set; }
        public string ProfileName { get; set; }
        public string OrgUnitName { get; set; }
        public string ProfileId { get; set; }
        public string OrgUnitId { get; set; }
        
        public string ReferrerId { get; set; }
        public string ReferrerName { get; set; }
        public string TechnologyId { get; set; }
        public string TechnologyName { get; set; }
        public string DesignationId { get; set; }
        public string DesignationName { get; set; }

        public string ImageURL { get; set; }
        public string UserGuid { get; set; }
        public string CompanyName { get; set; }
        public string CompanyId { get; set; }
        public bool IsPassword { get; set; }
        public bool PasswordSet { get; set; }

        public Nullable<System.DateTime> DOB { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string CorrespondenceAddr { get; set; }
        public string PermanentAddr { get; set; }

        public string PAN { get; set; }
        public string ReportTo { get; set; }
        public Nullable<int> UserTypeId { get; set; }
        public bool IsRegisteredUser { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> RecordDeleted { get; set; }
        public Nullable<bool> IsSuperAdmin { get; set; }
        public Nullable<System.DateTime> ActivationDate { get; set; }
        public Nullable<bool> IsLinkSend { get; set; }
        public string Remarks { get; set; }

     
        public Nullable<int> CandidateId { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public string ReportToFullName { get; set; }

        public string  TotalWorkHours { get; set; }
        public string EmpCode { get; set; }

    }
}