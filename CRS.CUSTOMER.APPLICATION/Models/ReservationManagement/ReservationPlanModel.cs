using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationManagement
{
    public class ReservationPlanListModel
    {
        public string ClubId { get; set; }
        public List<ReservationPlanDetailModel> ReservationPlanDetailModel { get; set; }
    }
    public class ReservationPlanDetailModel
    {
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public string PlanTime { get; set; }
        public string Price { get; set; }
        public string Liquor { get; set; }
        public string Nomination { get; set; }
        public string Remarks { get; set; }
    }

    public class ReservationCommonPlanDetailModel
    {
        public string ClubId { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string NoOfPeople { get; set; }
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public string PlanTime { get; set; }
        public string Price { get; set; }
        public string Liquor { get; set; }
        public string Nomination { get; set; }
        public string Remarks { get; set; }
    }
}