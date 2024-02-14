using CRS.CUSTOMER.SHARED.ReservationManagementV2;

namespace CRS.CUSTOMER.BUSINESS.ReservationManagementV2
{
    public interface IReservationManagementV2Business
    {
        InitiateClubReservationCommon InitiateClubReservationProcess(string ClubId, string SelectedDate = "");
    }
}
