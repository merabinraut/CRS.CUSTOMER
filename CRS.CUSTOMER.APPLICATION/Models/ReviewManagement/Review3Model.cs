using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReviewManagement
{
    public class Review3ViewModel
    {
        public ReviewClubDetailModel ReviewClubDetailModel { get; set; }
        public List<ReviewRemarkListResponseModel> ReviewRemarkListModel { get; set; } = new List<ReviewRemarkListResponseModel>();
    }
    public class ReviewRemarkListResponseModel
    {
        public string RemarkId { get; set; }
        public string RemarkLabel { get; set; }
        public string RemarkType { get; set; }
        public string RemarkTypeEnglish { get; set; }
    }
}