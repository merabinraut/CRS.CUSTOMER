using CRS.CUSTOMER.SHARED.LocationManagement;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.LocationManagement
{
    public class ClubDetailModel
    {
        public string ClubId { get; set; }
        public string LocationId { get; set; }
        public string ClubNameEng { get; set; }
        public string ClubNameJp { get; set; }
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
        public string ClubMobileNumber { get; set; }
        public string ClubLocation { get; set; }
        public string WebsiteLink { get; set; }
        public string TiktokLink { get; set; }
        public string TwitterLink { get; set; }
        public string InstagramLink { get; set; }
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
        public string LocationURL { get; set; }
        public string IsBookmarked { get; set; }
        public int TotalComment { get; set; }
        public int AverageRating { get; set; }
        public string ClubRegularPrice { get; set; }
        public string ClubNominationFee { get; set; }
        public string ClubAccompanyingFee { get; set; }
        public string ClubOnSiteNominationFee { get; set; }
        public string ClubVariousDrinksFee { get; set; }
        public string ClubTax { get; set; }
        public string ClubLastOrderTime { get; set; }
        public string ClubLastEntrySyokai { get; set; }
        public string ClubHoliday { get; set; }
        public List<LocationHostListModel> HostListModels { get; set; } = new List<LocationHostListModel>();
        public List<LocationHostListModel> TopHostListModels { get; set; } = new List<LocationHostListModel>();
        public List<string> ClubGalleryImageList { get; set; } = new List<string>();
        public List<GetClubReviewsModel> ClubReviewsModel { get; set; } = new List<GetClubReviewsModel>();
        public List<ClubWeeklyScheduleModel> ClubWeeklyScheduleList { get; set; } = new List<ClubWeeklyScheduleModel>();
        public List<ClubEventModel> ClubEventList { get; set; } = new List<ClubEventModel>();
        public List<NoticeModel> GetNoticeByClubId { get; set; }
    }
    public class ClubEventModel
    {
        public string EventDate { get; set; }
        public string EventDescription { get; set; }
    }
    public class NoticeModel
    {
        public string ClubId { get; set; }
        public string NoticeDate { get; set; }
        public string NoticeDescription { get; set; }
    }
}