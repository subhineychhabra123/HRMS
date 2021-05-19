using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
using EMPMGMT.Repository;
using EMPMGMT.Repository.Infrastructure.Contract;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business
{
    public class DesignationBusiness : IDesignationBusiness
    {
        private readonly DesignationRepository designationRepository;
        private readonly IUnitOfWork unitOfWork;
        public DesignationBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            designationRepository = new DesignationRepository(unitOfWork);
        }

        public List<DesignationModel> DesignationList(ListingParameters listingParameters)
        {

            List<DesignationModel> listDesignationModel = new List<DesignationModel>();
            List<Designation> listDesignation = new List<Designation>();
            listDesignation = designationRepository.GetAll(p => p.CompanyId == listingParameters.CompanyId || p.CompanyId.HasValue == false).ToList();
            AutoMapper.Mapper.Map(listDesignation, listDesignationModel);
            return listDesignationModel;
        }


     
    }
}
