using EMPMGMT.Repository;
using EMPMGMT.Domain;
using EMPMGMT.Repository;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business.Infrastructure
{
    public class AutoMapperBusinessProfile : AutoMapper.Profile
    {
        public static void Run()
        {

            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperBusinessProfile>();
            });
        }
        protected override void Configure()
        {
            base.Configure();


            #region Mapping From Model to DomainModel

            
            AutoMapper.Mapper.CreateMap<ActionItemComment, ActionItemCommentModel>();
            AutoMapper.Mapper.CreateMap<ActionItemResponsible, ActionItemResponsibleModel>()
                .ForMember(dest => dest.userModel, opt => opt.MapFrom(src => src.Employee))
                .ForMember(dest => dest.ActionItemModel, opt => opt.MapFrom(src => src.ActionItem));


            AutoMapper.Mapper.CreateMap<FileAttachments, FileAttachmentsModel>()
                       .ForMember(dest => dest.UserModel, opt => opt.MapFrom(src => src.Employee));


            AutoMapper.Mapper.CreateMap<Company, CompanyModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>().
                ForMember(dest => dest.ProfileModel, opt => opt.MapFrom(src => src.Profile2))
              .ForMember(dest => dest.CompanyModel, opt => opt.MapFrom(src => src.Company))
              .ForMember(dest => dest.reffererModel, opt => opt.MapFrom(src => src.Referrer));

            AutoMapper.Mapper.CreateMap<LeavesItemModel, Leaves>();
            AutoMapper.Mapper.CreateMap< Leaves,LeavesItemModel >().
             ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Employee.FirstName));
            AutoMapper.Mapper.CreateMap<Module, ModuleModel>();
            AutoMapper.Mapper.CreateMap<Permission, PermissionModel>();
            AutoMapper.Mapper.CreateMap<ModulePermission, ModulePermissionModel>();
            AutoMapper.Mapper.CreateMap<ProfilePermission, ProfilePermissionModel>();
            AutoMapper.Mapper.CreateMap<Profile, ProfileModel>();
            AutoMapper.Mapper.CreateMap<OrganizationUnit, OrganizationUnitModel>();

            AutoMapper.Mapper.CreateMap<Comments, CommentsModel>();
            AutoMapper.Mapper.CreateMap<LeaveType, LeaveTypeModel>();
            AutoMapper.Mapper.CreateMap<ActionItem, ActionItemModel>();


            AutoMapper.Mapper.CreateMap<ActionList, ActionListModel>()
                   .ForMember(dest => dest.userModel, opt => opt.MapFrom(src => src.Employee))
                   .ForMember(dest => dest.ProjectModel, opt => opt.MapFrom(src => src.Project))

                   .ForMember(dest => dest.FileAttachment, opt => opt.MapFrom(src => src.FileAttachments));


            //AutoMapper.Mapper.CreateMap<SSP_GetActionItemsOfActionList_Result, ActionItemModel>()
            //.ForMember(dest => dest.ResponsibleUserName, opt => opt.MapFrom(src => CommonFunctions.ConcatenateStrings(src.FirstName, src.LastName)));

            //AutoMapper.Mapper.CreateMap<SSP_GetActionItemsOfRCA_Result, ActionItemModel>()
            //     .ForMember(dest => dest.ResponsibleUserName, opt => opt.MapFrom(src => CommonFunctions.ConcatenateStrings(src.FirstName, src.LastName)));

            AutoMapper.Mapper.CreateMap<SSP_GetActionItemDescription_Result, ActionItemModel>();


            AutoMapper.Mapper.CreateMap<SSP_GetActionItemsFromActionListId_Result, ActionItemModel>()
                                  .ForMember(dest => dest.ResponsibleUserName, opt => opt.MapFrom(src => CommonFunctions.ConcatenateStrings(src.FirstName, src.LastName)));
            AutoMapper.Mapper.CreateMap<Technology, TechnologyModel>();
            AutoMapper.Mapper.CreateMap<Designation, DesignationModel>();
            AutoMapper.Mapper.CreateMap<Referrer, ReffererModel>();
            AutoMapper.Mapper.CreateMap<Project, ProjectModel>()
            .ForMember(dest => dest.EmployeeModel, opt => opt.MapFrom(src => src.Employee));

            AutoMapper.Mapper.CreateMap<Resources, ResourcesModel>();
            AutoMapper.Mapper.CreateMap<TimeSheet, TimeSheetModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetMonthYearTimeSheet_Result, TimeSheetModel>();
            AutoMapper.Mapper.CreateMap<ssp_GetMonthTimeSheet_Result, DateTimeSheetModel>();
            AutoMapper.Mapper.CreateMap<ssp_GetActionItemDateTimeSheet_Result, DailyActionItemsModel>()
                .ForMember(dest => dest.WorkHours, opt => opt.MapFrom(src => src.TimeTaken));
            AutoMapper.Mapper.CreateMap<Category, CategoryModel>();


            AutoMapper.Mapper.CreateMap<SSP_GetEmployeeWorkHours_Result, EmployeeModel>();

            AutoMapper.Mapper.CreateMap<SSP_GetActionItemsforDropdwonforProjectId_Result, ActionItemModel>();

            AutoMapper.Mapper.CreateMap<SSP_GetProjectListDetails_Result, ProjectModel>()
         .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FULLNAME));
            AutoMapper.Mapper.CreateMap<SSP_GetActionListDetails_Result, ActionListModel>()
         .ForMember(dest => dest.ResponsibleUserName, opt => opt.MapFrom(src => src.ResponsibleUserName));
            

            #endregion


            #region Mapping From DomainModel to Model



            AutoMapper.Mapper.CreateMap<ActionItemModel, ActionItem>()
                    .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId));


            AutoMapper.Mapper.CreateMap<ActionItemCommentModel, ActionItemComment>();

            AutoMapper.Mapper.CreateMap<ActionItemResponsibleModel, ActionItemResponsible>()
           .ForMember(dest => dest.ActionItemResponsibleId, opt => opt.MapFrom(src => src.ActionItemResponsibleId > 0 ? src.ActionItemResponsibleId : (int?)null));
            AutoMapper.Mapper.CreateMap<FileAttachmentsModel, FileAttachments>()
                   .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId > 0 ? src.ActionItemId : (int?)null))
                    .ForMember(dest => dest.ActionListId, opt => opt.MapFrom(src => src.ActionListId > 0 ? src.ActionListId : (int?)null))
                      .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId > 0 ? src.UserId : (int?)null));


            AutoMapper.Mapper.CreateMap<LeaveTypeModel, LeaveTypeVM>();
            AutoMapper.Mapper.CreateMap<CompanyModel, Company>();
            AutoMapper.Mapper.CreateMap<EmployeeModel, Employee>()
                //.ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyModel))
              .ForMember(dest => dest.OrgUnitId, opt => opt.MapFrom(src => src.OrgUnitId > 0 ? src.OrgUnitId : (int?)null))
                 .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId > 0 ? src.ProfileId : (int?)null))
                 .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId > 0 ? src.CompanyId : (int?)null))
             .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
               .ForMember(dest => dest.LinkSendDate, opt => opt.Ignore())
                   .ForMember(dest => dest.IsLinkSend, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
             .ForMember(dest => dest.Password, opt => opt.Ignore())
               .ForMember(dest => dest.Status, opt => opt.Ignore())
               .ForMember(dest => dest.ImageURL, opt => opt.Ignore())
               .ForMember(dest => dest.OrgUnitId, opt => opt.Ignore())
                 .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                   .ForMember(dest => dest.ImageURL, opt => opt.Ignore())
                  .AfterMap((s, d) =>
                  {
                      if (s.CompanyModel != null && s.CompanyModel.CompanyName != null)
                          d.Company = AutoMapper.Mapper.Map<CompanyModel, Company>(s.CompanyModel);
                      if (s.CreatedBy != 0)
                          d.CreatedBy = s.CreatedBy;

                      if (s.Status > 0)
                          d.Status = s.Status;

                      if (s.Password != null)
                          d.Password = s.Password;

                      if (s.CompanyId != 0)
                          d.CompanyId = s.CompanyId;

                      if (s.CreatedDate != null)
                          d.CreatedDate = s.CreatedDate;

                      if (s.OrgUnitId > 0)
                      {
                          d.OrgUnitId = s.OrgUnitId;
                      }
                      else
                      {
                          d.OrgUnitId = null;
                      }



                  });

            AutoMapper.Mapper.CreateMap<ModuleModel, Module>()
               .ForMember(dest => dest.ModulePermission, opt => opt.MapFrom(src => src.ModulePermissionModels))
               .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.UserModel))
               .ForMember(dest => dest.Employee1, opt => opt.MapFrom(src => src.UserModel1));

            AutoMapper.Mapper.CreateMap<PermissionModel, Permission>()
               .ForMember(dest => dest.ModulePermission, opt => opt.MapFrom(src => src.ModulePermissionModels));

            AutoMapper.Mapper.CreateMap<ModulePermissionModel, ModulePermission>()
               .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module))
               .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.Permission));

            AutoMapper.Mapper.CreateMap<ProfilePermissionModel, ProfilePermission>()
               .ForMember(dest => dest.ModulePermission, opt => opt.MapFrom(src => src.ModulePermission))
               .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.ProfileModel))
               .ForMember(dest => dest.Employee1, opt => opt.MapFrom(src => src.UserModel1))
               .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.UserModel));

            AutoMapper.Mapper.CreateMap<OrganizationUnitModel, OrganizationUnit>();

            AutoMapper.Mapper.CreateMap<ProfileModel, Profile>();
            AutoMapper.Mapper.CreateMap<CommentsModel, Comments>();


            AutoMapper.Mapper.CreateMap<CategoryModel, Category>();




            AutoMapper.Mapper.CreateMap<ActionListModel, ActionList>()
               .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
               .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                 .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
                 .AfterMap((s, d) =>
                 {
                     if (s.CreatedBy != 0)
                         d.CreatedBy = s.CreatedBy;
                     if (s.ModifiedBy != 0)
                         d.ModifiedBy = s.ModifiedBy;
                 });



            AutoMapper.Mapper.CreateMap<ActionItemModel, SSP_GetActionItemsFromActionListId_Result>();
            AutoMapper.Mapper.CreateMap<TechnologyModel, Technology>();
            AutoMapper.Mapper.CreateMap<DesignationModel, Designation>();
            AutoMapper.Mapper.CreateMap<ReffererModel, Referrer>();
            AutoMapper.Mapper.CreateMap<ProjectModel, Project>()
            .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.EmployeeModel));
            AutoMapper.Mapper.CreateMap<ResourcesModel, Resources>();
            AutoMapper.Mapper.CreateMap<TimeSheetModel, TimeSheet>();
            AutoMapper.Mapper.CreateMap<LeaveTypeModel,LeaveType >();
            //  AutoMapper.Mapper.CreateMap<TimeSheetModel, SSP_GetMonthYearTimeSheet_Result>();
            //  AutoMapper.Mapper.CreateMap<DateTimeSheetModel, ssp_GetMonthTimeSheet_Result>();

            #endregion
        }
    }
}
