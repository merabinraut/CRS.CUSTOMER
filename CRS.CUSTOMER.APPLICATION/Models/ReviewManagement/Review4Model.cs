using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReviewManagement
{
    public class Review4ViewModel
    {
        public ReviewClubDetailModel ReviewClubDetailModel { get; set; }
        public List<ReviewDichotomousQuestionModel> ReviewDichotomousQuestionModel { get; set; } = new List<ReviewDichotomousQuestionModel>();
        public List<ReviewDichotomousAnswerModel> ReviewDichotomousAnswerModel { get; set; } = new List<ReviewDichotomousAnswerModel>();
    }
    public class ReviewDichotomousQuestionModel
    {
        public string RemarkId { get; set; }
        public string RemarkLabel { get; set; }
    }
    public class ReviewDichotomousAnswerModel
    {
        public string RemarkId { get; set; }
        public string RemarkLabel { get; set; }
        public string RemarkLabelEnglish { get; set; }
    }
}