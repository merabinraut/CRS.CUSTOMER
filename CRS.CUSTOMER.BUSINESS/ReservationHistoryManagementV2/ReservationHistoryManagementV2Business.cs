using System.Collections.Generic;
using CRS.CUSTOMER.BUSINESS.ReservationHistoryManagementV2;
using CRS.CUSTOMER.REPOSITORY.ReservationHistoryManagementV2;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagementV2;

namespace CRS.CUSTOMER.BUSINESS.ReservationHistoryManagementV2
{
    public class ReservationHistoryManagementV2Business: IReservationHistoryManagementV2Business
    {
		private readonly IReservationHistoryManagementV2Repository _repo;
		public ReservationHistoryManagementV2Business(ReservationHistoryManagementV2Repository repo)
		{
			_repo = repo;
		}

        public CommonDbResponse CancelReservation(Common commonDBRequest)
        {
            return _repo.CancelReservation(commonDBRequest);
        }

        public CommonDbResponse DeleteReservation(Common commonDbRequest)
        {
            return _repo.DeleteReservation(commonDbRequest);
        }

        public List<AllHistoryModelCommon> GetAllHistoryList(string customerId)
        {
            return _repo.GetAllHistoryList(customerId);
        }

        public List<CancelledHistoryModelCommon> GetCancelledHistory(string customerId)
        {
            return _repo.GetCancelledHistory(customerId);
        }

        public ReservationHistoryDetailModelCommon GetReservationHistoryDetail(string customerId, string reservationId)
        {
            return _repo.GetReservationHistoryDetail(customerId, reservationId);
        }

        public List<ReservationHistoryV2ModelCommon>GetReservedList(string customerId)
        {
			return _repo.GetReservedList(customerId);
        }

        public List<VisitedHistoryModelCommon> GetVisitedHistoryList(string customerId)
        {
            return _repo.GetVisitedHistoryList(customerId);
        }

        public CommonDbResponse RedoReservation(Common commonDbRequest)
        {
            return _repo.RedoReservation(commonDbRequest);
        }

        public CommonDbResponse RescheduleReservation(Common commonDBRequest, string time)
        {
            return _repo.RescheduleReservation(commonDBRequest, time);
        }
    }
}

