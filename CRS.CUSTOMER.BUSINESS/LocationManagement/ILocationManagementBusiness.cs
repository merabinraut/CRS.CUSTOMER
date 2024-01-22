using CRS.CUSTOMER.SHARED.LocationManagement;
using CRS.CUSTOMER.SHARED.ReviewManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.LocationManagement
{
    public interface ILocationManagementBusiness
    {
        List<LocationManagementCommon> GetLocations();

        ClubDetailCommon GetClubDetailById(string clubId, string CustomerId = "");

        #region "Club Reservation"
        List<TimeIntervalListCommon> GetTimeInterval();
        #endregion
        #region Club/Host list via location
        List<LocationClubListCommon> GetClubListViaLoaction(string LocationId, string customerId);

        List<LocationHostListCommon> GetHostList(string LocationId, string ClubId = "", string customerId = "", string Type = "");
        List<GetClubReviewsCommon> GetClubReviewAndRatings(string ClubId);
        #endregion
        List<string> GetClubGalleryImage(string ClubId, string SelectType = "");
        List<ClubWeeklyScheduleCommon> GetClubReservationSchedule(string ClubId);
        #region Host view details
        ViewHostDetailCommon ViewHostDetails(string HostId, string customerId);
        #endregion
    }
}