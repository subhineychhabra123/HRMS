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
    
    public partial class LeavesApprovedBy
    {
        public int Id { get; set; }
        public Nullable<int> LeaveId { get; set; }
        public Nullable<int> EmpId { get; set; }
    
        public virtual Leaves Leaves { get; set; }
        public virtual Employee Employee { get; set; }
    }
}