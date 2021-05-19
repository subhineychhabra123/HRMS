using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
    public class ActionListModel
    {
        public int ActionListId { get; set; }
        public int CompanyId { get; set; }
        public int ResponsibleUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Objective { get; set; }
        public string RiskIssues { get; set; }
        public int Status { get; set; }

        public Nullable<int> ProjectId { get; set; }
      
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public EmployeeModel userModel { get; set; }
        public List<FileAttachmentsModel> FileAttachment { get;set; }
        public string Url { get; set; }
        public string ProjectName { get; set; }

        public string ResponsibleUserName { get; set; }
        public decimal TotalWorkTime { get; set; }

        private ProjectModel _projectModel;
        public ProjectModel ProjectModel
        {
            get
            {
                if (this._projectModel == null)
                    this._projectModel = new ProjectModel();
                return this._projectModel;
            }
            set { this._projectModel = value; }
        }

    }
}
