using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
   public class ProfilePermissionModel
    {
        public int ProfilePermissionId { get; set; }
        public Nullable<int> ProfileId { get; set; }
        public int ModulePermissionId { get; set; }
        public bool HasAccess { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }

        public ModulePermissionModel ModulePermission { get; set; }

        private ProfileModel _ProfileModel;
        public virtual ProfileModel ProfileModel
        {
            get
            {
                if (this._ProfileModel == null)
                    this._ProfileModel = new ProfileModel();
                return this._ProfileModel;
            }
            set { this._ProfileModel = value; }
        }
        private EmployeeModel _UserModel;
        public virtual EmployeeModel UserModel
        {
            get
            {
                if (this._UserModel == null)
                    this._UserModel = new EmployeeModel();
                return this._UserModel;
            }
            set { this._UserModel = value; }
        }
        private EmployeeModel _UserModel1;
        public virtual EmployeeModel UserModel1
        {
            get
            {
                if (this._UserModel1 == null)
                    this._UserModel1 = new EmployeeModel();
                return this._UserModel1;
            }
            set { this._UserModel1 = value; }
        }
    }
}
