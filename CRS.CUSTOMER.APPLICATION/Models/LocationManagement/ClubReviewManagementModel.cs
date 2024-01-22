using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.LocationManagement
{
    public class GetClubReviewsModel
    {
        public string ReviewId { get; set; }
        public string ClubId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerImage { get; set; }
        public string RatingScale { get; set; }
        public string CommentedOn { get; set; }
        public string VisitedOn { get; set; }
        public List<GetClubReviewHostModel> GetClubReviewHostList { get; set; } = new List<GetClubReviewHostModel>();
        public List<GetClubReviewRemarkModel> GetClubReviewRemarkList { get; set; } = new List<GetClubReviewRemarkModel>();
    }

    public class GetClubReviewHostModel
    {
        public string HostName { get; set; }
        public string HostImage { get; set; }
        public string HostId { get; set; }
        public string MVPHostId { get; set; }
    }

    public class GetClubReviewRemarkModel
    {
        public string EnglishRemark { get; set; }
        public string JapaneseRemark { get; set; }
        public string Remark { get; set; }
        public string RemarkType { get; set; }
    }
}