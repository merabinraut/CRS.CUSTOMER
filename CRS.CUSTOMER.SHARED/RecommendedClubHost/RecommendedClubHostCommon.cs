using CRS.CUSTOMER.SHARED.LocationManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.SHARED.RecommendedClubHost
{
    #region Club
    public class RecommendedClubRequestCommon
    {
        public string PositionId { get; set; }
        public string LocationId { get; set; }
        public string CustomerId { get; set; }
        public string PageType { get; set; }
    }

    public class RecommendedClubResponseCommon
    {
        public string ClubId { get; set; }
        public string ClubCode { get; set; }
        public string LocationId { get; set; }
        public string LocationURL { get; set; }
        public string ClubName { get; set; }
        public string GroupName { get; set; }
        public string FullName { get; set; }
        public string ClubLogo { get; set; }
        public string ClubCoverPhoto { get; set; }
        public string ClubDescription { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
        public string ClubOpeningTime { get; set; }
        public string ClubClosingTime { get; set; }
        public string TodaysClubSchedule { get; set; }
        public string ClubTimePeriod { get; set; }
        public string ClubNameJapanese { get; set; }
        public string IsBookmarked { get; set; }
        public int TotalComment { get; set; }
        public int AverageRating { get; set; }
        public List<string> ClubGalleryImage { get; set; } = new List<string>();
        public List<string> HostGalleryImage { get; set; } = new List<string>();
        public List<ClubWeeklyScheduleCommon> ClubWeeklyScheduleList { get; set; } = new List<ClubWeeklyScheduleCommon>();
    }
    #endregion

    #region Host
    public class RecommendedHostRequestCommon
    {
        public string PositionId { get; set; }
        public string LocationId { get; set; }
        public string CustomerId { get; set; }
        public string ClubId { get; set; }
        public string PageType { get; set; }
    }
    public class RecommendedHostResponseCommon
    {
        public string LocationId { get; set; }
        public string LocationURL { get; set; }
        public string ClubId { get; set; }
        public string ClubCode { get; set; }
        public string HostId { get; set; }
        public string HostCode { get; set; }
        public string HostName { get; set; }
        public string HostNameJapanese { get; set; }
        public string Occupation { get; set; }
        public string Rank { get; set; }
        public string ClubName { get; set; }
        public string ClubNameJapanese { get; set; }
        public string HostImage { get; set; }
        public string IsBookmarked { get; set; }
        public string ClubLogo { get; set; }
        public string HostPosition { get; set; }
        public string FormattedDOB { get; set; }
        public string HostHeight { get; set; }
        public string HostBirthPlace { get; set; }
        public string HostLoveCount { get; set; }
        public string HostBloodType { get; set; }
        public string HostConstellationGroup { get; set; }
        public string LiquorStrength { get; set; }
    }

    #endregion
}
