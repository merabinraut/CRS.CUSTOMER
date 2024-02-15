using System.Collections.Generic;
using CRS.CUSTOMER.REPOSITORY.ReservationManagementV2;
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

        public List<AllHistoryModelCommon> GetAllHistoryList(string customerId)
        {
            return _repo.GetAllHistoryList(customerId);
        }

        public List<CancelledHistoryModelCommon> GetCancelledHistory(string customerId)
        {
            return _repo.GetCancelledHistory(customerId);
        }

        public List<ReservationHistoryV2ModelCommon>GetReservedList(string customerId)
        {
			return _repo.GetReservedList(customerId);
        }

        public List<VisitedHistoryModelCommon> GetVisitedHistoryList(string customerId)
        {
            return _repo.GetVisitedHistoryList(customerId);
        }
    }
}

