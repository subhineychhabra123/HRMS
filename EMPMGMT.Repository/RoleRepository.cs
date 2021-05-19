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
    public class OrganizationUnitRepository : BaseRepository<OrganizationUnit>
    {
        public OrganizationUnitRepository(IUnitOfWork unit)
            : base(unit)
        {


        }
        public void AddDefaultOrganisationUnit(string OrgUnitName, int CompanyId, int Profileid)
        {

            Entities entities = (Entities)this.UnitOfWork.Db;
            entities.SSP_UserDefaultData(OrgUnitName, CompanyId, Profileid);

        }
      
      
        public List<SSP_GetOrganizationAutocomplete_Result> GetOrganizationAutocomplete(int companyId, int? orgUnitId, string searchText)
        {

            Entities entities = (Entities)this.UnitOfWork.Db;
            List<SSP_GetOrganizationAutocomplete_Result> orgUnits = entities.SSP_GetOrganizationAutocomplete(companyId, orgUnitId, searchText).ToList();
            return orgUnits;
        }

    }
}
