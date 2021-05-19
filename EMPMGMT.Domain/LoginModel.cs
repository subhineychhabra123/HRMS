using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
    public class LoginModel
    {        
        public string EmailId { get; set; }      
        public string Password { get; set; }
        public bool isChecked { get; set; }
    }
}
