using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReviewManagement;
using DocumentFormat.OpenXml.Office2016.Excel;
using System.Collections.Generic;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.ReviewManagement
{
    public class ReviewManagementRepository : IReviewManagementRepository
    {
        private readonly RepositoryDao _dao;
        public ReviewManagementRepository()
        {
            _dao = new RepositoryDao();
        }

        #region Customer reviewed detail
        public List<CustomerReviewedListCommon> GetCustomerReviewedList(string CustomerId)
        {
            string SQL = "EXEC dbo.sproc_customer_review_management @Flag='gcrl'";
            SQL += ",@CustomerId=" + _dao.FilterString(CustomerId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<CustomerReviewedListCommon>(dbResponse).ToList();
            return new List<CustomerReviewedListCommon>();
        }

        public List<CustomerReviewDetailResponseCommon> CustomerReviewDetail(CustomerReviewDetailRequestCommon Request)
        {
            return new List<CustomerReviewDetailResponseCommon>();
        }
        #endregion

        public CommonDbResponse GetReservationDetails(ReviewReservationRequestCommon Request)
        {
            string SQL = "EXEC sproc_review_and_rating_management @Flag = 'grd'";
            SQL += ",@CustomerId=" + _dao.FilterString(Request.CustomerId);
            SQL += ",@ReservationId=" + _dao.FilterString(Request.ReservationId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count == 1)
            {
                var Code = _dao.ParseColumnValue(dbResponse.Rows[0], "Code").ToString();
                var Message = _dao.ParseColumnValue(dbResponse.Rows[0], "Message").ToString();
                if (!string.IsNullOrEmpty(Code) && Code == "0")
                {
                    return new CommonDbResponse()
                    {
                        Code = ResponseCode.Success,
                        Message = Message ?? "Success",
                        Data = _dao.DataTableToListObject<ReviewReservationResponseCommon>(dbResponse).First()
                    };
                }
                return new CommonDbResponse()
                {
                    Code = ResponseCode.Failed,
                    Message = Message ?? "Failed"
                };
            }
            return new CommonDbResponse()
            {
                Code = ResponseCode.Failed,
                Message = "Something went wrong. Please try again later."
            };
        }

        public CommonDbResponse GetHostListByClub(ReviewHostListByClubRequestCommon Request)
        {
            string SQL = "EXEC sproc_review_and_rating_management @Flag = 'gchl'";
            SQL += ",@ClubId=" + _dao.FilterString(Request.ClubId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                var Code = _dao.ParseColumnValue(dbResponse.Rows[0], "Code").ToString();
                var Message = _dao.ParseColumnValue(dbResponse.Rows[0], "Message").ToString();
                if (!string.IsNullOrEmpty(Code) && Code == "0")
                {
                    return new CommonDbResponse()
                    {
                        Code = ResponseCode.Success,
                        Message = Message ?? "Success",
                        Data = _dao.DataTableToListObject<ReviewHostListByClubResponseCommon>(dbResponse).ToList()
                    };
                }
                return new CommonDbResponse()
                {
                    Code = ResponseCode.Failed,
                    Message = Message ?? "Failed"
                };
            }
            return new CommonDbResponse()
            {
                Code = ResponseCode.Failed,
                Message = "Something went wrong. Please try again later."
            };
        }

        public List<ReviewRemarkListResponseCommon> GetReviewRemarkList()
        {
            string SQL = "EXEC sproc_review_and_rating_management @Flag = 'grrl'";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                return _dao.DataTableToListObject<ReviewRemarkListResponseCommon>(dbResponse).ToList();
            }
            return new List<ReviewRemarkListResponseCommon>();
        }

        public List<ReviewDichotomousQuestionResponseCommon> GetReviewDichotomousQuestionList()
        {
            string SQL = "EXEC sproc_review_and_rating_management @Flag = 'grdql'";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                return _dao.DataTableToListObject<ReviewDichotomousQuestionResponseCommon>(dbResponse).ToList();
            }
            return new List<ReviewDichotomousQuestionResponseCommon>();
        }

        public List<ReviewDichotomousAnswerResponseCommon> GetReviewDichotomousAnswerList()
        {
            string SQL = "EXEC sproc_review_and_rating_management @Flag = 'gdal'";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                return _dao.DataTableToListObject<ReviewDichotomousAnswerResponseCommon>(dbResponse).ToList();
            }
            return new List<ReviewDichotomousAnswerResponseCommon>();
        }

        public CommonDbResponse ManageReview(ManageReviewRequestCommon Request)
        {
            string SQL = "EXEC sproc_review_and_rating_management @Flag = 'mr'";
            SQL += ", @CustomerId=" + _dao.FilterString(Request.CustomerId);
            SQL += ", @ReservationId=" + _dao.FilterString(Request.ReservationId);
            SQL += ", @ClubId=" + _dao.FilterString(Request.ClubId);
            SQL += ", @HostList=" + _dao.FilterString(Request.HostList);
            SQL += ", @MVPHostId=" + _dao.FilterString(Request.MVPHostId);
            SQL += ", @DichotomousList=" + _dao.FilterString(Request.DichotomousList);
            SQL += ", @RemarkList=" + _dao.FilterString(Request.RemarkList);
            SQL += ", @RatingScale=" + Request.RatingScale;
            SQL += ", @ActionUser=" + _dao.FilterString(Request.ActionUser);
            SQL += ", @ActionIP=" + _dao.FilterString(Request.ActionIP);
            SQL += ", @ActionPlatform=" + _dao.FilterString(Request.ActionPlatform);
            return _dao.ParseCommonDbResponse(SQL);
        }
    }
}
