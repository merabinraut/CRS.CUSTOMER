using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.SearchFilterManagement
{
    public class SearchFilterViewModel
    {
        public List<SearchFilterViewNewClubModel> ClubModel { get; set; }
    }
    public class SearchFilterViewNewClubModel
    {
        public string ClubId { get; set; }
        public string ClubName { get; set; }
        public string ClubNameJapanese { get; set; }
        public string ClubLogo { get; set; }
        public string ClubLocationId { get; set; }
    }
}