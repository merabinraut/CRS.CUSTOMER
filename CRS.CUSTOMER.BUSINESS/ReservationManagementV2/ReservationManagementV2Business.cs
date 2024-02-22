using CRS.CUSTOMER.REPOSITORY.ReservationManagementV2;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReservationManagementV2;
using System.Collections.Generic;
using System;

namespace CRS.CUSTOMER.BUSINESS.ReservationManagementV2
{
    public class ReservationManagementV2Business : IReservationManagementV2Business
    {
        private readonly IReservationManagementV2Repository _repo;
        public ReservationManagementV2Business(ReservationManagementV2Repository repo)
        {
            _repo = repo;
        }

        #region InitiateClubReservationProcess
        public InitiateClubReservationCommon InitiateClubReservationProcess(string ClubId, string SelectedDate = "")
        {
            return _repo.InitiateClubReservationProcess(ClubId, SelectedDate);
        }
        #endregion

        #region  Verify club and get club details
        public Tuple<ResponseCode, string, ClubBasicDetailCommon> VerifyAndGetClubBasicDetail(string ClubId)
        {
            return _repo.VerifyAndGetClubBasicDetail(ClubId);
        }
        #endregion

        #region check if the customer can proceed with the reservation process
        public Tuple<ResponseCode, string> IsReservationProcessValid(string ClubId, string CustomerId, string SelectedDate, string SelectedTime, string NoOfPeople)
        {
            return _repo.IsReservationProcessValid(ClubId, CustomerId, SelectedDate, SelectedTime, NoOfPeople);
        }
        #endregion

        #region Plan 
        public Tuple<ResponseCode, string, List<PlanV2Common>> GetPlans(string ClubId, string CustomerId)
        {
            return _repo.GetPlans(ClubId, CustomerId);
        }
        #endregion

        #region Host
        public List<HostListV2Common> GetHostList(string ClubId)
        {
            return _repo.GetHostList(ClubId);
        }
        #endregion

        #region Get host details by club and host id
        public List<HostListV2Common> GetSelectedHostDetail(string ClubId, string HostListId)
        {
            return _repo.GetSelectedHostDetail(ClubId, HostListId);
        }
        #endregion

        #region get customer reservation billing details
        public Tuple<ResponseCode, string, BillingResponseCommon> ReservationBillingDetail(BillingRequestModel Request)
        {
            return _repo.ReservationBillingDetail(Request);
        }
        #endregion

        #region Reservation Confirmation
        public CommonDbResponse ReservationConfirmation(ReservationConfirmationRequestCommon Request)
        {
            return _repo.ReservationConfirmation(Request);
        }
        #endregion
    }
}
