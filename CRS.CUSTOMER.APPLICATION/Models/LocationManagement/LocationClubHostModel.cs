using CRS.CUSTOMER.APPLICATION.Models.Dashboard;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.LocationManagement
{
    public class LocationClubHostRequestModel
    {
        public string LocationId { get; set; }
        public int GroupId { get; set; } = 1;
        public int PageNo { get; set; } = 1;
        public string ClubId { get; set; } = string.Empty;
    }
    public class LocationClubHostModel
    {
        public LocationClubHostRequestModel RequestModel { get; set; } = new LocationClubHostRequestModel();
        public List<LocationClubListModel> ClubListModel { get; set; } = new List<LocationClubListModel>();
        public List<LocationHostListModel> HostListModel { get; set; } = new List<LocationHostListModel>();
        public List<BannersModel> Banners { get; set; } = new List<BannersModel>();
    }
    public class LocationClubListModel
    {
        public string ClubId { get; set; }
        public string LocationId { get; set; }
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
        public string ClubTimePeriod { get; set; }
        public string TodaysClubSchedule { get; set; }
        public string IsBookmarked { get; set; }
        public int TotalComment { get; set; }
        public int AverageRating { get; set; }
        public List<string> ClubGalleryImage { get; set; } = new List<string>();
        public List<string> HostGalleryImage { get; set; } = new List<string>();
        public List<ClubWeeklyScheduleModel> ClubWeeklyScheduleList { get; set; } = new List<ClubWeeklyScheduleModel>();
    }

    public class LocationHostListModel
    {
        public string LocationId { get; set; }
        public string ClubId { get; set; }
        public string HostId { get; set; }
        public string HostName { get; set; }
        public string Occupation { get; set; }
        public string Rank { get; set; }
        public string ClubName { get; set; }
        public string HostImage { get; set; }
        public string IsBookmarked { get; set; }
        public string ClubLogo { get; set; }
        public string HostPosition { get; set; }
        public string HostNameJapanese { get; set; }
        public string FormattedDOB { get; set; }
        public string HostHeight { get; set; }
        public string HostBirthPlace { get; set; }
        public string HostLoveCount { get; set; }
        public string HostBloodType { get; set; }
        public string HostConstellationGroup { get; set; }
    }

    public class ClubWeeklyScheduleModel
    {
        public string EnglishDay { get; set; }
        public string Date { get; set; }
        public string JapaneseDate { get; set; }
        public string DateLabel { get; set; }
        public string Schedule { get; set; }
        public string JapaneseDay { get; set; }
        public string DayLabel { get; set; }
        public string DateValue { get; set; }
        public string FullDate { get; set; }
    }
}