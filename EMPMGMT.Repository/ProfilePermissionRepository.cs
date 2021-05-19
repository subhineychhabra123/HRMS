using EMPMGMT.Repository;
using EMPMGMT.Repository.Infrastructure;
using EMPMGMT.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Repository
{
   public class ProfilePermissionRepository : BaseRepository<ProfilePermission>
    {
        public ProfilePermissionRepository(IUnitOfWork unit)
            : base(unit)
        {

        }

        public List<Nullable<int>> CheckMainMenuConfiguration(int ProfileId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<Nullable<int>> TotalRecoed = entities.SSP_MainMenuConfigurationCheck(ProfileId).ToList();
            return TotalRecoed;
        }

        public List<Nullable<int>> CheckMainMenuManageMetricDashboard(int ProfileId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<Nullable<int>> TotalRecoed = entities.SSP_MainMenuManageMetricDashboardCheck(ProfileId).ToList();
            return TotalRecoed;

        }
    }
}
