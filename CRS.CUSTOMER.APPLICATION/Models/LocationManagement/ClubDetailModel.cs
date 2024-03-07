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
        public ClubBasicInformationModel GetClubBasicInformation { get; set; }
        public List<AllNoticeModel> GetAllNoticeTabList { get; set; }
        public List<AllScheduleModel> GetAllScheduleTabList { get; set; }
        public List<ScheduleDDLModel> GetScheduleDDL { get; set; }
        public List<PlanDetailModel> GetPlanDetailList { get; set; }
        public List<PlanGroup> GetPlanGroup { get; set; }
    }
    public class ClubEventModel
    {
        public string EventDate { get; set; }
        public string EventDescription { get; set; }
    }
    public class NoticeModel
    {
        public string AgentId { get; set; }
        public string EventId { get; set; }
        public string EventDate { get; set; }
        public string EventDescription { get; set; }
        public string EventType { get; set; }
        public string EventTitle { get; set; }
        public string Day { get; set; }
    }
    public class ClubBasicInformationModel
    {
        public string ClubAddress { get; set; }
        public string ClubOpeningTime { get; set; }
        public string ClubClosingTime { get; set; }
        public string LastEntryTime { get; set; }
        public string Holiday { get; set; }
        public string TiktokLink { get; set; }
        public string InstagramLink { get; set; }
        public string LineNumber { get; set; }
        public string TwitterLink { get; set; }
        public string RegularEntry { get; set; }
        public string EnjoyPlan { get; set; }
        public string GroupPlan { get; set; }
        public string EPLastEntryTime { get; set; }
        public string EpMaxReservation { get; set; }
        public string GPLastEntryTime { get; set; }
        public string GPMaxReservation { get; set; }
        public string VPLastEntryTiime { get; set; }
        public string VPMaxReservation { get; set; }
        public string RegularPrice { get; set; }
        public string DesignationFee { get; set; }
        public string CompanionFee { get; set; }
        public string ExtensionFee { get; set; }
        public string Drink { get; set; }
        public string Tax { get; set; }
        public string LastOrderTime { get; set; }
    }
    public class AllNoticeModel : NoticeModel
    {
        public string EventStatus { get; set; }
        public string DayName { get; set; }
    }
    public class AllScheduleModel
    {
        public string ScheduleId { get; set; }
        public string ScheduleTitle { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleDescription { get; set; }
        public string EventType { get; set; }
        public string AgentId { get; set; }
        public string ScheduleImage { get; set; }
        public string Day { get; set; }
        public string DayName { get; set; }
    }
    public class ScheduleDDLModel
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class PlanGroup
    {
        public string PlanName { get; set; }
        public List<PlanDetailModel> GetPlanGroupDetail { get; set; }
    }
    public class PlanDetailModel
    {
        public string Label { get; set; }
        public string LabelValue { get; set; }
        public string PlanName { get; set; }
        public string ClubPlanTypeId { get; set; }
        public string PlanListId { get; set; }
        public string ClubId { get; set; }
    }
}