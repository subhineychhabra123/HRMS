using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
  public  class DesignationModel
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}
