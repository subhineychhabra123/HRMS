using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EMPMGMT.Utility;
namespace EMPMGMT.Web.ViewModels
{
    public class OrganizationUnitVM 
    {
        public string OrgUnitId { get; set; }
        [Display(Name = "Unit Name")]
        public string OrgUnitName { get; set; }
        public string ParentOrgUnitId { get; set; }
        public bool IsDefaultForRegisterdUser { get; set; }
        public bool IsDefaultForStaffUser { get; set; }
        public int CompanyId { get; set; }
        public bool IsDefaultUnit { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public Boolean HasForecast { get; set; }
       
        public string MdMetricResponsibleId { get; set; }
        public string OrgUnitNameListingText { get; set; }
    }
}