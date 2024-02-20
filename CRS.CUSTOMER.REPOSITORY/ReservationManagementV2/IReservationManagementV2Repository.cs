using System.Collections.Generic;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagementV2;

namespace CRS.CUSTOMER.REPOSITORY.ReservationManagementV2
{
    public interface IReservationManagementV2Repository
    {
        CommonDbResponse CancelReservation(Common commonDBRequest);
        CommonDbResponse DeleteReservation(Common commonDbRequest);
        List<AllHistoryModelCommon> GetAllHistoryList(string customerId);
        List<CancelledHistoryModelCommon> GetCancelledHistory(string customerId);
        ReservationHistoryDetailModelCommon GetReservationHistoryDetail(string customerId, string reservationId);
        List<ReservationHistoryV2ModelCommon> GetReservedList(string customerId);
        List<VisitedHistoryModelCommon> GetVisitedHistoryList(string customerId);
        CommonDbResponse RedoReservation(Common commonDbRequest);
        CommonDbResponse RescheduleReservation(Common commonDBRequest, string time);
    }
}

