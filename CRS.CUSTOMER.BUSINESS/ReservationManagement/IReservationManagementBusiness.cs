using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagement;
using CRS.CUSTOMER.SHARED.ReservationManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.ReservationManagement
{
    public interface IReservationManagementBusiness
    {
        #region Plan Details
        List<ReservationPlanDetailCommon> GetPlanList(string PlanId = "", string CustomerId = "", string ClubId = "");
        #endregion
        #region Reservation Management
        CommonDbResponse CreateReservation(CreateReservationDetailCommon Request);
        CommonDbResponse ManageReservation(ManageReservationDetailCommon Request);
        List<ReservationHistoryCommon> GetReservationHistory(string CustomerId, string ReservationId = "", string InvoiceId = "");
        ReservationDetailCommon GetReservationDetail(ManageReservationDetailCommon Request);

        #endregion
        #region "Customer Reservation Detail"
        CustomerReservationDetailModelCommmon GetCustomerReservationDetail(string reservationId, string CustomerId);
        CommonDbResponse CancelReservation(string reservationId, string actionUser, string actionIp, string actionPlatform);
        CommonDbResponse UpdateReservationTime(string reservationId, string actionUser, string actionIp, string actionPlatform, string visitedTime);
        #endregion
        List<ReservationHostDetail> GetReservationHostDetails(string ReservationId);
        List<ReservationHostDetail> GetHostDetailsForReservation(string HostListId);
    }
}
