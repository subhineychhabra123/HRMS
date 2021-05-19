using EMPMGMT.Repository.Infrastructure;
using EMPMGMT.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Repository
{
    public class ProjectRepository : BaseRepository<Project>
    {
        public ProjectRepository(IUnitOfWork unit)
            : base(unit)
        {

        }


        public List<SSP_GetProjectListDetails_Result> GetProjectListDetails(string searchText,int userid, int currentPage, int PageSize, string sortColumns, string sortOrder )
        {

            Entities entities = (Entities)this.UnitOfWork.Db;
            List<SSP_GetProjectListDetails_Result> orgUnits = entities.SSP_GetProjectListDetails(searchText, userid, currentPage, PageSize, sortColumns, sortOrder).ToList();
            return orgUnits;
        }

        public List<SSP_GetProjectWorkHours_Result> GetProjectWorkHours(int ProjectId)
        {

            Entities entities = (Entities)this.UnitOfWork.Db;
            List<SSP_GetProjectWorkHours_Result> ProjectWorkHours = entities.SSP_GetProjectWorkHours(ProjectId).ToList();
            return ProjectWorkHours;
        }


    }
}
