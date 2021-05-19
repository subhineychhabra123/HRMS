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
    public class TechnologyBusiness : ITechnologyBusiness
    {
        private readonly TechnologyRepository technologyRepository;
        private readonly IUnitOfWork unitOfWork;
        public TechnologyBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            technologyRepository = new TechnologyRepository(unitOfWork);
        }
      public List<TechnologyModel> Technologies(ListingParameters listingParameters)
      {
          List<TechnologyModel> listTechnologyModel = new List<TechnologyModel>();
          List<Technology> listTechnology = new List<Technology>();
          listTechnology = technologyRepository.GetAll().ToList();
          AutoMapper.Mapper.Map(listTechnology, listTechnologyModel);
          return listTechnologyModel;
      }

    }
}
