using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
using EMPMGMT.Repository;
using EMPMGMT.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business
{
    public class ReffererBusiness : IReffererBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly RefferrerRepository refferrerRepository;
        public ReffererBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            refferrerRepository = new RefferrerRepository(unitOfWork);
          
        }

        public List<ReffererModel> GetUsersListForAutocomplete(string query)
        {
            List<ReffererModel> reffererModel = new List<ReffererModel>();

            List<Referrer> referrer = refferrerRepository.GetAll(x => (x.ReferrerName.Contains(query))).ToList();


            AutoMapper.Mapper.Map(referrer, reffererModel);
            return reffererModel;
        }
    }
}
