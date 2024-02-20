using System.Collections.Generic;
using CRS.CUSTOMER.REPOSITORY.ReservationManagementV2;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagementV2;

namespace CRS.CUSTOMER.BUSINESS.ReservationManagementV2
{
    public class ReservationManagementV2Business:IReservationManagementV2Business
	{
		private readonly IReservationManagementV2Repository _repo;
		public ReservationManagementV2Business(ReservationManagementV2Repository repo)
		{
			_repo = repo;
		}

        public CommonDbResponse CancelReservation(Common commonDBRequest)
        {
            return _repo.CancelReservation(commonDBRequest);
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

