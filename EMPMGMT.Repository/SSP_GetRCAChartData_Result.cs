//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace STRATEGY.Repository
{
    using System;
    
    public partial class SSP_GetRCAChartData_Result
    {
        public int MetricDashboardMonthlyDataId { get; set; }
        public int MDMetricResponsibleId { get; set; }
        public Nullable<int> MonthNo { get; set; }
        public string Comment { get; set; }
        public Nullable<int> ActualInteger { get; set; }
        public Nullable<int> PlannedInteger { get; set; }
        public Nullable<int> ForecastInteger { get; set; }
        public Nullable<decimal> ActualDecimal { get; set; }
        public Nullable<decimal> PlannedDecimal { get; set; }
        public Nullable<decimal> ForecastDecimal { get; set; }
        public Nullable<decimal> ActualPercentage { get; set; }
        public Nullable<decimal> PlannedPercentage { get; set; }
        public Nullable<decimal> ForecastPercentage { get; set; }
        public Nullable<bool> ActualBit { get; set; }
        public Nullable<bool> PlannedBit { get; set; }
        public Nullable<bool> ForecastBit { get; set; }
        public string ActualText { get; set; }
        public string PlannedText { get; set; }
        public string ForecastText { get; set; }
        public Nullable<System.DateTime> ActualDateTime { get; set; }
        public Nullable<System.DateTime> PlannedDateTime { get; set; }
        public Nullable<System.DateTime> ForecastDateTime { get; set; }
        public Nullable<bool> IsRootCause { get; set; }
        public Nullable<bool> HasAttachments { get; set; }
        public System.DateTime MonthDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public string Title { get; set; }
        public int DisplayPeriod { get; set; }
        public int MetricId { get; set; }
    }
}