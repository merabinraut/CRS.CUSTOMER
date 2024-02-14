using CRS.CUSTOMER.REPOSITORY.ReservationManagementV2;
using CRS.CUSTOMER.SHARED.ReservationManagementV2;

namespace CRS.CUSTOMER.BUSINESS.ReservationManagementV2
{
    public class ReservationManagementV2Business : IReservationManagementV2Business
    {
        private readonly IReservationManagementV2Repository _repo;
        public ReservationManagementV2Business(ReservationManagementV2Repository repo)
        {
            _repo = repo;
        }
        public InitiateClubReservationCommon InitiateClubReservationProcess(string ClubId, string SelectedDate = "")
        {
            return _repo.InitiateClubReservationProcess(ClubId, SelectedDate);
        }
    }
}
