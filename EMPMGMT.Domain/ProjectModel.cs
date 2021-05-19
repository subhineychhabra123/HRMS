using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
  public class ProjectModel
    {
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
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<bool> RecordDeleted { get; set; }
        public Nullable<int> ProjectLead { get; set; }
        public string FullName { get; set; }
        public string GblprojectLead { get; set; }
        public bool ErrorProjectName { get; set; }
        public bool ErrorProjectCode { get; set; }
        public string ProjectUrl { get; set; }

        public Nullable<decimal> TIMECONSUMED { get; set; }

        private EmployeeModel _employeeModel;
        public EmployeeModel EmployeeModel
        {
            get
            {
                if (this._employeeModel == null)
                    this._employeeModel = new EmployeeModel();
                return this._employeeModel;
            }
            set { this._employeeModel = value; }
        }
        public virtual ICollection<ResourcesModel> Resources { get; set; }
    }
}
