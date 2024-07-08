
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CRS.CUSTOMER.APPLICATION.Models.ProfileManagement
{
    public class PointReportModel
    {
        public List<PointDayTypeModel> AllPointReportList { get; set; }=new List<PointDayTypeModel>();
        public List<PointDayTypeModel> CreditPointReportList { get; set; } = new List<PointDayTypeModel>();
        public List<PointDayTypeModel> DebitPointDayTypeList { get; set; } = new List<PointDayTypeModel>();
    }
    public class PointDayTypeModel
    {
        public string DayType { get; set; }
        public List<PointReportDetailModel> PointReportList { get; set; } = new List<PointReportDetailModel>();
    }
    public class PointReportDetailModel
    {
        public string DayType { get; set; }
        public string TransactionDate { get; set; }
        public string Point { get; set; }
        public string TransactionMode { get; set; }
        public string Remark { get; set; } 
        public string TotalPoints { get; set; }
    }
}