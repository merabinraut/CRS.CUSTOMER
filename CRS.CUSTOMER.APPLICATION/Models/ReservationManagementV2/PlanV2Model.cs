using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationManagementV2
{
    public class PlanViewModel
    {
        public List<PlanDetailModel> PlanDetailModel { get; set; } = new List<PlanDetailModel>();
        public ClubBasicDetailModel ClubDetailModel { get; set; } = new ClubBasicDetailModel();
    }
    public class PlanDetailModel
    {
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanTime { get; set; }
        public string PlanPrice { get; set; }
        public string StrikePrice { get; set; }
        public string IsStrikeOut { get; set; }
        public string PlanNomination { get; set; }
        public string PlanRemarks { get; set; }
        public string PlanLiquor { get; set; }
        public string PlanLiquorJapanese { get; set; }
         
    }
}