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
    public class UserRepository : BaseRepository<Employee>
    {
        public UserRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
        public string CheckUserProfile(int companyId, int userId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            string profile = string.Empty;//String.Join("|", entities.SSP_CheckUserPlan(companyId, userId).ToList().ToArray());
            return profile;
        }
        public void AddDefaultOrganisationUnit(string OrgUnitName, int CompanyId,int ProfileId)
        {

            Entities entities = (Entities)this.UnitOfWork.Db;
            entities.SSP_UserDefaultData(OrgUnitName, CompanyId, ProfileId);

        }

       


       

        public List<Employee> GetResponsiblesAutocomplete(int goalId, int companyId, string query)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            //List<User> listUsers = entities.Function_SSP_GetResponsiblesForAutoComplete(goalId, companyId, query).ToList();
            List<Employee> listUsers = new List<Employee>();
            return listUsers;
        }

    }
}
