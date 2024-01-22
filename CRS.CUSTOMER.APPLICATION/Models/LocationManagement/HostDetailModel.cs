
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.LocationManagement
{
    public class HostDetailModel
    {
        public string ClubId { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string NoOfPeople { get; set; }
        public string PlanId { get; set; }
        public List<LocationHostListModel> HostListModels { get; set; }
    }
}