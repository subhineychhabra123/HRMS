using EMPMGMT.Domain;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business.Interfaces
{
    public interface IProjectBusiness
    {
        List<ProjectModel> GetProjectList(ListingParameters listingParameters, ref int totalRecords, int UserId);
        string UpdateProject(ProjectModel projectModel);
        ProjectModel AddProject(ProjectModel projectModel);
        ProjectModel ProjectDetails(ProjectModel projectModel);
        bool DeleteProject(ProjectModel projectModel);
        ProjectModel GetProject(int projectId);
        List<ProjectModel> ProjectList(int UserId);

        decimal ProjectWorkHours(int ProjectId);

    }
}
