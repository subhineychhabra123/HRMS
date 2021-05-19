using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EMPMGMT.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: "MDRoute", url: "Employee/RootCauseAction/{Id_encrypted}/{type}", defaults: new { controller = "Employee", action = "RootCauseAction", id_encrypted = UrlParameter.Optional, type = UrlParameter.Optional });

            routes.MapRoute(name: "UserRoutes", url: "Employee/{action}/{Id_encrypted}/{ProjectId}", defaults: new { controller = "Employee", action = "Home", id_encrypted = UrlParameter.Optional, ProjectId= UrlParameter.Optional });


            routes.MapRoute(name: "ProjectRoutes", url: "Project/{action}/{Id_encrypted}/{ProjectId}", defaults: new { controller = "Employee", action = "ProjectList", id_encrypted = UrlParameter.Optional, ProjectId = UrlParameter.Optional });

            routes.MapRoute(name: "ProjectDetailRoutes", url: "Project/{action}/{Id_encrypted}/{ProjectId}", defaults: new { controller = "Employee", action = "ProjectDetail", id_encrypted = UrlParameter.Optional, ProjectId = UrlParameter.Optional });




            routes.MapRoute(name: "EditActionListRoutes", url: "Project/{action}/{Id_encrypted}/{ProjectId}", defaults: new { controller = "Employee", action = "EditActionList", id_encrypted = UrlParameter.Optional, ProjectId = UrlParameter.Optional });


            routes.MapRoute(name: "OrganizationUnitsRoutes", url: "Organization/{action}/{Id_encrypted}", defaults: new { controller = "Employee", action = "OrganizationUnits", id_encrypted = UrlParameter.Optional });


            routes.MapRoute(name: "ManageCategoryRoutes", url: "Category/{action}/{Id_encrypted}", defaults: new { controller = "Employee", action = "ManageCategory", id_encrypted = UrlParameter.Optional });

            routes.MapRoute(name: "ProfilesRoutes", url: "Permission/{action}/{Id_encrypted}", defaults: new { controller = "Employee", action = "Profiles", id_encrypted = UrlParameter.Optional });

            routes.MapRoute(name: "TimeRoutes", url: "Project/{action}/{Id_encrypted}", defaults: new { controller = "Employee", action = "Timesheet", id_encrypted = UrlParameter.Optional });

            //routes.MapRoute(name: "UserTimeRoutes", url: "Project/{action}/{Id_encrypted}", defaults: new { controller = "Employee", action = "UserTimesheet", id_encrypted = UrlParameter.Optional });









            routes.MapRoute(name: "ManageActionListRoutes", url: "Project/{action}/{Id_encrypted}/{ProjectId}", defaults: new { controller = "Employee", action = "ManageActionList", id_encrypted = UrlParameter.Optional, ProjectId = UrlParameter.Optional });

            

            routes.MapRoute(name: "ActionItemRoutes", url: "Project/{action}/{Id_encrypted}", defaults: new { controller = "Employee", action = "ManageActionItem", id_encrypted = UrlParameter.Optional });


            routes.MapRoute(name: "AdminRoutes", url: "admin/{action}/{Id_encrypted}", defaults: new { controller = "Admin", action = "Index", id_encrypted = UrlParameter.Optional });
            //routes.MapRoute(
            //       name: "Default",
            //       url: "{controller}/{action}/{*id}",
            //       defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            //   );

            routes.MapRoute("SiteRouteIndex", "Home", new { controller = "Site", action = "Index" });
            routes.MapRoute("SiteRouteFeatures", "Features", new { controller = "Site", action = "Features" });
            routes.MapRoute("SiteRouteContactUs", "Contactus", new { controller = "Site", action = "Contactus" });
            routes.MapRoute("SiteRouteLogin", "Login", new { controller = "Site", action = "Login" });
            routes.MapRoute("SiteRouteForgotPassword", "ForgotPassword", new { controller = "Site", action = "ForgotPassword" });
            routes.MapRoute("SiteRouteRegister", "Register", new { controller = "Site", action = "Register" });
            routes.MapRoute("SiteRouteResetPassword", "ResetPassword", new { controller = "Site", action = "ResetPassword" });
            routes.MapRoute(name: "SiteRoutes1", url: "{action}", defaults: new { controller = "Site", action = "Login" });
            routes.MapRoute(name: "Default", url: "{controller}/{action}", defaults: new { controller = "Site", action = "Login" });
        }
    }
}