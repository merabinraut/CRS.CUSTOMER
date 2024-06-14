using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReservationManagementV2;
using System.Collections.Generic;
using System;

namespace CRS.CUSTOMER.BUSINESS.ReservationManagementV2
{
    public interface IReservationManagementV2Business
    {
        #region InitiateClubReservationProcess
        InitiateClubReservationCommon InitiateClubReservationProcess(string ClubId, string SelectedDate = "");
        #endregion

        #region  Verify club and get club details
        Tuple<ResponseCode, string, ClubBasicDetailCommon> VerifyAndGetClubBasicDetail(string ClubId);
        #endregion

        #region check if the customer can proceed with the reservation process
        Tuple<ResponseCode, string> IsReservationProcessValid(string ClubId, string CustomerId, string SelectedDate, string SelectedTime, string NoOfPeople);
        #endregion

        #region Plan 
        Tuple<ResponseCode, string, List<PlanV2Common>> GetPlans(string ClubId, string CustomerId, string SelectedDate, string SelectedTime);
        #endregion

        #region Host
        List<HostListV2Common> GetHostList(string ClubId);
        #endregion

        #region Get host details by club and host id
        List<HostListV2Common> GetSelectedHostDetail(string ClubId, string HostListId);
        #endregion

        #region get customer reservation billing details
        Tuple<ResponseCode, string, BillingResponseCommon> ReservationBillingDetail(BillingRequestModel Request);
        #endregion

        #region Reservation Confirmation
        CommonDbResponse ReservationConfirmation(ReservationConfirmationRequestCommon Request);
        #endregion
    }
}
