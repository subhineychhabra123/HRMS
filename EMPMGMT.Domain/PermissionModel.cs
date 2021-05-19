using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
   public class PermissionModel
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionCONSTANT { get; set; }

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
