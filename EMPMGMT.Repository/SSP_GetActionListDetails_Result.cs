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
    
    public partial class SSP_GetActionListDetails_Result
    {
        public Nullable<int> TotalCount { get; set; }
        public int ActionListId { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Objective { get; set; }
        public string RiskIssues { get; set; }
        public Nullable<int> ResponsibleUserId { get; set; }
        public Nullable<int> Status { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string ResponsibleUserName { get; set; }
        public Nullable<decimal> TotalWorkTime { get; set; }
    }
}
