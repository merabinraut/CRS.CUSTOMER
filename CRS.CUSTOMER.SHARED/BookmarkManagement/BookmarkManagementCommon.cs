using CRS.CUSTOMER.SHARED.LocationManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.SHARED.BookmarkManagement
{
    public class BookmarkedClubCommon
    {
        public string ClubId { get; set; }
        public string ClubCode { get; set; }
        public string LocationId { get; set; }
        public string LocationURL { get; set; }
        public string ClubName { get; set; }
        public string ClubNameJapanese { get; set; }
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
        public string IsBookmarked { get; set; }
        public int TotalComment { get; set; }
        public string ClubCategory { get; set; }
        public string Landline { get; set; }

        public int AverageRating { get; set; }
        public List<string> ClubGalleryImage { get; set; } = new List<string>();
        public List<string> HostGalleryImage { get; set; } = new List<string>();
        public List<ClubWeeklyScheduleCommon> ClubWeeklyScheduleList { get; set; } = new List<ClubWeeklyScheduleCommon>();
    }
    public class BookmarkedHostCommon
    {
        public string LocationId { get; set; }
        public string LocationURL { get; set; }
        public string ClubId { get; set; }
        public string ClubCode { get; set; }
        public string HostId { get; set; }
        public string HostCode { get; set; }
        public string HostName { get; set; }
        public string PreviousOccupation { get; set; }
        public string Rank { get; set; }
        public string ClubName { get; set; }
        public string ClubNameJap { get; set; }

        public string ClubLogo { get; set; }
        public string HostImage { get; set; }
        public string ConstellationGroup { get; set; }
        public string Height { get; set; }
        public string BloodType { get; set; }
        public string LiquorStrength { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string LoveCount { get; set; }
        public string HostNameJapanese { get; set; }
        public string FormattedDOB { get; set; }
    }

    public class GalleryImageCommon
    {
        public string ImagePath { get; set; }
    }
    public class ClubHostManagementCommon : Common
    {
        public string AgentType { get; set; }
        public string ClubId { get; set; }
        public string HostId { get; set; }
        public string Status { get; set; }
    }
}