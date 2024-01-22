namespace CRS.CUSTOMER.SHARED.ReservationHistoryManagement
{
    public class CustomerReservationDetailModelCommmon : Common
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
    }

    public class ReservationHostDetail
    {
        public string HostName { get; set; }
        public string Occupation { get; set; }
        public string HostImagePath { get; set; }
    }
}
