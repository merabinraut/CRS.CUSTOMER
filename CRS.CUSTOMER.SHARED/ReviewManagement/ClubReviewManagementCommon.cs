using System.Collections.Generic;

namespace CRS.CUSTOMER.SHARED.ReviewManagement
{
    public class GetClubReviewsCommon
    {
        public string ReviewId { get; set; }
        public string ClubId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerImage { get; set; }
        public string RatingScale { get; set; }
        public string CommentedOn { get; set; }
        public string VisitedOn { get; set; }
        public List<GetClubReviewHostCommon> GetClubReviewHostList { get; set; } = new List<GetClubReviewHostCommon>();
        public List<GetClubReviewRemarkCommon> GetClubReviewRemarkList { get; set; } = new List<GetClubReviewRemarkCommon>();
    }

    public class GetClubReviewHostCommon
    {
        public string HostName { get; set; }
        public string HostImage { get; set; }
        public string HostId { get; set; }
        public string MVPHostId { get; set; }
    }

    public class GetClubReviewRemarkCommon
    {
        public string EnglishRemark { get; set; }
        public string JapaneseRemark { get; set; }
        public string RemarkType { get; set; }
        public bool IsCourseExtended { get; set; }
    }
}
