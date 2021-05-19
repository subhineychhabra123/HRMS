using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Utility
{
    public class Enums
    {
        public enum BoolStatus
        {
            Yes,
            No
        }

        public enum LoginStatus
        {
            ValidLogin = 1,
            InvalidLogin = 2,
            ExpiredLogin = 3
        }

        public enum UserType
        {
            Admin = 1,
            User = 2,
            SubAdmin = 3
        }

        public enum Profile
        {
            Administrator = 1,

        }
        public enum ResponseResult
        {
            Success,
            Failure,
            NameExist,
            StageOrderExist
        }

        public enum Module
        {
            Lead = 3,
            Contact = 4,
            Account = 18,
            AccountCase = 19,
        }


        public enum ActionTaskPriority
        {
            [Description("")]
            NotSpecified = 0,
            [Description("High")]
            Urgent = 1,
            [Description(" Medium")] //To Do
            ToDo = 2,
            [Description(" Urgent")] // On Hold
            OnHold = 3,
            [Description(" Low")] //Working
            Working = 4,
            //[Description("Do Later")]
            //Invited = 5,
            //[Description("Parked")]
            //AllUsers = 6

        }

        public enum UserStatus
        {
            Active = 1,
            Deactive = 2,
            Pending = 3,
            Moreinfo = 4,
            Invited = 5,
            Expired = 6,
            AllUsers = 25

        }


        public enum RenderType
        {
            Table = 1,
            List = 2
        }



        public enum DataType
        {
            Percentage = 1,
            Number = 2,
            [Description("Decimal Number")]
            Decimal_Number = 3,
            Date = 4,
            [Description("Yes/No")]
            YesNo = 5,
            Text = 6
        }

        public enum EvalutionType
        {

            [Description("Less Is Good")]
            Less_Is_Good = 1,
            [Description("More Is Good")]
            More_Is_Good = 2,
            [Description("None")]
            None = 3
        }

        public enum EvalutionMethod
        {
            Absolute = 1,
            Proportionate = 2,
        }

        public enum DefaultYTDCalculation
        {
            Sum = 1,
            Average = 2,
            [Description("Sum And Average")]
            SumAndAverage = 3
        }

        public enum MetricChart
        {
            [Description("Pie Chart")]
            PieChart = 1,
            [Description("Bar Chart")]
            BarChart = 2,
            [Description("Trend Chart")]
            TrendChart = 3
        }

        public enum UpdateFrquency
        {

            Daily = 1,
            Weekly = 2


        }

        public enum MetricRecord
        {

            AllMetric = 10



        }


        public enum ActionListStatus
        {
            [Description("Red")]
            Red = 1,
            [Description("Yellow")]
            Yellow = 2,
            [Description("Green")]
            Green = 3,

        }


        public enum OccurrenceType
        {
            Daily = 1,
            Weekly = 2,
            Monthly = 3
        }
        public enum PeriodView
        {
            [Description("Before Last")]
            BeforeLast = 1,
            [Description("Last")]
            Last = 2,
            [Description("Current")]
            Current = 3,
            [Description("Next")]
            Next = 4
        }
        public enum DisplayedPeriod
        {
            [Description("From The Beginning Of Period")]
            FromTheBeginingOfPeriod = 1,
            Current = 2,

        }

        public enum RCAMetricType
        {
            OrgUnit = 1,
            ActionList = 2,
            Partner = 3,
        }

        public enum GoalLevel
        {
            Level1 = 1,
            Level2 = 2,
            Level3 = 3,
        }
        public enum ProjectStatus
        {
             Pipeline = 1,
              InProgress=2,
               Halted = 3,          
              Completed = 4

        }
        public enum OptionType
        {
            RadioButton = 1,
            CheckBox = 2,
            TextArea = 3
        }
    }
}
