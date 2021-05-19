using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
using EMPMGMT.Repository;
using EMPMGMT.Repository.Infrastructure.Contract;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business
{
   public  class ActionItemCommentBusiness: IActionItemCommentBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ActionItemCommentRepository actionItemCommentRepository;

        public ActionItemCommentBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            actionItemCommentRepository = new ActionItemCommentRepository(unitOfWork);

        }

    }
}
