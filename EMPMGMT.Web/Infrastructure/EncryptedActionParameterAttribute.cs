using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EMPMGMT.Utility;
namespace EMPMGMT.Web.Infrastructure
{
    public class EncryptedActionParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            for (int i = 0; i < filterContext.ActionParameters.Count; i++)
            {
                if (filterContext.ActionParameters.Keys.ElementAt(i).ToLower().EndsWith("_encrypted") && filterContext.ActionParameters.Values.ElementAt(i) != null)
                {

                    string EncryptedParamValue = (string)filterContext.ActionParameters.Values.ElementAt(i);
                    string DecryptedParamValue;
                    try
                    {

                        DecryptedParamValue = EncryptedParamValue.Decrypt().ToString();
                        if (Convert.ToInt16(DecryptedParamValue) == -1)
                        {
                            filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                        }
                        filterContext.ActionParameters[filterContext.ActionParameters.Keys.ElementAt(i)] = DecryptedParamValue;
                    }
                    catch (Exception ex)
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }

                }
                //  if(filterContext.ActionParameters[]
            }
            base.OnActionExecuting(filterContext);

        }

    }
}