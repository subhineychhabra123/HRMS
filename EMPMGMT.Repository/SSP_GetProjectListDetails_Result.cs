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
    
    public partial class SSP_GetProjectListDetails_Result
    {
        public Nullable<int> TotalCount { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectDescription { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string CommunicationEmailId { get; set; }
        public string CommunicationEmailPassword { get; set; }
        public string SourceControlDetail { get; set; }
        public Nullable<int> Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> RecordDeleted { get; set; }
        public Nullable<int> ProjectLead { get; set; }
        public string ProjectUrl { get; set; }
        public Nullable<decimal> TIMECONSUMED { get; set; }
        public string FULLNAME { get; set; }
    }
}
