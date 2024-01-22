using CRS.CUSTOMER.REPOSITORY.ReviewManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReviewManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.ReviewManagement
{
    public class ReviewManagementBusiness : IReviewManagementBusiness
    {
        private readonly IReviewManagementRepository _repo;
        public ReviewManagementBusiness(ReviewManagementRepository repo)
        {
            _repo = repo;
        }
        #region Customer reviewed detail
        public List<CustomerReviewedListCommon> GetCustomerReviewedList(string CustomerId)
        {
            return _repo.GetCustomerReviewedList(CustomerId);
        }

        public List<CustomerReviewDetailResponseCommon> CustomerReviewDetail(CustomerReviewDetailRequestCommon Request)
        {
            return _repo.CustomerReviewDetail(Request);
        }
        #endregion

        public CommonDbResponse GetHostListByClub(ReviewHostListByClubRequestCommon Request)
        {
            return _repo.GetHostListByClub(Request);
        }

        public CommonDbResponse GetReservationDetails(ReviewReservationRequestCommon Request)
        {
            return _repo.GetReservationDetails(Request);
        }

        public List<ReviewDichotomousAnswerResponseCommon> GetReviewDichotomousAnswerList()
        {
            return _repo.GetReviewDichotomousAnswerList();
        }

        public List<ReviewDichotomousQuestionResponseCommon> GetReviewDichotomousQuestionList()
        {
            return _repo.GetReviewDichotomousQuestionList();
        }

        public List<ReviewRemarkListResponseCommon> GetReviewRemarkList()
        {
            return _repo.GetReviewRemarkList();
        }

        public CommonDbResponse ManageReview(ManageReviewRequestCommon Request)
        {
            return _repo.ManageReview(Request);
        }
    }
}
