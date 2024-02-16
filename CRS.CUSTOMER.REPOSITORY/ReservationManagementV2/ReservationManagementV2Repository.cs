using System.Collections.Generic;
using System.Linq;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagementV2;
using CRS.CUSTOMER.SHARED.ReservationManagement;
using DocumentFormat.OpenXml.Office2016.Excel;

namespace CRS.CUSTOMER.REPOSITORY.ReservationManagementV2
{
    public class ReservationManagementV2Repository:IReservationManagementV2Repository
	{
		RepositoryDao _dao;
		public ReservationManagementV2Repository()
		{
			_dao = new RepositoryDao();
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
                    VisitedDate = _dao.ParseColumnValue(dbResponse, "VisitDate").ToString(),
                    VisitTime = _dao.ParseColumnValue(dbResponse, "VisitTime").ToString(),
                    TransactionStatus = _dao.ParseColumnValue(dbResponse, "TransactionStatus").ToString(),
                    ClubLogo = _dao.ParseColumnValue(dbResponse, "ClubLogo").ToString(),
                    ClubNameEng = _dao.ParseColumnValue(dbResponse, "ClubNameEng").ToString(),
                    ClubNameJp = _dao.ParseColumnValue(dbResponse, "ClubNameJp").ToString(),
                    Price = _dao.ParseColumnValue(dbResponse, "Price").ToString(),
                    HostImages = _dao.ParseColumnValue(dbResponse, "HostImages").ToString(),
                    LocationName=_dao.ParseColumnValue(dbResponse, "LocationName").ToString()
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
    }
}

