using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Utility
{
    [Serializable]
    public class CurrentUser
    {
        public int UserId { get; set; }
        public string UserIdEncrypted { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public string FullName { get; set; }
        //TODO //! Add value in session when login.
        public int CurrentDesignationId { get; set; }
        public Enums.UserType Role { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        private string _profileImageUrl = string.Empty;
        public string ProfileImageUrl
        {
            get
            {
                return string.IsNullOrWhiteSpace(this._profileImageUrl) ? "no_image.gif" : this._profileImageUrl;
            }
            set { this._profileImageUrl = value; }
        }

        public string RoleName
        {
            get
            {
                return Role.ToString();
            }
        }
        public string ReturnUrl { get; set; }
    }
    public class ListingParameters
    {
        public int CurrentPage { get; set; }
        public string Title { get; set; }
        public int PageSize { get; set; }
        public int UserStatus { get; set; }
        public bool IsRegistered { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string SearchText { get; set; }
        public string GoalId { get; set; }
        public string OrderBy { get; set; }
        public string OrderByColumn { get; set; }
        public string ActionListId { get; set; }
        public string ResponsibleUserId { get; set; }
        public string ProjectId { get; set; }
        public string ActionItemId { get; set; }
        public string MetricDashboardId { get; set; }
        public int LeaveId { get; set; }
        // public bool InvitedUser { get; set; }
        public bool Expired { get; set; }
    }

    public class MetricDashboardDataParameters
    {
        public int Id { get; set; }
        public string MetricId { get; set; }
        public string MetricDashboardId { get; set; }
        public string MdMetricId { get; set; }
        public string OrgUnitId { get; set; }
        public string Comment { get; set; }
        public string ActualValue { get; set; }
        public string PlannedValue { get; set; }
        public string ForecastValue { get; set; }
        public string SelectedDashboardViewType { get; set; }
        public string MdMetricResponsibleId { get; set; }
        public string RootCauseId { get; set; }
        public string Date { get; set; }
        public String DataType { get; set; }
        public string RootCauseAnalysisId { get; set; }
        public string DocumentId { get; set; }
    }


    public class MetricDashboardCellData
    {
        public int DataType { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
        public string MdMetricResponsibleId { get; set; }
        public string Comment { get; set; }
        public Nullable<int> ActualInteger { get; set; }
        public Nullable<int> PlannedInteger { get; set; }
        public Nullable<int> ForecastInteger { get; set; }
        public Nullable<decimal> ActualDecimal { get; set; }
        public Nullable<decimal> PlannedDecimal { get; set; }
        public Nullable<decimal> ForecastDecimal { get; set; }
        public Nullable<bool> ActualBit { get; set; }
        public Nullable<bool> PlannedBit { get; set; }
        public Nullable<bool> ForecastBit { get; set; }
        public string ActualText { get; set; }
        public string PlannedText { get; set; }
        public string ForecastText { get; set; }
        public Nullable<System.DateTime> ActualDateTime { get; set; }
        public Nullable<System.DateTime> PlannedDateTime { get; set; }
        public Nullable<System.DateTime> ForecastDateTime { get; set; }
        public Nullable<decimal> ActualPercentage { get; set; }
        public Nullable<decimal> PlannedPercentage { get; set; }
        public Nullable<decimal> ForecastPercentage { get; set; }
        public Boolean IsRootCause { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        //public int DataYear { get; set; }
        //public int DataMonth { get; set; }
        //public int DataDay { get; set; }
        public Boolean HasAttachments { get; set; }

    }
    public class MailBodyTemplate
    {
        public string RegistrationUserName { get; set; }
        public string AccountLoginUserId { get; set; }
        public string AccountLoginPassowrd { get; set; }
        public string AccountLoginUrl { get; set; }
        public string MailBody { get; set; }
        public string WebSiteLogoPath { get; set; }
        public string AccessForDeploymentModule { get; set; }
        public string ModulesAccessType { get; set; }
        public string ModuleAccessValidFrom { get; set; }
        public string ModuleAccessValidTill { get; set; }
        public string Comment_By_Admin { get; set; }


    }
    public class Response
    {
        public Response()
        {
            this.StatusCode = 200;

        }
        public string Status;
        public int StatusCode;
        public string Message;
    }


    public class EnumData
    {

        public int EnumID
        {
            get;
            set;
        }

        public string EnumText
        {
            get;
            set;
        }
    }

    public class MDTableHead
    {
        public int Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Selected { get; set; }
        public string Title { get; set; }
    }
    public class MDDetailViewModel
    {
        public DateTime ViewDate { get; set; }
        public int ViewType { get; set; }
        public string MetricDashboardId { get; set; }
        public int DisplayPeriod { get; set; }
    }

    public class HighChartParameters
    {
        public string MetricResponsibleId { get; set; }
        public int OccurenceType { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public int DisplayPeriod { get; set; }
        public string RCAId { get; set; }
    }

    public class DateConatiner
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PriorStartDate { get; set; }
        public DateTime PriorEndDate { get; set; }
    }

    public interface ITreeNode<T>
    {

        string ObjectId { get; set; }
        string ParentObjectId { get; set; }
        List<T> Children { get; set; }
    }

}
