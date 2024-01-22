using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationHistory
{
    public class CustomerReservationDetailModel
    {
        public string CustomerName { get; set; }
        public string NickName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string ReservedTime { get; set; }
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public string Price { get; set; }
        public string Remarks { get; set; }
        public string PlanTime { get; set; }
        public string NoOfPeople { get; set; }
        public string ReservedDate { get; set; }
        public string ClubName1 { get; set; }
        public string ClubName2 { get; set; }
        public string LogoImage { get; set; }
        public string VisitDate { get; set; }
        public string Liquor { get; set; }
        public List<ReservationHostDetailModel> ReservationHostDetailModel { get; set; } = new List<ReservationHostDetailModel>();
    }

    public class ReservationHostDetailModel
    {
        public string HostName { get; set; }
        public string Occupation { get; set; }
        public string HostImagePath { get; set; }
    }
}