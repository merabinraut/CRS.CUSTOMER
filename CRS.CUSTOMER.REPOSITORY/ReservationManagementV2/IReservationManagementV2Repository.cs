using System.Collections.Generic;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagementV2;

namespace CRS.CUSTOMER.REPOSITORY.ReservationManagementV2
{
    public interface IReservationManagementV2Repository
    {
        List<AllHistoryModelCommon> GetAllHistoryList(string customerId);
        List<CancelledHistoryModelCommon> GetCancelledHistory(string customerId);
        List<ReservationHistoryV2ModelCommon> GetReservedList(string customerId);
        List<VisitedHistoryModelCommon> GetVisitedHistoryList(string customerId);
    }
}

