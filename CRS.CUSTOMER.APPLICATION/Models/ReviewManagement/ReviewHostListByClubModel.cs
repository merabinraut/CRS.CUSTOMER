using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReviewManagement
{
    public class ReviewHostListByClubViewModel
    {
        public ReviewClubDetailModel ReviewClubDetailModel { get; set; }
        public List<ReviewHostListByClubResponseModel> ReviewHostListModel { get; set; }
    }
    public class ReviewHostListByClubResponseModel
    {
        public string ClubId { get; set; }
        public string HostId { get; set; }
        public string HostName { get; set; }
        public string HostRank { get; set; }
        public string HostPosition { get; set; }
        public string HostImage { get; set; }
    }
}