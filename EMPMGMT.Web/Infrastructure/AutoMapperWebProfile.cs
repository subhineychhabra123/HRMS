using EMPMGMT.Business.Infrastructure;
using EMPMGMT.Domain;
using EMPMGMT.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMPMGMT.Utility;
namespace EMPMGMT.Web.Infrastructure
{
    public class AutoMapperWebProfile : AutoMapper.Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperWebProfile>();
                a.AddProfile<AutoMapperBusinessProfile>();
            });
        }
        protected override void Configure()
        {
            base.Configure();

            #region Mapping From ViewModel to DomainModel



            AutoMapper.Mapper.CreateMap<ActionItemResponsibleVM, ActionItemResponsibleModel>()
              .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId.Decrypt()))
               .ForMember(dest => dest.ResponsibleUserId, opt => opt.MapFrom(src => src.ResponsibleUserId.Decrypt()))
                .ForMember(dest => dest.ActionItemResponsibleId, opt => opt.MapFrom(src => src.ActionItemResponsibleId != null ? src.ActionItemResponsibleId.Decrypt() : Convert.ToInt32(src.ActionItemResponsibleId)));
            //    .ForMember(dest => dest.RCAId, opt => opt.MapFrom(src => src.RCAId.Decrypt()))
            //.ForMember(dest => dest.ParentActionItemId, opt => opt.MapFrom(src => src.ParentActionItemId.Decrypt()))
            //   .ForMember(dest => dest.ActionListId, opt => opt.MapFrom(src => src.ActionListId.Decrypt()));

            AutoMapper.Mapper.CreateMap<ActionItemVM, ActionItemModel>()
            .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId.Decrypt()))
             .ForMember(dest => dest.ResponsibleUserId, opt => opt.MapFrom(src => src.ResponsibleUserId.Decrypt()))
             .ForMember(dest => dest.RCAId, opt => opt.MapFrom(src => src.RCAId != null ? src.RCAId : null))
                 .ForMember(dest => dest.RCAId, opt => opt.MapFrom(src => src.RCAId.Decrypt()))
             .ForMember(dest => dest.ParentActionItemId, opt => opt.MapFrom(src => src.ParentActionItemId.Decrypt()))
                .ForMember(dest => dest.ActionListId, opt => opt.MapFrom(src => src.ActionListId.Decrypt()))
                  .ForMember(dest => dest.ActionListId, opt => opt.MapFrom(src => src.ActionListId.Decrypt()));



            AutoMapper.Mapper.CreateMap<ActionItemCommentVM, ActionItemCommentModel>()
           .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId.Decrypt()))
            .ForMember(dest => dest.ActionItemCommentId, opt => opt.MapFrom(src => src.ActionItemCommentId.Decrypt()));



            AutoMapper.Mapper.CreateMap<FileAttachmentsVM, FileAttachmentsModel>()
               .ForMember(dest => dest.ActionListId, opt => opt.MapFrom(src => src.ActionListId.Decrypt()))
                 .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId.Decrypt()))
                 .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.DocumentId.Decrypt()))
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Decrypt()));


            AutoMapper.Mapper.CreateMap<CategoryVM, CategoryModel>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId.Decrypt()));
            AutoMapper.Mapper.CreateMap<CompanyVM, CompanyModel>();
            AutoMapper.Mapper.CreateMap<EmployeeVM, EmployeeModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Decrypt()))
            .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId.Decrypt()))
                .ForMember(dest => dest.OrgUnitId, opt => opt.MapFrom(src => src.OrgUnitId.Decrypt()))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId.Decrypt()))
            .ForMember(dest => dest.ReferrerId, opt => opt.MapFrom(src => src.ReferrerId.Decrypt()))
            .ForMember(dest => dest.TechnologyId, opt => opt.MapFrom(src => src.TechnologyId.Decrypt()))
           .ForMember(dest => dest.ReportTo, opt => opt.MapFrom(src => src.ReportTo.Decrypt()))
            .ForMember(dest => dest.DesignationId, opt => opt.MapFrom(src => src.DesignationId.Decrypt()));

            AutoMapper.Mapper.CreateMap<RegistrationVM, EmployeeModel>()
                 .ForMember(dest => dest.CompanyModel, opt => opt.MapFrom(src => src.Company));
            AutoMapper.Mapper.CreateMap<OrganizationUnitVM, OrganizationUnitModel>()
                 .ForMember(dest => dest.OrgUnitId, opt => opt.MapFrom(src => src.OrgUnitId != "0" ? src.OrgUnitId.Decrypt() : 0))
                   .ForMember(dest => dest.ParentOrgUnitId, opt => opt.MapFrom(src => src.ParentOrgUnitId.Decrypt()));
            AutoMapper.Mapper.CreateMap<ProfileVM, ProfileModel>()
                  .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId.Decrypt()));

            AutoMapper.Mapper.CreateMap<CommentsVM, CommentsModel>()
                   .ForMember(dest => dest.CommentTo, opt => opt.MapFrom(src => src.CommentTo.Decrypt()));

            AutoMapper.Mapper.CreateMap<ActionListVM, ActionListModel>()
              .ForMember(dest => dest.ActionListId, opt => opt.MapFrom(src => src.ActionListId.Decrypt()))
             .ForMember(dest => dest.ResponsibleUserId, opt => opt.MapFrom(src => src.ResponsibleUserId.Decrypt()))
               .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.Decrypt()));


            AutoMapper.Mapper.CreateMap<TechnologyVM, TechnologyModel>()
              .ForMember(dest => dest.TechnologyId, opt => opt.MapFrom(src => src.TechnologyId.Decrypt()));

            AutoMapper.Mapper.CreateMap<DesignationVM, DesignationModel>()
                    .ForMember(dest => dest.DesignationId, opt => opt.MapFrom(src => src.DesignationId.Decrypt()));
            AutoMapper.Mapper.CreateMap<RefferrerVM, ReffererModel>()
                    .ForMember(dest => dest.ReferrerId, opt => opt.MapFrom(src => src.ReferrerId.Decrypt()));
            AutoMapper.Mapper.CreateMap<ProjectVM, ProjectModel>()
                  .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.Decrypt()))
                   .ForMember(dest => dest.ProjectLead, opt => opt.MapFrom(src => src.ProjectLead.Decrypt()));

            AutoMapper.Mapper.CreateMap<ResourcesVM, ResourcesModel>()
              .ForMember(dest => dest.ResourceId, opt => opt.MapFrom(src => src.ResourceId.Decrypt()))
             .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.Decrypt()))
                //.ForMember(dest => dest.Employee.FullName, opt => opt.MapFrom(src => src.FullName))
             .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Decrypt()));


            AutoMapper.Mapper.CreateMap<TimeSheetVM, TimeSheetModel>()
           .ForMember(dest => dest.TimeSheetId, opt => opt.MapFrom(src => src.TimeSheetId.Decrypt()))
           .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.Decrypt()))
             .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId.Decrypt()))
           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Decrypt()));


            AutoMapper.Mapper.CreateMap<DateTimeSheetVM, DateTimeSheetModel>();
            AutoMapper.Mapper.CreateMap<DailyActionItemVM, DailyActionItemsModel>()
              .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.Decrypt()))
             .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId.Decrypt()))
             .ForMember(dest => dest.TimeSheetId, opt => opt.MapFrom(src => src.TimeSheetId.Decrypt()));

            AutoMapper.Mapper.CreateMap<LeavesItemVM, LeavesItemModel>();

            #endregion


            #region Mapping From DomainModel to ViewModel




            AutoMapper.Mapper.CreateMap<ActionItemResponsibleModel, ActionItemResponsibleVM>()
              .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId.Encrypt()))
              .ForMember(dest => dest.ActionItemResponsibleId, opt => opt.MapFrom(src => src.ActionItemResponsibleId.Encrypt()))
               .ForMember(dest => dest.ResponsibleUserId, opt => opt.MapFrom(src => src.ResponsibleUserId.Encrypt()))
               .ForMember(dest => dest.ActionItemName, opt => opt.MapFrom(src => src.ActionItemModel.ItemName))
               .ForMember(dest => dest.StatusDrop, opt => opt.MapFrom(src => src.ActionItemModel.StatusDrop))

             .ForMember(dest => dest.ResponsibleUserName, opt => opt.MapFrom(src => src.userModel.FullName));





            AutoMapper.Mapper.CreateMap<ActionItemModel, ActionItemVM>()
     .ForMember(dest => dest.ObjectId, opt => opt.MapFrom(src => src.ActionItemId.Encrypt()))
     .ForMember(dest => dest.ParentObjectId, opt => opt.MapFrom(src => src.ParentActionItemId > 0 ? src.ParentActionItemId.Encrypt() : null))
       .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId.Encrypt()))
      .ForMember(dest => dest.ActionListId, opt => opt.MapFrom(src => src.ActionListId.Encrypt()))
        .ForMember(dest => dest.RCAId, opt => opt.MapFrom(src => src.RCAId.HasValue ? src.RCAId.Value.Encrypt() : null))
                //.ForMember(dest => dest.RCAId, opt => opt.MapFrom(src => src.RCAId.Encrypt()))
       .ForMember(dest => dest.ResponsibleUserId, opt => opt.MapFrom(src => src.ResponsibleUserId.Encrypt()))
     .ForMember(dest => dest.ParentActionItemId, opt => opt.MapFrom(src => src.ParentActionItemId.Encrypt()))
     .ForMember(dest => dest.ResponsibleUserName, opt => opt.MapFrom(src => src.userModel != null ? src.userModel.FullName : src.ResponsibleUserName));
            //.ForMember(dest => dest.IsSendEmailNotification, opt => opt.MapFrom(src => src.IsSendEmailNotification == true ? Enums.BoolStatus.Yes.ToString() : Enums.BoolStatus.No.ToString() ))

            //.ForMember(dest => dest.IsArchived, opt => opt.MapFrom(src => src.IsArchived == true ? Enums.BoolStatus.Yes.ToString() : Enums.BoolStatus.No.ToString()));
            AutoMapper.Mapper.CreateMap<ActionItemCommentModel, ActionItemCommentVM>()
              .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId.Encrypt()))
                .ForMember(dest => dest.ActionItemCommentId, opt => opt.MapFrom(src => src.ActionItemCommentId.Encrypt()));





            AutoMapper.Mapper.CreateMap<FileAttachmentsModel, FileAttachmentsVM>()
                .ForMember(dest => dest.ActionListId, opt => opt.MapFrom(src => src.ActionListId.Encrypt()))
                .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => Convert.ToInt32(src.ActionItemId).Encrypt()))
                    .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.DocumentId.Encrypt()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Convert.ToInt32(src.UserId).Encrypt()));



            AutoMapper.Mapper.CreateMap<CategoryModel, CategoryVM>()
                  .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId.Encrypt()));
            AutoMapper.Mapper.CreateMap<CompanyModel, CompanyVM>();
            AutoMapper.Mapper.CreateMap<EmployeeModel, RegistrationVM>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Encrypt()));


            AutoMapper.Mapper.CreateMap<EmployeeModel, EmployeeVM>()
                 .ForMember(dest => dest.Password, opt => opt.Ignore())
                 .ForMember(dest => dest.PasswordSet, opt => opt.MapFrom(src => src.Password == null ? false : true))
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Encrypt()))
                   .ForMember(dest => dest.OrgUnitId, opt => opt.MapFrom(src => src.OrgUnitId.Encrypt()))
                     .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId.Encrypt()))
                      .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId.Encrypt()))
                .ForMember(dest => dest.ProfileName, opt => opt.MapFrom(src => src.ProfileModel.ProfileName))
                .ForMember(dest => dest.OrgUnitName, opt => opt.MapFrom(src => src.organizationUnitModel.OrgUnitName))
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL != null ? src.ImageURL : "no_image.gif"))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyModel.CompanyName))
                  .ForMember(dest => dest.ReferrerId, opt => opt.MapFrom(src => src.reffererModel.ReferrerId.Encrypt()))
                   .ForMember(dest => dest.ReferrerName, opt => opt.MapFrom(src => src.reffererModel.ReferrerName))
                   .ForMember(dest => dest.TechnologyId, opt => opt.MapFrom(src => src.technologyModel.TechnologyId.Encrypt()))
                    .ForMember(dest => dest.TechnologyName, opt => opt.MapFrom(src => src.technologyModel.TechnologyName))
                    .ForMember(dest => dest.DesignationId, opt => opt.MapFrom(src => src.designationModel.DesignationId.Encrypt()))
                    .ForMember(dest => dest.DesignationName, opt => opt.MapFrom(src => src.designationModel.DesignationName))
                    .ForMember(dest => dest.ReportTo, opt => opt.MapFrom(src => src.ReportTo.HasValue ? src.ReportTo.Value.Encrypt() : null));




            AutoMapper.Mapper.CreateMap<OrganizationUnitModel, OrganizationUnitVM>()

                .ForMember(dest => dest.OrgUnitId, opt => opt.MapFrom(src => src.OrgUnitId.Encrypt()))
                .ForMember(dest => dest.ParentOrgUnitId, opt => opt.MapFrom(src => src.ParentOrgUnitId.HasValue ? src.ParentOrgUnitId.Value.Encrypt() : null))
                 .ForMember(dest => dest.MdMetricResponsibleId, opt => opt.MapFrom(src => src.MdMetricResponsibleId.Encrypt()));

            AutoMapper.Mapper.CreateMap<ProfileModel, ProfileVM>()
                .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId.Encrypt()));

            AutoMapper.Mapper.CreateMap<CommentsModel, CommentsVM>()
                  .ForMember(dest => dest.CommentTo, opt => opt.MapFrom(src => src.CommentTo.Encrypt()));





            AutoMapper.Mapper.CreateMap<ActionListModel, ActionListVM>()
                 .ForMember(dest => dest.ActionListId, opt => opt.MapFrom(src => src.ActionListId.Encrypt()))
                 .ForMember(dest => dest.ResponsibleUserId, opt => opt.MapFrom(src => src.ResponsibleUserId.Encrypt()))
                 .ForMember(dest => dest.ResponsibleUserName, opt => opt.MapFrom(src => src.ResponsibleUserName == "" || src.ResponsibleUserName == null ? src.userModel.FullName:src.ResponsibleUserName))                   
                 .ForMember(dest => dest.StatusImagePath, opt => opt.MapFrom(src => src.Status == 1 ? Constants.CONTENT_IMAGES_PATH + "bullet-red.png" : src.Status == 2 ? Constants.CONTENT_IMAGES_PATH + "bullet-yellow.png" : Constants.CONTENT_IMAGES_PATH + "bullet-green.png"))

                    .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.Value.Encrypt()))
             .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.ProjectModel.ProjectName));


            AutoMapper.Mapper.CreateMap<TechnologyModel, TechnologyVM>()
             .ForMember(dest => dest.TechnologyId, opt => opt.MapFrom(src => src.TechnologyId.Encrypt()));

            AutoMapper.Mapper.CreateMap<DesignationModel, DesignationVM>()
            .ForMember(dest => dest.DesignationId, opt => opt.MapFrom(src => src.DesignationId.Encrypt()));

            AutoMapper.Mapper.CreateMap<ReffererModel, RefferrerVM>()
           .ForMember(dest => dest.ReferrerId, opt => opt.MapFrom(src => src.ReferrerId.Encrypt()));

            AutoMapper.Mapper.CreateMap<ProjectModel, ProjectVM>()
          .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.Encrypt()))
          .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.EmployeeModel.FullName==""? src.FullName:src.EmployeeModel.FullName))
          .ForMember(dest => dest.ProjectLead, opt => opt.MapFrom(src => src.ProjectLead != null ? src.ProjectLead.Value.Encrypt() : "0"));

            AutoMapper.Mapper.CreateMap<ResourcesModel, ResourcesVM>()
               .ForMember(dest => dest.ResourceId, opt => opt.MapFrom(src => src.ResourceId.Encrypt()))
                 .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Employee.FullName))
                 .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId != null ? src.ProjectId.Value.Encrypt() : "0"))
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId != null ? src.UserId.Value.Encrypt() : "0"));


            AutoMapper.Mapper.CreateMap<LeaveTypeModel, LeaveTypeVM>();
            AutoMapper.Mapper.CreateMap<TimeSheetModel, TimeSheetVM>()
           .ForMember(dest => dest.TimeSheetId, opt => opt.MapFrom(src => src.TimeSheetId.Encrypt()))
           .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.Encrypt()))
             .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId.Encrypt()))
           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Encrypt()));
            AutoMapper.Mapper.CreateMap<DateTimeSheetModel, DateTimeSheetVM>();
            AutoMapper.Mapper.CreateMap<DailyActionItemsModel, DailyActionItemVM>()
              .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId.Encrypt()))
             .ForMember(dest => dest.ActionItemId, opt => opt.MapFrom(src => src.ActionItemId.Encrypt()))
             .ForMember(dest => dest.TimeSheetId, opt => opt.MapFrom(src => src.TimeSheetId.Encrypt()));
            AutoMapper.Mapper.CreateMap<LeavesItemModel,LeavesItemVM>();
                     

            #endregion
        }
    }
}