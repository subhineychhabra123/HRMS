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
    
    public partial class ClientDetail
    {
        public ClientDetail()
        {
            this.ProjectClientDetail = new HashSet<ProjectClientDetail>();
        }
    
        public int ClientDetailId { get; set; }
        public string ClientName { get; set; }
        public string Emailid { get; set; }
        public string ClientDescription { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    
        public virtual ICollection<ProjectClientDetail> ProjectClientDetail { get; set; }
    }
}
