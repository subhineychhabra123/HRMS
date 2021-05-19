using EMPMGMT.Business;
using EMPMGMT.Business.Infrastructure;
using EMPMGMT.Business.Interfaces;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EMPMGMT.Web.Infrastructure
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController
            GetControllerInstance(RequestContext requestContext,
            Type controllerType)
        {
            try
            {
                if ((requestContext == null) || (controllerType == null))
                    return null;

                return (Controller)ObjectFactory.GetInstance(controllerType);
            }
            catch (StructureMapException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                System.Diagnostics.Debug.WriteLine(ObjectFactory.GetAllInstances<IController>());
                throw;
                //System.Diagnostics.Debug.WriteLine(ObjectFactory.WhatDoIHave());
                //throw new Exception(ObjectFactory.WhatDoIHave());
            }
        }
    }

    public static class StructureMapper
    {
        public static void Run()
        {
            ControllerBuilder.Current
                .SetControllerFactory(new StructureMapControllerFactory());

            ObjectFactory.Initialize(action =>
            {
                action.AddRegistry(new RepositoryRegistry());
                action.AddRegistry(new BusinessRegistry());

            });
        }
    }
    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            For<IUserBusiness>().Use<UserBusiness>();
            For<IProfileBusiness>().Use<ProfileBusiness>();
            For<IOrganizationUnitBusiness>().Use<OrganizationUnitBusiness>();
            For<IProfilePermissionBusiness>().Use<ProfilePermissionBusiness>();
            For<ICommentsnUnitBusiness>().Use<CommentsUnitBusiness>();
          
            For<IActionItemResponsibleBusiness>().Use<ActionItemResponsibleBusiness>();


            For<ILeavesItemBusinuss>().Use<LeavesItemBusiness>();
            For<IFileAttachmentsBusiness>().Use<FileAttachmentsBusiness>();
            For<IActionListBussiness>().Use<ActionListBussiness>();
            For<IActionItemBusiness>().Use<ActionItemBusiness>();
            For<IActionItemCommentBusiness>().Use<ActionItemCommentBusiness>();
            For<ITechnologyBusiness>().Use<TechnologyBusiness>();
            For<IDesignationBusiness>().Use<DesignationBusiness>();
            For<IReffererBusiness>().Use<ReffererBusiness>();
            For<IProjectBusiness>().Use<ProjectBusiness>();
            For<IResourcesBusiness>().Use<ResourcesBusiness>();
            For<ITimeSheetBusiness>().Use<TimeSheetBusiness>();
            For<ICategoryBusiness>().Use<CategoryBusiness>();
        }
    }
}