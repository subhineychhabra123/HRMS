using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
    public class CompanyModel
    {
        public int CompanyId
        {
            get;
            set;
        }
        public string CompanyName
        {
            get;
            set;
        }
        public string BreakThroughObjectiveYear
        {
            get;
            set;
        }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
