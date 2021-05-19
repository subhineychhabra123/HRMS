using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMPMGMT.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ErrorController()
        {

        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult Internal()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult SessionTimeOut()
        {
            EMPMGMT.Utility.Response response = new EMPMGMT.Utility.Response();
            response.StatusCode = 401;
            response.Message = "Session Time Out";
            return Json(response, JsonRequestBehavior.AllowGet);

        }
    }
}
