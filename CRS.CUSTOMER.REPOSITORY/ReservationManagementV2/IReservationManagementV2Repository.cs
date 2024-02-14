using CRS.CUSTOMER.SHARED.ReservationManagementV2;

namespace CRS.CUSTOMER.REPOSITORY.ReservationManagementV2
{
    public interface IReservationManagementV2Repository
    {
        InitiateClubReservationCommon InitiateClubReservationProcess(string ClubId, string SelectedDate = "");
    }
}
