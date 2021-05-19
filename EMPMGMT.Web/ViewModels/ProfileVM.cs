using EMPMGMT.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class ProfileVM
    {
        public string ProfileId { get; set; }
        [Required(ErrorMessage = "Profile Name Required")]
        [Display(Name = "ProfileName")]

        [DataType(DataType.Text)]
        public string ProfileName { get; set; }
        [Required(ErrorMessage = "Profile Description Required")]
        [Display(Name = "Description")]

        [DataType(DataType.Text)]
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public Nullable<bool> IsDefaultForRegisterdUser { get; set; }
        public Nullable<bool> IsDefaultForStaffUser { get; set; }

        private ICollection<ProfilePermissionModel> _ProfilePermissionModels;
        public virtual ICollection<ProfilePermissionModel> ProfilePermissionModels
        {
            get
            {
                if (this._ProfilePermissionModels == null)
                    this._ProfilePermissionModels = new List<ProfilePermissionModel>();
                return this._ProfilePermissionModels;
            }
            set { this._ProfilePermissionModels = value; }
        }
    }
}