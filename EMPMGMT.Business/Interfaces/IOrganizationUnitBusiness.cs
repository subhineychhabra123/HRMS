using EMPMGMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business.Interfaces
{
    public interface IOrganizationUnitBusiness
    {
        List<OrganizationUnitModel> GetOrganizationUnitDDL(int companyId);
        bool AddOrganizationUnit(OrganizationUnitModel organizationUnitModel);
        string UpdateOrganizationUnit(OrganizationUnitModel organizationUnitModel);
        bool DeleteOrganizationUnit(int organizationUnitId, int reassignOrganizationUnitId, int companyId);
        List<OrganizationUnitModel> GetOrganizationUnitByCompanyId(int companyId);
        // List<OrganizationUnitModel>GetOrgUnitListForAddOrgUnitInDashboard(int companyId,int metricDashboardId);
        OrganizationUnitModel GetDefaultOrgUnitDetail(int CompanyId);
     
       List<OrganizationUnitModel> GetOrgUnitListForAutoComplete(int companyId, string searchString);
    }
}
