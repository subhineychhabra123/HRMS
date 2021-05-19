using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
    public class ModuleModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCONSTANT { get; set; }
        public Nullable<int> ParentModuleId { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public string RenderType { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }

        public virtual ICollection<ModuleModel> ModuleModel1 { get; set; }
        public virtual ModuleModel ModuleModel2 { get; set; }

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

        private ICollection<ModulePermissionModel> _ModulePermissionModels;
        public virtual ICollection<ModulePermissionModel> ModulePermissionModels
        {
            get
            {
                if (this._ModulePermissionModels == null)
                    this._ModulePermissionModels = new List<ModulePermissionModel>();
                return this._ModulePermissionModels;
            }
            set { this._ModulePermissionModels = value; }
        }       
    }
}
