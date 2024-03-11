namespace CRS.CUSTOMER.APPLICATION.Models.Search
{
    public class ClubSearchFilterRequestModel
    {
        public string LocationId { get; set; }
        public string SearchFilter { get; set; }
        public string ClubCategory { get; set; }
        public string Price { get; set; }
        public string Shift { get; set; }
        public string Time { get; set; }
        public string ClubAvailability { get; set; }
    }

    public class HostSearchFilterRequestModel
    {
        public string LocationId { get; set; }
        public string SearchFilter { get; set; }
        public string Height { get; set; }
        public string Age { get; set; }
        public string BloodType { get; set; }
        public string ConstellationGroup { get; set; }
        public string Occupation { get; set; }
    }
}