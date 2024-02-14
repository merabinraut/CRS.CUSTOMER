using System.Collections.Generic;
using System.Linq;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagementV2;

namespace CRS.CUSTOMER.REPOSITORY.ReservationManagementV2
{
    public class ReservationManagementV2Repository:IReservationManagementV2Repository
	{
		RepositoryDao _dao;
		public ReservationManagementV2Repository()
		{
			_dao = new RepositoryDao();
		}

        public List<ReservationHistoryV2ModelCommon> GetReservedList(string customerId)
        {
            string sp_name = "sproc_reservation_management_v2 @Flag='grhl'";
            sp_name += ",@CustomerId=" + _dao.FilterString(customerId);
            var dbResponseInfo = _dao.ExecuteDataTable(sp_name);
            if (dbResponseInfo != null && dbResponseInfo.Rows.Count > 0) return _dao.DataTableToListObject<ReservationHistoryV2ModelCommon>(dbResponseInfo).ToList();
            return new List<ReservationHistoryV2ModelCommon>();
        }

        public List<VisitedHistoryModelCommon> GetVisitedHistoryList(string customerId)
        {
            string sp_name = "sproc_reservation_management_v2 @Flag='gvhl'";
            sp_name += ",@CustomerId=" + _dao.FilterString(customerId);
            var dbResponseInfo = _dao.ExecuteDataTable(sp_name);
            if (dbResponseInfo != null && dbResponseInfo.Rows.Count > 0) return _dao.DataTableToListObject<VisitedHistoryModelCommon>(dbResponseInfo).ToList();
            return new List<VisitedHistoryModelCommon>();
        }
    }
}

