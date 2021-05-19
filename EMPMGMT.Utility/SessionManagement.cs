using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EMPMGMT.Utility
{
    [Serializable]
    public class SessionManagement
    {
        public static CurrentUser LoggedInUser
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null)
                    return null;
                if (HttpContext.Current.Session["LoggedInUser"] == null)
                {
                    HttpContext.Current.Session["LoggedInUser"] = new CurrentUser();
                }
                return (CurrentUser)HttpContext.Current.Session["LoggedInUser"];
            }
            set { HttpContext.Current.Session["LoggedInUser"] = value; }
        }
    }
}
