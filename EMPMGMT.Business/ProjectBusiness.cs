using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
using EMPMGMT.Repository;
using EMPMGMT.Repository.Infrastructure.Contract;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business
{
    public class ProjectBusiness : IProjectBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ProjectRepository projectRespositoy;
        private readonly UserRepository userRepository;
        private readonly ProjectgroupRepository projectgroupRepository;
        private readonly UserGroupRepository usergroupRepository;
        private readonly ResourcesRepository resourceRepository;

        private readonly ActionListRepository actionListRepository;
        private readonly ActionItemRepository actionItemRepository;
        private readonly ActionItemResponsibleRepository actionItemResponsibleRepository;

        public ProjectBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            projectRespositoy = new ProjectRepository(unitOfWork);
            userRepository = new UserRepository(unitOfWork);
            projectgroupRepository = new ProjectgroupRepository(unitOfWork);
            usergroupRepository = new UserGroupRepository(unitOfWork);
            resourceRepository = new ResourcesRepository(unitOfWork);
           
            actionListRepository = new ActionListRepository(unitOfWork);
            actionItemRepository = new ActionItemRepository(unitOfWork);
            actionItemResponsibleRepository = new ActionItemResponsibleRepository(unitOfWork);
        }

        #region Project
        public List<ProjectModel> GetProjectList(ListingParameters listingParameters, ref int totalRecords, int UserId)
        {
            int currentPage = listingParameters.CurrentPage;
            int pageSize = listingParameters.PageSize;
            int status = (int)EMPMGMT.Utility.Enums.ProjectStatus.Pipeline;
            List<ProjectModel> projectModel = new List<ProjectModel>();
            List<Project> project = new List<Project>();
            ProjectModel objUserModel = new ProjectModel();
            Project objUsers = new Project();

            List<SSP_GetProjectListDetails_Result> projectList = new List<SSP_GetProjectListDetails_Result>();
            projectList = projectRespositoy.GetProjectListDetails(listingParameters.SearchText, UserId, listingParameters.CurrentPage, listingParameters.PageSize, listingParameters.OrderByColumn, listingParameters.OrderBy);
            if (projectList.Count()>0)
                totalRecords = projectList[0].TotalCount.Value;
            
            AutoMapper.Mapper.Map(projectList, projectModel);
            
            //For Testing and Confirm is pending........
          //  List<int> objItemList = actionItemResponsibleRepository.GetAll(x => x.ResponsibleUserId == UserId && x.RecordDeleted == false).Select(x => x.ActionItemId).ToList();
            //List<int> objActionList = actionItemRepository.GetAll(x => objItemList.Contains(x.ActionItemId) && x.RecordDeleted == false).Select(x => x.ActionListId.Value).ToList();
            //List<int> objProjectcount = actionListRepository.GetAll(x => objActionList.Contains(x.ActionListId) && x.RecordDeleted == false).Select(x => x.ProjectId.Value).ToList();
            //List<int> objProjectList = projectRespositoy.GetAll(x => objProjectcount.Contains(x.ProjectId) && x.RecordDeleted == false).Select(x => x.ProjectId).ToList();
            //////////////////////////////////////

            //List<int> resourceList = resourceRepository.GetAll(a => a.UserId == UserId && a.RecordDeleted == false && a.Project.RecordDeleted == false).Select(a => a.ProjectId.Value).ToList();
            //Expression<Func<Project, bool>> whereCondition = x => (((x.ProjectName.Contains(listingParameters.SearchText == null ? x.ProjectName : listingParameters.SearchText) || x.ProjectCode.Contains(listingParameters.SearchText == null ? x.ProjectCode : listingParameters.SearchText)) && (x.RecordDeleted == false) && ((resourceList.Contains(x.ProjectId) || objProjectList.Contains(x.ProjectId)) || (x.CreatedBy == UserId || x.ModifiedBy == UserId || x.ProjectLead == UserId))));
            //totalRecords = projectRespositoy.Count(whereCondition);
            //project = projectRespositoy.GetPagedRecordsDecending(whereCondition, y => y.CreatedDate, currentPage, pageSize).ToList();

            //AutoMapper.Mapper.Map(project, projectModel);
            return projectModel;
        }

        public ProjectModel ProjectDetails(ProjectModel projectModel)
        {

            Project project = projectRespositoy.SingleOrDefault(a => a.ProjectId == projectModel.ProjectId && a.RecordDeleted == false);
            AutoMapper.Mapper.Map(project, projectModel);
            return projectModel;
        }

        public string UpdateProject(ProjectModel projectModel)
        {
            var isProjectCodeExists = projectRespositoy.Exists(r => r.ProjectCode == projectModel.ProjectCode && r.ProjectId != projectModel.ProjectId && r.RecordDeleted == false);
            var isProjectNameExists = projectRespositoy.Exists(r => r.ProjectName == projectModel.ProjectName && r.ProjectId != projectModel.ProjectId && r.RecordDeleted == false);
            Project project = projectRespositoy.SingleOrDefault(x => x.ProjectId == projectModel.ProjectId && x.RecordDeleted == false);
            if ((!isProjectNameExists) && (project != null))
            {
                if ((!isProjectCodeExists) && (project != null))
                {
                    project.ProjectName = projectModel.ProjectName;
                    project.ProjectCode = projectModel.ProjectCode;
                    project.ProjectDescription = projectModel.ProjectDescription;
                    project.CommunicationEmailId = projectModel.CommunicationEmailId;
                    project.CommunicationEmailPassword = projectModel.CommunicationEmailPassword;
                    project.SourceControlDetail = projectModel.SourceControlDetail;
                    project.StartDate = projectModel.StartDate;
                    project.EndDate = projectModel.EndDate;
                    project.Status = projectModel.Status;
                    project.ProjectLead = projectModel.ProjectLead;
                    project.ModifiedBy = projectModel.ModifiedBy;
                    project.ModifiedDate = DateTime.UtcNow;
                    projectRespositoy.Update(project);
                    return "Success";
                }
                return "ProjectCode";
            }
            return "ProjectName";
        }



        public ProjectModel AddProject(ProjectModel projectModel)
        {
            var isProjectCodeExists = projectRespositoy.Exists(r => r.ProjectCode == projectModel.ProjectCode && r.RecordDeleted == false);
            var isProjectNameExists = projectRespositoy.Exists(r => r.ProjectName == projectModel.ProjectName && r.RecordDeleted == false);
            Project project = new Project();
            AutoMapper.Mapper.Map(projectModel, project);
            project.Employee = null;
            if (!isProjectNameExists)
            {
                if (!isProjectCodeExists)
                {
                    projectRespositoy.Insert(project);
                }
                else
                {
                    projectModel.ErrorProjectCode = true;
                }
            }
            else
            {
                projectModel.ErrorProjectName = true;
            }

            AutoMapper.Mapper.Map(project, projectModel);
            return projectModel;
        }

        public bool DeleteProject(ProjectModel projectModel)
        {
            Project project = projectRespositoy.SingleOrDefault(r => r.ProjectId == projectModel.ProjectId && r.RecordDeleted == false);
            if (project != null)
            {
                project.RecordDeleted = projectModel.RecordDeleted;
                project.ModifiedBy = projectModel.ModifiedBy;
                project.ModifiedDate = projectModel.ModifiedDate;
                projectRespositoy.Update(project);
                return true;
            }
            return false;
        }

        public ProjectModel GetProject(int projectId)
        {
            ProjectModel projectModel = new ProjectModel();
            Project project = projectRespositoy.SingleOrDefault(u => u.ProjectId == projectId && u.RecordDeleted == false);


            AutoMapper.Mapper.Map(project, projectModel);


            return projectModel;
        }

        #endregion

        #region Time Sheet DropDwon

        public List<ProjectModel> ProjectList(int UserId)
        {
            List<ProjectModel> listProjectModel = new List<ProjectModel>();
            List<Project> listProject = new List<Project>();
            //////// For using User and Project Group association//////
            /* List<UserGroup> usergrp = new List<UserGroup>(); 
             List<int> groupids=     usergroupRepository.GetAll(x=>x.UserId==UserId).Select(x=>x.GroupId.Value).ToList();
              usergrp = usergroupRepository.GetAll(x => x.UserId == UserId).ToList();
             List<int> projectids = projectgroupRepository.GetAll(x => groupids.Contains(x.GroupId)).Select(x=>x.ProjectId).ToList();
             listProject = projectRespositoy.GetAll(x => projectids.Contains(x.ProjectId) && x.RecordDeleted==false).ToList();
            // listProject = usergrp.Group.ProjectGroup.Project;*/
            ///////////////////////////////////////////////
            List<int> listresources = resourceRepository.GetAll(a => a.UserId == UserId && a.RecordDeleted == false).Select(a => a.ProjectId.Value).ToList();          // Directory using Resourcess
            if (listresources.Count() > 0)
            {
                listProject = projectRespositoy.GetAll(a => listresources.Contains(a.ProjectId) && a.RecordDeleted == false).ToList();
            }



            AutoMapper.Mapper.Map(listProject, listProjectModel);
            return listProjectModel;
        }


        public decimal ProjectWorkHours(int ProjectId)
        {
            List<SSP_GetProjectWorkHours_Result> projectworkHours = new List<SSP_GetProjectWorkHours_Result>();
            decimal projecttimehours = 0;
            projectworkHours=projectRespositoy.GetProjectWorkHours(ProjectId);
            if (projectworkHours.Count() > 0)
            {
                projecttimehours =projectworkHours[0].TotalWorkTime.Value;
            }
            else
            { return 0; }
            return projecttimehours;
        }
        #endregion


    }
}