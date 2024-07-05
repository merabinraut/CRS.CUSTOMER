
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CRS.CUSTOMER.APPLICATION.Models.ProfileManagement
{
    public class PointReportModel
    {
        public List<PointReportDetailModel> AllPointReportList { get; set; }=new List<PointReportDetailModel>();
        public List<PointReportDetailModel> CreditPointReportList { get; set; } = new List<PointReportDetailModel>();
        public List<PointReportDetailModel> DebitPointReportList { get; set; } = new List<PointReportDetailModel>();
    }
    public class PointReportDetailModel
    {
        public string DayType { get; set; }
        public string TransactionDate { get; set; }
        public string Point { get; set; }
        public string TransactionMode { get; set; }
        public string Remark { get; set; }
    }
}