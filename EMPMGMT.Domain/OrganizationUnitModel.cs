using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
    public class OrganizationUnitModel
    {
        public int OrgUnitId { get; set; }
        public string OrgUnitName { get; set; }
        public Nullable<int> ParentOrgUnitId { get; set; }
        public bool IsDefaultUnit { get; set; }
        public bool IsDefaultForRegisterdUser { get; set; }
        public bool IsDefaultForStaffUser { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public Boolean HasForecast { get; set; }
       
        public int MdMetricResponsibleId { get; set; }
        public string OrgUnitNameListingText { get; set; }

    }
}
