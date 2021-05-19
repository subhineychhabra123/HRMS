using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
    public class ProfileModel
    {
        public int ProfileId { get; set; }
       
        public string ProfileName { get; set; }
        public string Description { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public bool IsDefaultProfile { get; set; }
        public virtual ICollection<EmployeeModel> UserModels { get; set; }
        public virtual EmployeeModel UserModel { get; set; }
        public virtual EmployeeModel UserModel1 { get; set; }
        public virtual CompanyModel CompanyModel { get; set; }
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
