using CRS.CUSTOMER.APPLICATION.Models.LocationManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.BookmarkManagement
{
    public class BookmarkManagementModel
    {
        public List<BookmarkedClubModel> BookmarkedClubs { get; set; }
        public List<BookmarkedHostModel> BookmarkedHosts { get; set; }
    }

    public class BookmarkedClubModel
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
        public List<ClubWeeklyScheduleModel> ClubWeeklyScheduleList { get; set; }

    }
    public class BookmarkedHostModel
    {
        public string LocationId { get; set; }
        public string ClubId { get; set; }
        public string HostId { get; set; }
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
    }

    public class GalleryImageModel
    {
        public string ImagePath { get; set; }
    }
    public class ClubHostManagementModel
    {
        public string AgentId { get; set; }
        public string AgentType { get; set; }
        public string ClubId { get; set; }
        public string HostId { get; set; }
        public string Status { get; set; }
    }
}