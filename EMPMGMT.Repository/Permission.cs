//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EMPMGMT.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Permission
    {
        public Permission()
        {
            this.ModulePermission = new HashSet<ModulePermission>();
        }
    
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionCONSTANT { get; set; }
    
        public virtual ICollection<ModulePermission> ModulePermission { get; set; }
    }
}
