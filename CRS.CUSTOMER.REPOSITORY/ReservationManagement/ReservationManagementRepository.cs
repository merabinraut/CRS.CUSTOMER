
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagement;
using CRS.CUSTOMER.SHARED.ReservationManagement;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.ReservationManagement
{
    public class ReservationManagementRepository : IReservationManagementRepository
    {
        RepositoryDao _dao;
        public ReservationManagementRepository()
        {
            _dao = new RepositoryDao();
        }

        #region Plan Details
        public List<ReservationPlanDetailCommon> GetPlanList(string PlanId = "", string CustomerId = "", string ClubId = "")
        {
            string SQL = "EXEC sproc_customer_club_and_host_management @Flag = 'gpl'";
            SQL += ",@PlanId=" + _dao.FilterString(PlanId);
            SQL += ",@customerAgentId=" + _dao.FilterString(CustomerId);
            SQL += ",@ClubId=" + _dao.FilterString(ClubId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                var response = new List<ReservationPlanDetailCommon>();
                var Code = _dao.ParseColumnValue(dbResponse.Rows[0], "Code").ToString();
                var Message = _dao.ParseColumnValue(dbResponse.Rows[0], "Message").ToString();
                if (!string.IsNullOrEmpty(Code) && Code.Trim() == "0")
                {
                    response = _dao.DataTableToListObject<ReservationPlanDetailCommon>(dbResponse).ToList();
                    response.ForEach(x => { x.Code = "0"; x.Message = "Success"; });
                }
                else
                {
                    response = new List<ReservationPlanDetailCommon>
                    {
                        new ReservationPlanDetailCommon
                        {
                            Code = "1",
                            Message = Message?? "Invalid request"
                        }
                    };
                }
                return response;
            }
            return new List<ReservationPlanDetailCommon>
            {
                new ReservationPlanDetailCommon
                {
                    Code = "1",
                    Message = "Invalid request"
                }
            };
        }

        #endregion
        #region Reservation Management
        public CommonDbResponse CreateReservation(CreateReservationDetailCommon Request)
        {
            string SQL = "EXEC sproc_reservation_management @Flag = 'ird'";
            SQL += ",@CustomerId=" + _dao.FilterString(Request.CustomerId);
            SQL += ",@ClubId=" + _dao.FilterString(Request.ClubId);
            SQL += ",@PlanId=" + _dao.FilterString(Request.PlanId);
            SQL += ",@VisitDate=" + _dao.FilterString(Request.VisitDate);
            SQL += ",@VisitTime=" + _dao.FilterString(Request.VisitTime);
            SQL += ",@NoOfPeople=" + _dao.FilterString(Request.NoOfPeople);
            SQL += ",@HostIdList=" + _dao.FilterString(Request.HostIdList);
            SQL += ",@ActionUser=" + _dao.FilterString(Request.ActionUser);
            SQL += ",@ActionIP=" + _dao.FilterString(Request.ActionIP);
            SQL += ",@ActionPlatform=" + _dao.FilterString(Request.ActionPlatform);
            return _dao.ParseCommonDbResponse(SQL);
        }

        public CommonDbResponse ManageReservation(ManageReservationDetailCommon Request)
        {
            string SQL = "EXEC sproc_reservation_management @Flag = 'mtpt'";
            SQL += ",@ReservationId=" + _dao.FilterString(Request.ReservationId);
            SQL += ",@CustomerId=" + _dao.FilterString(Request.CustomerId);
            SQL += ",@PaymentType=" + _dao.FilterString(Request.PaymentType);
            SQL += ",@ActionUser=" + _dao.FilterString(Request.ActionUser);
            SQL += ",@ActionIP=" + _dao.FilterString(Request.ActionIP);
            SQL += ",@ActionPlatform=" + _dao.FilterString(Request.ActionPlatform);
            return _dao.ParseCommonDbResponse(SQL);
        }

        public List<ReservationHistoryCommon> GetReservationHistory(string CustomerId, string ReservationId = "", string InvoiceId = "")
        {
            string SQL = "EXEC sproc_reservation_management @Flag = 'grh'";
            SQL += ",@CustomerId=" + _dao.FilterString(CustomerId);
            SQL += ",@ReservationId=" + _dao.FilterString(ReservationId);
            SQL += ",@InvoiceId=" + _dao.FilterString(InvoiceId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<ReservationHistoryCommon>(dbResponse).ToList();
            return new List<ReservationHistoryCommon>();
        }

        public ReservationDetailCommon GetReservationDetail(ManageReservationDetailCommon Request)
        {
            string SQL = "EXEC sproc_reservation_management @Flag = 'grd'";
            SQL += ",@CustomerId=" + _dao.FilterString(Request.CustomerId);
            SQL += ",@ReservationId=" + _dao.FilterString(Request.ReservationId);
            var dbResponse = _dao.ExecuteDataRow(SQL);
            if (dbResponse != null)
            {
                return new ReservationDetailCommon()
                {
                    ReservationId = _dao.ParseColumnValue(dbResponse, "ReservationId").ToString(),
                    InvoiceId = _dao.ParseColumnValue(dbResponse, "InvoiceId").ToString(),
                    ClubId = _dao.ParseColumnValue(dbResponse, "ClubId").ToString(),
                    CustomerId = _dao.ParseColumnValue(dbResponse, "CustomerId").ToString(),
                    ReservedDate = _dao.ParseColumnValue(dbResponse, "ReservedDate").ToString(),
                    VisitDate = _dao.ParseColumnValue(dbResponse, "VisitDate").ToString(),
                    VisitTime = _dao.ParseColumnValue(dbResponse, "VisitTime").ToString(),
                    NoOfPeople = _dao.ParseColumnValue(dbResponse, "NoOfPeople").ToString(),
                    PlanDetailId = _dao.ParseColumnValue(dbResponse, "PlanDetailId").ToString(),
                    HostDetailId = _dao.ParseColumnValue(dbResponse, "HostDetailId").ToString(),
                    PaymentType = _dao.ParseColumnValue(dbResponse, "PaymentType").ToString(),
                    TransactionStatus = _dao.ParseColumnValue(dbResponse, "TransactionStatus").ToString()
                };
            }
            return new ReservationDetailCommon();
        }
        #endregion
        #region"Customer Reservation Detail"
        public CustomerReservationDetailModelCommmon GetCustomerReservationDetail(string reservationId, string CustomerId)
        {
            CustomerReservationDetailModelCommmon responseInfo = new CustomerReservationDetailModelCommmon();
            string sp_name = "EXEC sproc_customer_getcustomerreservationdetail @Flag='gcrd'";
            sp_name += ",@ReservationId=" + _dao.FilterString(reservationId);
            sp_name += ",@CustomerId=" + _dao.FilterString(CustomerId);
            var dbResponseInfo = _dao.ExecuteDataTable(sp_name);
            if (dbResponseInfo != null)
            {
                var code = _dao.ParseColumnValue(dbResponseInfo.Rows[0], "Code").ToString();
                if (!string.IsNullOrEmpty(code) && code.Trim() == "0")
                {
                    foreach (DataRow row in dbResponseInfo.Rows)
                    {
                        return new CustomerReservationDetailModelCommmon()
                        {
                            CustomerName = row["CustomerName"].ToString(),
                            NickName = row["NickName"].ToString(),
                            EmailAddress = row["EmailAddress"].ToString(),
                            MobileNumber = row["MobileNumber"].ToString(),
                            PlanName = row["PlanName"].ToString(),
                            PlanType = row["PlanType"].ToString(),
                            Price = row["Price"].ToString(),
                            Remarks = row["Remarks"].ToString(),
                            PlanTime = row["PlanTime"].ToString(),
                            NoOfPeople = row["NoOfPeople"].ToString(),
                            ReservedDate = row["ReservedDate"].ToString(),
                            ReservedTime = row["ReservedTime"].ToString(),
                            ClubName1 = row["ClubName1"].ToString(),
                            ClubName2 = row["ClubName2"].ToString(),
                            LogoImage = row["LogoImage"].ToString(),
                            VisitDate = row["VisitDate"].ToString(),
                            Liquor = row["Liquor"].ToString()
                        };
                    }
                }
            }
            return responseInfo;
        }

        public CommonDbResponse CancelReservation(string reservationId, string actionUser, string actionIp, string actionPlatform)
        {
            string sp_name = "EXEC sproc_customer_cancelbookedreservation @Flag='cbr'";
            sp_name += ",@ReservationId=" + _dao.FilterString(reservationId);
            sp_name += ",@ActionUser=" + _dao.FilterString(actionUser);
            sp_name += ",@ActionIp=" + _dao.FilterString(actionIp);
            sp_name += ",@ActionPlatform=" + _dao.FilterString(actionPlatform);
            var dbResponseInfo = _dao.ParseCommonDbResponse(sp_name);
            return dbResponseInfo;
        }

        public CommonDbResponse UpdateReservationTime(string reservationId, string actionUser, string actionIp, string actionPlatform, string visitedTime)
        {
            string sp_name = "EXEC sproc_customer_updatereservationtime @Flag='urt'";
            sp_name += ",@ReservationId=" + _dao.FilterString(reservationId);
            sp_name += ",@ActionUser=" + _dao.FilterString(actionUser);
            sp_name += ",@ActionIp=" + _dao.FilterString(actionIp);
            sp_name += ",@ActionPlatform=" + _dao.FilterString(actionPlatform);
            sp_name += ",@UpdatedTime=" + _dao.FilterString(visitedTime);
            var dbResponseInfo = _dao.ParseCommonDbResponse(sp_name);
            return dbResponseInfo;
        }
        #endregion

        public List<ReservationHostDetail> GetReservationHostDetails(string ReservationId)
        {
            string SQL = "EXEC sproc_customer_getcustomerreservationdetail @Flag='grhs'";
            SQL += ",@ReservationId=" + _dao.FilterString(ReservationId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<ReservationHostDetail>(dbResponse).ToList();
            return new List<ReservationHostDetail>();
        }

        public List<ReservationHostDetail> GetHostDetailsForReservation(string HostListId)
        {
            string SQL = "EXEC sproc_reservation_management @Flag='ghlfr'";
            SQL += ",@HostIdList=" + _dao.FilterString(HostListId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<ReservationHostDetail>(dbResponse).ToList();
            return new List<ReservationHostDetail>();
        }
    }
}
