using System.Collections.Generic;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagementV2;

namespace CRS.CUSTOMER.BUSINESS.ReservationManagementV2
{
    public interface IReservationManagementV2Business
    {
        List<AllHistoryModelCommon> GetAllHistoryList(string customerId);
        List<CancelledHistoryModelCommon> GetCancelledHistory(string customerId);
        List<ReservationHistoryV2ModelCommon> GetReservedList(string customerId);
        List<VisitedHistoryModelCommon> GetVisitedHistoryList(string customerId);
    }
}

