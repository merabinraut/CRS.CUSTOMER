using System.Collections.Generic;
using System.Linq;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagementV2;

namespace CRS.CUSTOMER.REPOSITORY.ReservationManagementV2
{
    public class ReservationManagementV2Repository : IReservationManagementV2Repository
    {
        RepositoryDao _dao;
        public ReservationManagementV2Repository()
        {
            _dao = new RepositoryDao();
        }

        public CommonDbResponse CancelReservation(Common commonDBRequest)
        {
            string sp_name = "sproc_reservation_history_management_v2 @Flag='cr'";
            sp_name += ",@ReservationId=" + _dao.FilterString(commonDBRequest.AgentId);
            sp_name += ",@ActionUser=" + _dao.FilterString(commonDBRequest.ActionUser);
            sp_name += ",@ActionIp=" + _dao.FilterString(commonDBRequest.ActionIP);
            sp_name += ",@ActionPlatform=" + _dao.FilterString(commonDBRequest.ActionPlatform);
            var dbresponse = _dao.ParseCommonDbResponse(sp_name);
            return dbresponse;
        }

        public CommonDbResponse DeleteReservation(Common commonDbRequest)
        {
            string sp_name = "sproc_reservation_history_management_v2 @Flag='dr'";
            sp_name += ",@ReservationId=" + _dao.FilterString(commonDbRequest.AgentId);
            sp_name += ",@ActionUser=" + _dao.FilterString(commonDbRequest.ActionUser);
            sp_name += ",@ActionIp=" + _dao.FilterString(commonDbRequest.ActionIP);
            sp_name += ",@ActionPlatform=" + _dao.FilterString(commonDbRequest.ActionPlatform);
            var dbResponse = _dao.ParseCommonDbResponse(sp_name);
            return dbResponse;
        }

        public List<AllHistoryModelCommon> GetAllHistoryList(string customerId)
        {
            string sp_name = "sproc_reservation_history_management_v2 @Flag='gahl'";
            sp_name += ",@CustomerId=" + _dao.FilterString(customerId);
            var dbResponseInfo = _dao.ExecuteDataTable(sp_name);
            if (dbResponseInfo != null && dbResponseInfo.Rows.Count > 0) return _dao.DataTableToListObject<AllHistoryModelCommon>(dbResponseInfo).ToList();
            return new List<AllHistoryModelCommon>();
        }

        public List<CancelledHistoryModelCommon> GetCancelledHistory(string customerId)
        {
            string sp_name = "sproc_reservation_history_management_v2 @Flag='gchl'";
            sp_name += ",@CustomerId=" + _dao.FilterString(customerId);
            var dbResponseInfo = _dao.ExecuteDataTable(sp_name);
            if (dbResponseInfo != null && dbResponseInfo.Rows.Count > 0) return _dao.DataTableToListObject<CancelledHistoryModelCommon>(dbResponseInfo).ToList();
            return new List<CancelledHistoryModelCommon>();
        }

        public ReservationHistoryDetailModelCommon GetReservationHistoryDetail(string customerId, string reservationId)
        {
            string SQL = "EXEC sproc_reservation_history_management_v2 @Flag='grhd'";
            SQL += ",@CustomerId=" + _dao.FilterString(customerId);
            SQL += ",@ReservationId=" + _dao.FilterString(reservationId);
            var dbResponse = _dao.ExecuteDataRow(SQL);
            if (dbResponse != null)
            {
                return new ReservationHistoryDetailModelCommon()
                {
                    ReservationId = _dao.ParseColumnValue(dbResponse, "ReservationId").ToString(),
                    ClubId = _dao.ParseColumnValue(dbResponse, "ClubId").ToString(),
                    CustomerId = _dao.ParseColumnValue(dbResponse, "CustomerId").ToString(),
                    ReservedDate = _dao.ParseColumnValue(dbResponse, "ReservedDate").ToString(),
                    VisitDate = _dao.ParseColumnValue(dbResponse, "VisitDate").ToString(),
                    VisitTime = _dao.ParseColumnValue(dbResponse, "VisitTime").ToString(),
                    TransactionStatus = _dao.ParseColumnValue(dbResponse, "TransactionStatus").ToString(),
                    ClubLogo = _dao.ParseColumnValue(dbResponse, "ClubLogo").ToString(),
                    ClubNameEng = _dao.ParseColumnValue(dbResponse, "ClubNameEng").ToString(),
                    ClubNameJp = _dao.ParseColumnValue(dbResponse, "ClubNameJp").ToString(),
                    Price = _dao.ParseColumnValue(dbResponse, "Price").ToString(),
                    HostImages = _dao.ParseColumnValue(dbResponse, "HostImages").ToString(),
                    LocationName = _dao.ParseColumnValue(dbResponse, "LocationName").ToString(),
                    NoOfPeople = _dao.ParseColumnValue(dbResponse, "NoOfPeople").ToString(),
                    PlanTime = _dao.ParseColumnValue(dbResponse, "PlanTime").ToString(),
                    Nomination = _dao.ParseColumnValue(dbResponse, "Nomination").ToString(),
                };
            }
            return new ReservationHistoryDetailModelCommon();
        }

        public List<ReservationHistoryV2ModelCommon> GetReservedList(string customerId)
        {
            string sp_name = "sproc_reservation_history_management_v2 @Flag='grhl'";
            sp_name += ",@CustomerId=" + _dao.FilterString(customerId);
            var dbResponseInfo = _dao.ExecuteDataTable(sp_name);
            if (dbResponseInfo != null && dbResponseInfo.Rows.Count > 0) return _dao.DataTableToListObject<ReservationHistoryV2ModelCommon>(dbResponseInfo).ToList();
            return new List<ReservationHistoryV2ModelCommon>();
        }

        public List<VisitedHistoryModelCommon> GetVisitedHistoryList(string customerId)
        {
            string sp_name = "sproc_reservation_history_management_v2 @Flag='gvhl'";
            sp_name += ",@CustomerId=" + _dao.FilterString(customerId);
            var dbResponseInfo = _dao.ExecuteDataTable(sp_name);
            if (dbResponseInfo != null && dbResponseInfo.Rows.Count > 0) return _dao.DataTableToListObject<VisitedHistoryModelCommon>(dbResponseInfo).ToList();
            return new List<VisitedHistoryModelCommon>();
        }

        public CommonDbResponse RedoReservation(Common commonDbRequest)
        {
            string sp_name = "sproc_reservation_history_management_v2 @Flag='rr'";
            sp_name += ",@ReservationId=" + _dao.FilterString(commonDbRequest.AgentId);
            sp_name += ",@ActionUser=" + _dao.FilterString(commonDbRequest.ActionUser);
            sp_name += ",@ActionIp=" + _dao.FilterString(commonDbRequest.ActionIP);
            sp_name += ",@ActionPlatform=" + _dao.FilterString(commonDbRequest.ActionPlatform);
            var dbResponse = _dao.ParseCommonDbResponse(sp_name);
            return dbResponse;
        }

        public CommonDbResponse RescheduleReservation(Common commonDBRequest, string time)
        {
            string sp_name = "sproc_reservation_history_management_v2 @Flag='urt'";
            sp_name += ",@Time=" + _dao.FilterString(time);
            sp_name += ",@ReservationId=" + _dao.FilterString(commonDBRequest.AgentId);
            sp_name += ",@ActionUser=" + _dao.FilterString(commonDBRequest.ActionUser);
            sp_name += ",@ActionIp=" + _dao.FilterString(commonDBRequest.ActionIP);
            sp_name += ",@ActionPlatform=" + _dao.FilterString(commonDBRequest.ActionPlatform);
            var dbResponseInfo = _dao.ParseCommonDbResponse(sp_name);
            return dbResponseInfo;
        }
    }
}

