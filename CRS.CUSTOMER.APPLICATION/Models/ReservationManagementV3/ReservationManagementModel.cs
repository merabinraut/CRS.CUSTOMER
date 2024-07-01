using CRS.CUSTOMER.APPLICATION.Models.ReservationManagementV2;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationManagementV3
{
    public class ReservationManagementCommonModel
    {
        public string SelectedHost { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string NoOfPeople { get; set; }
        public ClubBasicDetailModel ClubDetailModel { get; set; }
        public List<PlanDetailModel> PlanDetailModel { get; set; }
        public List<HostListV2Model> HostListModel { get; set; }
        public BillingViewModel BillingViewModel { get; set; }
    }
}