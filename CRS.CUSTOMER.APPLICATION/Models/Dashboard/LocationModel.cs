using CRS.CUSTOMER.APPLICATION.Models.SearchFilterManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.Dashboard
{
    /// <summary>
    /// View Locations model for customer dashboard
    /// </summary>
    public class LocationListModel
    {
        public string LocationID { get; set; }
        public string LocationName { get; set; }
        public string LocationImage { get; set; }
        public string LocationURl { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Action { get; set; }
        public string LocationDisplayName { get; set; }
    }

    public class RecommendationLocationModel
    {
        public string id { get; set; }
        public string name { get; set; }
        //public double lat { get; set; }
        //public double lng { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class RecommendedClubAndHostModel
    {
        public List<ClubRecommendationListModel> RecommendedClubModel { get; set; } = new List<ClubRecommendationListModel>();
        public List<HostRecommendationListModel> RecommendedHostModel { get; set; } = new List<HostRecommendationListModel>();
    }
}