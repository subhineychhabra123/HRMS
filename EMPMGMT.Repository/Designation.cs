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
    
    public partial class Designation
    {
        public Designation()
        {
            this.Employee = new HashSet<Employee>();
        }
    
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public Nullable<int> CompanyId { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
    }
}
