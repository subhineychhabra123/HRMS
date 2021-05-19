using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
    public  class ModulePermissionModel
    {
        public int ModulePermissionId { get; set; }
        public int ModuleId { get; set; }
        public int PermissionId { get; set; }
        public bool IsReadOnly { get; set; }

        private PermissionModel _Permission;
        public virtual PermissionModel Permission
        {
            get
            {
                if (this._Permission == null)
                    this._Permission = new PermissionModel();
                return this._Permission;
            }
            set { this._Permission = value; }
        }

        private ICollection<ProfilePermissionModel> _ProfilePermissionModels;
        public virtual ICollection<ProfilePermissionModel> TaskModels1
        {
            get
            {
                if (this._ProfilePermissionModels == null)
                    this._ProfilePermissionModels = new List<ProfilePermissionModel>();
                return this._ProfilePermissionModels;
            }
            set { this._ProfilePermissionModels = value; }
        }

        private ModuleModel _Module;
        public virtual ModuleModel Module
        {
            get
            {
                if (this._Module == null)
                    this._Module = new ModuleModel();
                return this._Module;
            }
            set { this._Module = value; }
        }
    }
}
