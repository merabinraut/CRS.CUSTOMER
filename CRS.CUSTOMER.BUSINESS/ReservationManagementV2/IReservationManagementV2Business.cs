﻿using System.Collections.Generic;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagementV2;

namespace CRS.CUSTOMER.BUSINESS.ReservationManagementV2
{
    public interface IReservationManagementV2Business
    {
        List<AllHistoryModelCommon> GetAllHistoryList(string customerId);
        List<CancelledHistoryModelCommon> GetCancelledHistory(string customerId);
        ReservationHistoryDetailModelCommon GetReservationHistoryDetail(string customerId, string reservationId);
        List<ReservationHistoryV2ModelCommon> GetReservedList(string customerId);
        List<VisitedHistoryModelCommon> GetVisitedHistoryList(string customerId);
        CommonDbResponse RescheduleReservation(Common commonDBRequest, string time);
    }
}

