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
    public class TimeSheetBusiness : ITimeSheetBusiness
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly TimeSheetRepository timeSheetRepository;

        public TimeSheetBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            timeSheetRepository = new TimeSheetRepository(unitOfWork);

        }

        #region Update TimeSheet
        public int UpdateTimeSheetRecord(TimeSheetModel timeSheetModel)
        {
           // var isExists = timeSheetRepository.Exists(r => r.TimeSheetId == timeSheetModel.TimeSheetId  && r.DeletedRecord == false);

            //if (isExists)
            //{
            //    return 0;
            //}
            //else
            //{
                TimeSheet timeSheet = timeSheetRepository.SingleOrDefault(r => r.TimeSheetId == timeSheetModel.TimeSheetId && r.DeletedRecord == false);
                if (timeSheet != null)
                {
                    timeSheet.ActionItemId = timeSheetModel.ActionItemId;
                    timeSheet.ProjectId = timeSheetModel.ProjectId;
                    timeSheet.EntryDate = timeSheetModel.EntryDate;
                    timeSheet.TimeTaken = timeSheetModel.TimeTaken;
                    timeSheet.Comment = timeSheetModel.Comment;
                    timeSheet.ModifiedBy = timeSheetModel.ModifiedBy;
                    timeSheet.ModifiedDate = DateTime.UtcNow;
                    timeSheetRepository.Update(timeSheet);
                }
           // }
            return 0;
        }
        #endregion
        #region Save TimeSheet
        public int SaveTimeSheetRecord(TimeSheetModel timeSheetModel)
        {
            TimeSheet timeSheet = new TimeSheet();

            //var exists = timeSheetRepository.SingleOrDefault(r => r.ProjectId == timeSheetModel.ProjectId && r.ActionItemId == timeSheetModel.ActionItemId && r.UserId == timeSheetModel.UserId && r.EntryDate == timeSheetModel.EntryDate);
            //if (exists != null)
            //{
            //   if (exists.DeletedRecord == true)
            //   {         exists.ActionItemId = timeSheetModel.ActionItemId;
            //            exists.ProjectId = timeSheetModel.ProjectId;
            //            exists.UserId = timeSheetModel.UserId;
            //            exists.EntryDate = timeSheetModel.EntryDate;
            //            exists.TimeTaken = timeSheetModel.TimeTaken;
            //            exists.Comment = timeSheetModel.Comment;
            //            exists.ModifiedBy = timeSheetModel.ModifiedBy;
            //            exists.ModifiedDate = DateTime.UtcNow;
            //            exists.DeletedRecord = false;
            //            timeSheetRepository.Update(exists);                 
            //   }
            //    else
            //    {
            //        return 1;
            //    }
            //}
            //else
            //{
                AutoMapper.Mapper.Map(timeSheetModel, timeSheet);
                timeSheetRepository.Insert(timeSheet);
//        }
           
            return 0;
        }
        #endregion
        #region Get TimeSheet
        public List<TimeSheetModel> GetTimeSheetList(int UserId)
        {
          
            List<TimeSheetModel> timeSheetModel = new List<TimeSheetModel>();
            List<SSP_GetMonthYearTimeSheet_Result> listGetMonthTimeSheet = new List<SSP_GetMonthYearTimeSheet_Result>();
            listGetMonthTimeSheet = timeSheetRepository.GetTimeSheet(UserId).ToList();
            AutoMapper.Mapper.Map(listGetMonthTimeSheet, timeSheetModel);
            return timeSheetModel;
        }
        public List<DateTimeSheetModel> GetDateTimeSheetList(string Month, int Year, int UserId)
        {
            List<DateTimeSheetModel> objDatelimeList = new List<DateTimeSheetModel>();
            List<ssp_GetMonthTimeSheet_Result> objDateList = new List<ssp_GetMonthTimeSheet_Result>();
            objDateList = timeSheetRepository.GetDateTimeSheet(Month, Year, UserId);
            AutoMapper.Mapper.Map(objDateList, objDatelimeList);
            return objDatelimeList;
        }

        public List<DailyActionItemsModel> GetDailyActionItemList(int date, string Month, int Year, int UserId)
        {
            List<DailyActionItemsModel> DailyActionItemList = new List<DailyActionItemsModel>();
            List<ssp_GetActionItemDateTimeSheet_Result> objDailyActionItemList = new List<ssp_GetActionItemDateTimeSheet_Result>();
            objDailyActionItemList = timeSheetRepository.GetActionItemDetails(date, Month, Year, UserId);
            AutoMapper.Mapper.Map(objDailyActionItemList, DailyActionItemList);
            return DailyActionItemList;

        }
        public List<TimeSheetModel> GetCurrentTimeSheetDetails(int timeSheetId)
        {
            List<TimeSheetModel> CurrentTimeSheetModel= new List<TimeSheetModel>();
            List<TimeSheet> objCurrentTimeSheet = new List<TimeSheet>();
            objCurrentTimeSheet = timeSheetRepository.GetAll(x => x.TimeSheetId == timeSheetId).ToList();
            AutoMapper.Mapper.Map(objCurrentTimeSheet, CurrentTimeSheetModel);
            return CurrentTimeSheetModel;

        }


        public TimeSheetModel GetCurrentTimeSheetbyId(int timeSheetId)
        {
            TimeSheetModel CurrentTimeSheetModel = new TimeSheetModel();
            TimeSheet objCurrentTimeSheet = new TimeSheet();
            objCurrentTimeSheet = timeSheetRepository.SingleOrDefault(x => x.TimeSheetId == timeSheetId);
            AutoMapper.Mapper.Map(objCurrentTimeSheet, CurrentTimeSheetModel);
            return CurrentTimeSheetModel;

        }

        public int DeleteCurrentTimeSheetbyId(int timeSheetId, int UserId)
        {
            TimeSheet timeSheet = new TimeSheet();
             timeSheet = timeSheetRepository.SingleOrDefault(x => x.TimeSheetId == timeSheetId && x.DeletedRecord == false && (x.UserId == UserId));
            if (timeSheet == null)
                return 0;
            else
            {
                timeSheet.ModifiedBy = UserId;
                timeSheet.ModifiedDate = DateTime.UtcNow;
                timeSheet.DeletedRecord = true;
                timeSheetRepository.Update(timeSheet);
                return 1;
            }
        }

            #endregion

        #region Get User List with Total Work hours for Login Team Lead Only
        public List<EmployeeModel> GetEmployeeWorkHours(int UserId)
        {
            List<EmployeeModel> objEmployees = new List<EmployeeModel>();
            List<SSP_GetEmployeeWorkHours_Result> spEmployeeList = new List<SSP_GetEmployeeWorkHours_Result>();
            spEmployeeList= timeSheetRepository.GetEmployeeWorkHours(UserId);
            AutoMapper.Mapper.Map(spEmployeeList, objEmployees);            
            return objEmployees;
        }

        #endregion
    }
}
