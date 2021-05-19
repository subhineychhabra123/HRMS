using EMPMGMT.Repository;
using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
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
   public class LeavesItemBusiness: ILeavesItemBusinuss 
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly LeavesItemRepository leavesitemrepository;
        private readonly LeaveTypeRepository leavetyperepository;
        private readonly UserRepository userRepository;
        private readonly LeavesApprovedByRepository leaveapprovedbyRepository;
        public LeavesItemBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            leavesitemrepository = new LeavesItemRepository(unitOfWork);
            leavetyperepository = new Repository.LeaveTypeRepository(unitOfWork);
            userRepository = new UserRepository(unitOfWork);
            leaveapprovedbyRepository = new LeavesApprovedByRepository(unitOfWork);
                     

        }

        public int SaveLeaveItemRecord(LeavesItemModel LeavesItemModel)
        {
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            Leaves Leaves = new Leaves();
            AutoMapper.Mapper.Map(LeavesItemModel, Leaves);
            var ApprovedId = userRepository.GetAll(m => m.EmailId == LeavesItemModel.UserEmail).FirstOrDefault();
            if (LeavesItemModel.FullDay!=null) { 
            Leaves.ApprovedById = null;
            Leaves.ApprovedStatus = null;
            Leaves.EmpId = LeavesItemModel.UserId;
            Leaves.FromDate = LeavesItemModel.FromDate;
            Leaves.ToDate = LeavesItemModel.ToDate;
            Leaves.Reason = LeavesItemModel.Reason;
            Leaves.TypeId = LeavesItemModel.FullDay;
            leavesitemrepository.Insert(Leaves);
            }
            if (LeavesItemModel.FirstHalf != 0) 
            {
                Leaves.ApprovedById = null;
                Leaves.ApprovedStatus = null;
                Leaves.EmpId = LeavesItemModel.UserId;
                Leaves.TypeId = LeavesItemModel.FirstHalf;
                Leaves.FromDate = LeavesItemModel.FromDateForFirstHalf;
                Leaves.Reason = LeavesItemModel.ReasonForFirstHalf;
                leavesitemrepository.Insert(Leaves);
            }
            if (LeavesItemModel.SecondHalf != 0)
            {
                Leaves.ApprovedById = null;
                Leaves.ApprovedStatus = null;
                Leaves.EmpId = LeavesItemModel.UserId;
                Leaves.TypeId = LeavesItemModel.SecondHalf;
                Leaves.FromDate = LeavesItemModel.FromDateForSecondHalf;
                Leaves.Reason = LeavesItemModel.ReasonForSecondHalf;
                leavesitemrepository.Insert(Leaves);
            }
            if (LeavesItemModel.shortleave != 0)
            {
                Leaves.ApprovedById = null;
                Leaves.ApprovedStatus = null;
                Leaves.EmpId = LeavesItemModel.UserId;
                Leaves.TypeId = LeavesItemModel.shortleave;
                Leaves.FromDate = LeavesItemModel.FromDateForshortleave;
                Leaves.Reason = LeavesItemModel.ReasonForshortleave;
                leavesitemrepository.Insert(Leaves);
            }
            return LeavesItemModel.LeaveId;
        }

        public void ApproveLeaveItemRecord(LeavesItemModel leavesItemModel)
        {
            Leaves Leaves = new Leaves();
            Leaves ApprovedObj = leavesitemrepository.GetAll(m => m.LeaveId == leavesItemModel.LeaveId).FirstOrDefault();
            ApprovedObj.ApprovedStatus = leavesItemModel.ApprovedStatus;
            ApprovedObj.ApprovedById = leavesItemModel.ApprovedById;
            leavesitemrepository.Update(ApprovedObj);
            LeavesApprovedBy approveby = new LeavesApprovedBy();
            approveby.LeaveId = leavesItemModel.LeaveId;
            approveby.EmpId = leavesItemModel.ApprovedById;
            leaveapprovedbyRepository.Insert(approveby);

            
        }




        public List<LeaveTypeModel> GetAllLeaveId()
        {
             List<LeaveTypeModel> GetAllLeaveIdObj = new List<LeaveTypeModel>();
             List<LeaveType> leaveTypes = new List<LeaveType>();
             leaveTypes = leavetyperepository.GetAll().ToList();
             AutoMapper.Mapper.Map(leaveTypes, GetAllLeaveIdObj);
             
             return GetAllLeaveIdObj;
        }

        public List<LeavesItemModel> GetAllLeavesDetail(ListingParameters listingParameters)
        {
            List<LeavesItemModel> LeaveDetailObject = new List<LeavesItemModel>();
            List<Leaves> leavedetail = new List<Leaves>();
            List<Employee> leaveEmployeName = new List<Employee>();
            


            leavedetail = leavesitemrepository.GetAll().ToList();

           // leaveEmployeName = userRepository.GetAll(m => m.UserId == leavedetail.UserId);
            AutoMapper.Mapper.Map(leavedetail, LeaveDetailObject);
            return LeaveDetailObject;
        
        }
    }
}
