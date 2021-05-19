using EMPMGMT.Domain;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business.Interfaces
{
    public interface ITimeSheetBusiness
    {
        int UpdateTimeSheetRecord(TimeSheetModel timeSheetModel);
        int SaveTimeSheetRecord(TimeSheetModel timeSheetModel);
        List<TimeSheetModel> GetTimeSheetList(int UserId);
        List<DateTimeSheetModel> GetDateTimeSheetList(string Month, int Year, int UserId);
        List<DailyActionItemsModel> GetDailyActionItemList(int date, string Month, int Year, int UserId);
        List<TimeSheetModel> GetCurrentTimeSheetDetails(int timeSheetId);
        TimeSheetModel GetCurrentTimeSheetbyId(int timeSheetId);

        int DeleteCurrentTimeSheetbyId(int timeSheetId, int UserId);

        List<EmployeeModel> GetEmployeeWorkHours(int UserId);
    }
}

