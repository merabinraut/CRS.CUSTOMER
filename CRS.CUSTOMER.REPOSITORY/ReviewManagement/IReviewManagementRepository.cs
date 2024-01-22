using CRS.CUSTOMER.SHARED.ReviewManagement;
using CRS.CUSTOMER.SHARED;
using System.Collections.Generic;

namespace CRS.CUSTOMER.REPOSITORY.ReviewManagement
{
    public interface IReviewManagementRepository
    {
        #region Customer reviewed detail
        List<CustomerReviewedListCommon> GetCustomerReviewedList(string CustomerId);

        List<CustomerReviewDetailResponseCommon> CustomerReviewDetail(CustomerReviewDetailRequestCommon Request);
        #endregion
        CommonDbResponse GetReservationDetails(ReviewReservationRequestCommon Request);
        CommonDbResponse GetHostListByClub(ReviewHostListByClubRequestCommon Request);
        List<ReviewRemarkListResponseCommon> GetReviewRemarkList();
        List<ReviewDichotomousQuestionResponseCommon> GetReviewDichotomousQuestionList();
        List<ReviewDichotomousAnswerResponseCommon> GetReviewDichotomousAnswerList();
        CommonDbResponse ManageReview(ManageReviewRequestCommon Request);
    }
}
