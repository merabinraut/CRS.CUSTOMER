using CRS.CUSTOMER.REPOSITORY.ReservationManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagement;
using CRS.CUSTOMER.SHARED.ReservationManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.ReservationManagement
{
    public class ReservationManagementBusiness : IReservationManagementBusiness
    {
        private readonly IReservationManagementRepository _repo;
        public ReservationManagementBusiness(ReservationManagementRepository repo)
        {
            _repo = repo;
        }
        #region Plan Details
        public List<ReservationPlanDetailCommon> GetPlanList(string PlanId = "", string CustomerId = "", string ClubId = "")
        {
            return _repo.GetPlanList(PlanId, CustomerId, ClubId);
        }
        #endregion
        #region Reservation Management
        public CommonDbResponse CreateReservation(CreateReservationDetailCommon Request)
        {
            return _repo.CreateReservation(Request);
        }
        public CommonDbResponse ManageReservation(ManageReservationDetailCommon Request)
        {
            return _repo.ManageReservation(Request);
        }
        public List<ReservationHistoryCommon> GetReservationHistory(string CustomerId, string ReservationId = "", string InvoiceId = "")
        {
            return _repo.GetReservationHistory(CustomerId, ReservationId, InvoiceId);
        }
        public ReservationDetailCommon GetReservationDetail(ManageReservationDetailCommon Request)
        {
            return _repo.GetReservationDetail(Request);
        }


        #endregion
        #region"Customer Reservation Detail"
        public CustomerReservationDetailModelCommmon GetCustomerReservationDetail(string reservationId, string CustomerId)
        {
            return _repo.GetCustomerReservationDetail(reservationId, CustomerId);
        }

        public CommonDbResponse CancelReservation(string reservationId, string actionUser, string actionIp, string actionPlatform)
        {
            return _repo.CancelReservation(reservationId, actionUser, actionIp, actionPlatform);
        }

        public CommonDbResponse UpdateReservationTime(string reservationId, string actionUser, string actionIp, string actionPlatform, string visitedTime)
        {
            return _repo.UpdateReservationTime(reservationId, actionUser, actionIp, actionPlatform, visitedTime);
        }
        #endregion
        public List<ReservationHostDetail> GetReservationHostDetails(string ReservationId)
        {
            return _repo.GetReservationHostDetails(ReservationId);
        }

        public List<ReservationHostDetail> GetHostDetailsForReservation(string HostListId)
        {
            return _repo.GetHostDetailsForReservation(HostListId);
        }
    }
}
