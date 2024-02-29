using CRS.CUSTOMER.REPOSITORY.LocationManagement;
using CRS.CUSTOMER.SHARED.LocationManagement;
using CRS.CUSTOMER.SHARED.ReviewManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.LocationManagement
{
    public class LocationManagementBusiness : ILocationManagementBusiness
    {
        private readonly LocationManagementRepository _repo;

        public LocationManagementBusiness(LocationManagementRepository repo) => this._repo = repo;

        public ClubDetailCommon GetClubDetailById(string clubId, string CustomerId = "")
        {
            return _repo.GetClubDetailById(clubId, CustomerId);
        }

        public List<LocationManagementCommon> GetLocations()
        {
            return _repo.GetLocations();
        }
        #region Club/Host list via location
        public List<LocationClubListCommon> GetClubListViaLoaction(string LocationId, string customerId)
        {
            return _repo.GetClubListViaLoaction(LocationId, customerId);
        }

        public List<LocationHostListCommon> GetHostList(string LocationId, string ClubId = "", string customerId = "", string Type = "")
        {
            return _repo.GetHostList(LocationId, ClubId, customerId, Type);
        }
        #endregion
        #region "Club Reservation"
        public List<TimeIntervalListCommon> GetTimeInterval()
        {
            return _repo.GetTimeInterval();
        }
        #endregion
        public List<GetClubReviewsCommon> GetClubReviewAndRatings(string ClubId)
        {
            return _repo.GetClubReviewAndRatings(ClubId);
        }
        public List<string> GetClubGalleryImage(string ClubId, string SelectType = "")
        {
            return _repo.GetClubGalleryImage(ClubId, SelectType);
        }
        public List<ClubWeeklyScheduleCommon> GetClubReservationSchedule(string ClubId)
        {
            return _repo.GetClubReservationSchedule(ClubId);
        }
        #region Host view details
        public ViewHostDetailCommon ViewHostDetails(string HostId, string customerId)
        {
            return _repo.ViewHostDetails(HostId, customerId);
        }
        public ViewHostDetailCommonV2 ViewHostDetailsV2(string HostId, string customerId)
        {
            return _repo.ViewHostDetailsV2(HostId, customerId);
        }

        public List<NoticeModelCommon> GetNoticeByClubId(string cId)
        {
            return _repo.GetNoticeByClubId(cId);
        }

        public ClubBasicInformationModelCommon GetClubBasicInformation(string cId)
        {
            return _repo.GetClubBasicInformation(cId);
        }

        public List<AllNoticeModelCommon> GetAllNoticeTabList(string cId)
        {
            return _repo.GetAllNoticeTabList(cId);
        }

        public List<AllScheduleModelCommon> GetAllScheduleTabList(string cId, string sFD)
        {
            return _repo.GetAllScheduleTabList(cId, sFD);
        }
        #endregion
    }
}