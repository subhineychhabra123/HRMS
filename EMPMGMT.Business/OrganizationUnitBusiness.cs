using EMPMGMT.Repository;
using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;

using EMPMGMT.Repository.Infrastructure.Contract;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business
{
    public class OrganizationUnitBusiness : IOrganizationUnitBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly OrganizationUnitRepository organizationUnitRepository;
        private readonly UserRepository userRepository;
        public OrganizationUnitBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            organizationUnitRepository = new OrganizationUnitRepository(unitOfWork);
            userRepository = new UserRepository(unitOfWork);
        }
        public bool AddOrganizationUnit(OrganizationUnitModel organizationUnitModel)
        {
            OrganizationUnit organizationUnit = new OrganizationUnit();

            bool isExists = organizationUnitRepository.Exists(r => r.OrgUnitName == organizationUnitModel.OrgUnitName && r.RecordDeleted == false); //(r.CompanyId == organizationUnitModel.CompanyId) &&
            if (!isExists)
            {
                organizationUnit = new OrganizationUnit();

                AutoMapper.Mapper.Map(organizationUnitModel, organizationUnit);
                organizationUnitRepository.Insert(organizationUnit);
                return true;
            }
            else
            {
                return false;
            }
        }

      
        //public int GetDefaultMdOrgUnitID(int companyId, int metricDashboardId)
        //{
        //    OrganizationUnitModel orgUnitModel = null;
        //    //var MdOrgUnit = 0;
        //    int MdOrgUnit = organizationUnitRepository.GetDefaultMdOrgUnitID(companyId, metricDashboardId);

        //  //  SingleOrDefault(u => u.CompanyId == companyId && u.MetricDashboardOrganization.Count(x => x.MDOrgId == 0) > 0);
        //    orgUnitModel = new OrganizationUnitModel();
        //    orgUnitModel.MetricDashboardOrgModel.MDOrgId = MdOrgUnit;
        //    AutoMapper.Mapper.Map(MdOrgUnit, orgUnitModel);
        //    //return orgUnitModel;
        //    return MdOrgUnit;
        //}
        public OrganizationUnitModel GetDefaultOrgUnitDetail(int CompanyId)
        {
            OrganizationUnitModel orgUnitModel = null;
            OrganizationUnit user = organizationUnitRepository.SingleOrDefault(u=>u.IsDefaultUnit == true);//u => u.CompanyId == CompanyId &&
            orgUnitModel = new OrganizationUnitModel();
            AutoMapper.Mapper.Map(user, orgUnitModel);
            return orgUnitModel;
        }
        //public List<OrganizationUnitModel> GetOrgUnitListForAddOrgUnitInDashboard(int companyId, int metricDashboardId)
        //{

        //}
        //public bool IsOrganisationNameExist(int companyId, string OrgUnitName)
        //{
        //    OrganizationUnit organizationUnit = new OrganizationUnit();

        //    bool isExists = organizationUnitRepository.Exists(r => r.OrgUnitName == organizationUnitModel.OrgUnitName && (r.CompanyId == organizationUnitModel.CompanyId || r.CompanyId == null) && r.RecordDeleted == false);
        //    if (!isExists)
        //    {
        //        organizationUnit = new OrganizationUnit();

        //        AutoMapper.Mapper.Map(organizationUnitModel, organizationUnit);
        //        organizationUnitRepository.Insert(organizationUnit);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        public string UpdateOrganizationUnit(OrganizationUnitModel organizationUnitModel)
        {

            var isExists = organizationUnitRepository.Exists(r => r.OrgUnitName == organizationUnitModel.OrgUnitName && r.OrgUnitId != organizationUnitModel.OrgUnitId && r.RecordDeleted == false);// (r.CompanyId == organizationUnitModel.CompanyId) &&
            if (isExists)
            {
                return "AlreadyExist";
            }
            else
            {
                OrganizationUnit organizationUnit = organizationUnitRepository.SingleOrDefault(r => r.OrgUnitId == organizationUnitModel.OrgUnitId && r.RecordDeleted == false);//r.CompanyId == organizationUnitModel.CompanyId && 
                if (organizationUnit != null)
                {
                    organizationUnit.OrgUnitName = organizationUnitModel.OrgUnitName;
                    if (!organizationUnitModel.IsDefaultUnit)
                    {
                        organizationUnit.ParentOrgUnitId = organizationUnitModel.ParentOrgUnitId;
                    }
                    organizationUnit.ModifiedDate = organizationUnitModel.ModifiedDate;
                    organizationUnit.ModifiedBy = organizationUnitModel.ModifiedBy;
                    organizationUnitRepository.Update(organizationUnit);
                    return "Success";
                }
                return "Error";

            }
        }
        public bool DeleteOrganizationUnit(int organizationUnitId, int reassignOrganizationUnitId, int companyId)
        {
            try
            {
                List<OrganizationUnit> organizationUnitList = new List<OrganizationUnit>();
                OrganizationUnit organizationUnitObject = new OrganizationUnit();
                List<Employee> userList = new List<Employee>();
                if (reassignOrganizationUnitId > 0)
                {
                    organizationUnitList = organizationUnitRepository.GetAll(r => r.ParentOrgUnitId == organizationUnitId && r.RecordDeleted == false).ToList();//&& r.CompanyId == companyId
                    if (organizationUnitList.Count > 0)
                    {
                        foreach (OrganizationUnit organizationUnit in organizationUnitList)
                        {
                            organizationUnit.ParentOrgUnitId = reassignOrganizationUnitId;
                        }
                        organizationUnitRepository.UpdateAll(organizationUnitList);
                    }
                }
                organizationUnitObject = organizationUnitRepository.SingleOrDefault(r => r.OrgUnitId == organizationUnitId && r.RecordDeleted == false);
                organizationUnitObject.RecordDeleted = true;
             
                //organizationUnitObject.ToList().ForEach(x => { x.RecordDeleted = true; });

                organizationUnitRepository.Update(organizationUnitObject);
                //code for updating the OrganizationUnitId in user table
                userList = userRepository.GetAll(r => r.OrgUnitId == organizationUnitId && r.Status == (int)Enums.UserStatus.Active).ToList(); // && r.CompanyId == companyId
                if (userList.Count > 0)
                {
                    foreach (Employee u in userList)
                    {
                        u.OrgUnitId = reassignOrganizationUnitId;
                    }
                    userRepository.UpdateAll(userList);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<OrganizationUnitModel> GetOrganizationUnitByCompanyId(int companyId)
        {
            List<OrganizationUnitModel> listOrganizationUnitModel = new List<OrganizationUnitModel>();
            List<OrganizationUnit> listOrganizationUnit = organizationUnitRepository.GetAll(p=>p.RecordDeleted == false).OrderBy(x => x.OrgUnitName).ToList(); //p => p.CompanyId == companyId &&
            AutoMapper.Mapper.Map(listOrganizationUnit, listOrganizationUnitModel);
            return listOrganizationUnitModel;
        }
        public List<OrganizationUnitModel> GetOrganizationUnitDDL(int companyId)
        {
            List<OrganizationUnitModel> listOrganizationUnitModel = new List<OrganizationUnitModel>();
            listOrganizationUnitModel = organizationUnitRepository.GetAll(p => p.RecordDeleted == false || p.CompanyId == null).Select(x => new OrganizationUnitModel { OrgUnitId = x.OrgUnitId, OrgUnitName = x.OrgUnitName }).ToList();//p.CompanyId == companyId && 
            if (listOrganizationUnitModel == null) { new List<OrganizationUnitModel>(); }
            listOrganizationUnitModel.Insert(0, new OrganizationUnitModel
            {
                OrgUnitId = Constants.DropDownListDefualtValue,
                OrgUnitName = Constants.DropDownListDefaultText
            });
            return listOrganizationUnitModel;
        }

        public List<OrganizationUnitModel> GetOrgUnitListForAutoComplete(int companyId, string searchString)
        {
            List<OrganizationUnitModel> listOrganizationUnitModel = new List<OrganizationUnitModel>();
            List<OrganizationUnit> listOrganizationUnit = new List<OrganizationUnit>();
            //listOrganizationUnit = organizationUnitRepository.GetAll(x => x.OrgUnitName.Contains(searchString) && x.CompanyId == companyId && x.RecordDeleted == false).ToList();
            int? orgUnitId = null;
            listOrganizationUnitModel = organizationUnitRepository.GetOrganizationAutocomplete(companyId, orgUnitId, searchString).ToList().Select(x => new OrganizationUnitModel { OrgUnitId = Convert.ToInt32(x.OrgUnitId), OrgUnitName = x.OrgUnitName, OrgUnitNameListingText = x.OrgUnitNameListingText }).ToList();
            // AutoMapper.Mapper.Map(listOrganizationUnit, listOrganizationUnitModel);
            return listOrganizationUnitModel;
        }
    }
}
