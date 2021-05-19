using EMPMGMT.Repository.Infrastructure;
using EMPMGMT.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Repository
{
    public class TimeSheetRepository :BaseRepository<TimeSheet>
    {
        Entities entities;
        public TimeSheetRepository(IUnitOfWork unit)
            : base(unit)
        {
            entities = (Entities)this.UnitOfWork.Db;
        }



        public List<SSP_GetMonthYearTimeSheet_Result> GetTimeSheet(int UserId)
        {
            List<SSP_GetMonthYearTimeSheet_Result> listTimeSheet = entities.SSP_GetMonthYearTimeSheet(UserId).ToList();
            return listTimeSheet;
        }
        public List<ssp_GetMonthTimeSheet_Result> GetDateTimeSheet(string Month, int Year, int UserId)
        {
            List<ssp_GetMonthTimeSheet_Result> listDateTimeSheet = entities.ssp_GetMonthTimeSheet(Month, Year, UserId).ToList();
            return listDateTimeSheet;
        }
        public List<ssp_GetActionItemDateTimeSheet_Result> GetActionItemDetails(int Date, string Month, int Year, int UserId)
        {
            List<ssp_GetActionItemDateTimeSheet_Result> listActionItemList = entities.ssp_GetActionItemDateTimeSheet(Date, Month, Year, UserId).ToList();
            return listActionItemList;

        }

        public List<SSP_GetEmployeeWorkHours_Result> GetEmployeeWorkHours(int UserId)
        {
            List<SSP_GetEmployeeWorkHours_Result> listActionItemList = entities.SSP_GetEmployeeWorkHours(UserId).ToList();
            return listActionItemList;

        }
    }
}
