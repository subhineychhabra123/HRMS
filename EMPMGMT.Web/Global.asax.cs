using EMPMGMT.Web;
using EMPMGMT.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;


namespace EMPMGMT.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            StructureMapper.Run();
            AutoMapperWebProfile.Run();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteTable.Routes.Add(new SubdomainRoute());
//            RouteTable.Routes.Add("DomainRoute", new DomainRoute(
//    "{language}.localhost.com",    
//    "{controller}/{action}/{id}",   
//    new { tenant = "tenant", controller = "Site", action = "Index", id = "" }  
//));
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (System.Web.HttpContext.Current.Session != null)
            {
                if (EMPMGMT.Utility.SessionManagement.LoggedInUser == null || EMPMGMT.Utility.SessionManagement.LoggedInUser.UserId == 0)
                {
                    string[] groups = null;
                    System.Security.Principal.GenericIdentity id = new System.Security.Principal.GenericIdentity(string.Empty, "RIAuthentication");
                    System.Security.Principal.GenericPrincipal principal = new System.Security.Principal.GenericPrincipal(id, groups);
                    Context.User = principal;
                }
               

            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // code to change culture
        }

        protected void Application_AuthenticateRequest(object sender, System.EventArgs e)
        {
            string cookieName = System.Web.Security.FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                System.Web.Security.FormsAuthenticationTicket authTicket = null;
                authTicket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    string[] groups = authTicket.UserData.Split('|');
                    System.Security.Principal.GenericIdentity id = new System.Security.Principal.GenericIdentity(authTicket.Name, "RIAuthentication");
                    System.Security.Principal.GenericPrincipal principal = new System.Security.Principal.GenericPrincipal(id, groups);
                    Context.User = principal;

                }
            }
        }
    }
}