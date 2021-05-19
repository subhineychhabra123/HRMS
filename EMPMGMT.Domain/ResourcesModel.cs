using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
   public class ResourcesModel
    {
        public int ResourceId { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> UserId { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public Nullable<int> ProjectLead { get; set; }
        public virtual EmployeeModel Employee { get; set; }
        public string FullName { get; set; }

        private EmployeeModel _employeeModel;
        //public EmployeeModel EmployeeModel
        //{
        //    get
        //    {
        //        if (this._employeeModel == null)
        //            this._employeeModel = new EmployeeModel();
        //        return this._employeeModel;
        //    }
        //    set { this._employeeModel = value; }
        //}


       
    }
}
