using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.Dashboard;
using CRS.CUSTOMER.APPLICATION.Models.LocationManagement;
using CRS.CUSTOMER.BUSINESS.CommonManagement;
using CRS.CUSTOMER.BUSINESS.Dashboard;
using CRS.CUSTOMER.BUSINESS.LocationManagement;
using CRS.CUSTOMER.BUSINESS.ProfileManagement;
using CRS.CUSTOMER.BUSINESS.RecommendedClubHost;
using CRS.CUSTOMER.BUSINESS.ReservationManagement;
using CRS.CUSTOMER.BUSINESS.SearchFilterManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.RecommendedClubHost;
using CRS.CUSTOMER.SHARED.ReservationManagement;
using CRS.CUSTOMER.SHARED.SearchFilterManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class LocationManagementController : CustomController
    {
        private readonly ILocationManagementBusiness _business;
        private readonly IReservationManagementBusiness _reservationBuss;
        private readonly IProfileManagementBusiness _profileBuss;
        private readonly ISearchFilterManagementBusiness _searchBuss;
        private readonly IDashboardBusiness _dashboardBuss;
        private readonly IRecommendedClubHostBusiness _recommendedClubHostBuss;
        private readonly ICommonManagementBusiness _commonManagementBuss;
        private readonly Dictionary<string, string> _locationHelper = ApplicationUtilities.MapJsonDataToDictionaryViaKeyName("URLManagementConfigruation", "Location");

        public LocationManagementController(ILocationManagementBusiness business, IReservationManagementBusiness reservationBuss, IProfileManagementBusiness profileBuss
            , ISearchFilterManagementBusiness searchBuss, IDashboardBusiness dashboardBuss, IRecommendedClubHostBusiness recommendedClubHostBuss, ICommonManagementBusiness commonManagementBuss)
        {
            this._business = business;
            this._reservationBuss = reservationBuss;
            this._profileBuss = profileBuss;
            this._searchBuss = searchBuss;
            this._dashboardBuss = dashboardBuss;
            this._recommendedClubHostBuss = recommendedClubHostBuss;
            this._commonManagementBuss = commonManagementBuss;
        }
        [HttpGet]
        public ActionResult Index(LocationClubHostRequestModel RequestModel, string RenderId = "")
        {
            var locationId = ApplicationUtilities.GetKeyValueFromDictionary(_locationHelper, RequestModel.LocationId);
            ViewBag.ActionPageName = "Dashboard";
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
            //var id = !string.IsNullOrEmpty(RequestModel.LocationId) ? RequestModel.LocationId.DecryptParameter() : null;
            if (string.IsNullOrEmpty(locationId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = "Invalid location details",
                    //Title = NotificationMessage.INFORMATION.ToString()
                });
                return Redirect("/");
            }
            var agentId = ApplicationUtilities.GetSessionValue("AgentId")?.ToString()?.DecryptParameter();
            var Model = new LocationClubHostModel();
            var bannerServiceResp = _dashboardBuss.GetBanners();
            if (bannerServiceResp != null && bannerServiceResp.Count > 0)
            {
                Model.Banners = bannerServiceResp.MapObjects<BannersModel>();
                Model.Banners.ForEach(x => x.BannerId = x.BannerId?.EncryptParameter());
                Model.Banners.ForEach(x => x.BannerImage = ImageHelper.ProcessedImage(x.BannerImage));
            }

            var recommendedClubDBRequest = new RecommendedClubRequestCommon()
            {
                PositionId = RequestModel.GroupId.ToString(),
                LocationId = locationId,//!string.IsNullOrEmpty(RequestModel.LocationId) ? RequestModel.LocationId.DecryptParameter() : null,
                CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter()
            };

            var dbClubResponse = _recommendedClubHostBuss.GetRecommendedClub(recommendedClubDBRequest);
            Model.ClubListModel = dbClubResponse.MapObjects<LocationClubListModel>();
            foreach (var item in Model.ClubListModel)
            {
                item.ClubId = item.ClubId.EncryptParameter();
                item.LocationId = item.LocationId.EncryptParameter();
                item.ClubLogo = ImageHelper.ProcessedImage(item.ClubLogo);
                item.ClubCoverPhoto = ImageHelper.ProcessedImage(item.ClubCoverPhoto);
                //item.ClubWeeklyScheduleList.ForEach(x => x.DayLabel = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.EnglishDay : x.JapaneseDay);
                item.HostGalleryImage = item.HostGalleryImage.Select(x => ImageHelper.ProcessedImage(x)).ToList();
            }
            if (Model.ClubListModel != null && Model.ClubListModel.Count > 0)
            {
                var recommendedHostDBRequest = new RecommendedHostRequestCommon()
                {
                    PositionId = RequestModel.GroupId.ToString(),
                    LocationId = locationId,//!string.IsNullOrEmpty(RequestModel.LocationId) ? RequestModel.LocationId.DecryptParameter() : null,
                    CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter()
                };
                var dbHostResponse = _recommendedClubHostBuss.GetRecommendedHost(recommendedHostDBRequest);
                Model.HostListModel = dbHostResponse.MapObjects<LocationHostListModel>();
                foreach (var item in Model.HostListModel)
                {
                    item.ClubId = item.ClubId.EncryptParameter();
                    item.LocationId = item.LocationId.EncryptParameter();
                    item.HostId = item.HostId.EncryptParameter();
                    item.HostImage = ImageHelper.ProcessedImage(item.HostImage);
                    item.ClubLogo = ImageHelper.ProcessedImage(item.ClubLogo);
                }
                RequestModel.ClubId = recommendedHostDBRequest.ClubId?.EncryptParameter();
            }
            //List<Tuple<int, string>> tuples = new List<Tuple<int, string>>();
            //for (int i = 0; i < Model.ClubListModel.Count; i++)
            //{
            //    tuples.Add(new Tuple<int, string>(i + 1, Model.ClubListModel[i].ClubId));
            //}
            //ViewBag.TuplesJson = Newtonsoft.Json.JsonConvert.SerializeObject(tuples);
            ViewBag.LocationId = RequestModel.LocationId;
            var getTotalPage = _recommendedClubHostBuss.GetTotalRecommendedPageCount();
            ViewBag.TotalGroupCount = getTotalPage >= 0 ? getTotalPage : 0;
            Model.RequestModel = RequestModel.MapObject<LocationClubHostRequestModel>();

            ViewBag.RenderValue = !string.IsNullOrEmpty(RenderId) ? RenderId : null;
            if (RequestModel.GroupId > 1)
                ViewBag.TabActive = true;
            return View(Model);
        }

        #region Club Detail
        [HttpGet]
        public ActionResult ClubDetail_V2(string LocationId, string ClubId, string ScheduleFilterDate = null)
        {
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
            var cId = !string.IsNullOrEmpty(ClubId) ? ClubId.DecryptParameter() : null;
            var lId = !string.IsNullOrEmpty(LocationId) ? LocationId.DecryptParameter() : null;
            string sFD = null;//!string.IsNullOrEmpty(ScheduleFilterDate[0]) ? ScheduleFilterDate : null;
            if (ScheduleFilterDate != null)
            {
                //sFD = ScheduleFilterDate;
                DateTime date = DateTime.ParseExact(ScheduleFilterDate, "yyyy年 M月", null);
                sFD = date.ToString("yyyy/MM");
            }
            if (string.IsNullOrEmpty(cId) || string.IsNullOrEmpty(lId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.WARNING,
                    Message = "Invalid Details",
                    //Title = NotificationMessage.WARNING.ToString()
                });
                return Redirect("/");
            }
            string agentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();

            var clubDetailResp = _business.GetClubDetailById(cId, agentId);
            if (clubDetailResp != null && string.IsNullOrEmpty(clubDetailResp.ClubId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.WARNING,
                    Message = "こちらの店舗は現在ホスログに記載されておりません", //Club is no longer available in the hoslog,
                    //Title = NotificationMessage.WARNING.ToString()
                });
                return Redirect("/");
            }
            var responseModel = clubDetailResp.MapObject<ClubDetailModel>();
            responseModel.ClubId = responseModel.ClubId.EncryptParameter();
            responseModel.LocationId = responseModel.LocationId.EncryptParameter();
            var dbHostList = _business.GetHostList(lId, cId);
            responseModel.HostListModels = dbHostList.MapObjects<LocationHostListModel>();
            foreach (var item in responseModel.HostListModels)
            {
                item.ClubId = item.ClubId.EncryptParameter();
                item.HostId = item.HostId.EncryptParameter();
                item.LocationId = item.LocationId.EncryptParameter();
                item.HostImage = ImageHelper.ProcessedImage(item.HostImage);
            }
            var dbTopHostList = _business.GetHostList(lId, cId, agentId, "trhl");
            responseModel.TopHostListModels = dbTopHostList.MapObjects<LocationHostListModel>();
            foreach (var item in responseModel.TopHostListModels)
            {
                item.ClubId = item.ClubId.EncryptParameter();
                item.HostId = item.HostId.EncryptParameter();
                item.LocationId = item.LocationId.EncryptParameter();
                item.HostImage = ImageHelper.ProcessedImage(item.HostImage);
            }
            var clubGalleryImageDBResponse = _business.GetClubGalleryImage(responseModel.ClubId.DecryptParameter(), "A");
            if (clubGalleryImageDBResponse != null && clubGalleryImageDBResponse.Count > 0)
            {
                responseModel.ClubGalleryImageList = clubGalleryImageDBResponse;
                responseModel.ClubGalleryImageList.ForEach(x => x = ImageHelper.ProcessedImage(x));
            }
            else responseModel.ClubGalleryImageList = new List<string>();
            responseModel.ClubCoverPhoto = ImageHelper.ProcessedImage(responseModel.ClubCoverPhoto);
            responseModel.ClubLogo = ImageHelper.ProcessedImage(responseModel.ClubLogo);
            responseModel.ClubWeeklyScheduleList.ForEach(x => x.DayLabel = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.EnglishDay : x.JapaneseDay);
            var reviewDBResponse = _business.GetClubReviewAndRatings(cId);
            if (reviewDBResponse != null && reviewDBResponse.Count > 0)
            {
                responseModel.ClubReviewsModel = reviewDBResponse.MapObjects<GetClubReviewsModel>();
                foreach (var item in responseModel.ClubReviewsModel)
                {
                    if (!string.IsNullOrEmpty(item.CustomerImage))
                    {
                        item.CustomerImage = ImageHelper.ProcessedImage(item.CustomerImage);
                    }
                    else
                    {
                        item.CustomerImage = "";
                    }
                }
                foreach (var item in responseModel.ClubReviewsModel)
                {
                    item.GetClubReviewRemarkList.ForEach(x => x.Remark = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.EnglishRemark : x.JapaneseRemark);
                }
                foreach (var item in responseModel.ClubReviewsModel)
                {
                    foreach (var item_sec in item.GetClubReviewHostList)
                    {
                        if (!string.IsNullOrEmpty(item_sec.HostImage))
                        {
                            item_sec.HostImage = ImageHelper.ProcessedImage(item_sec.HostImage);
                        }
                        else
                        {
                            item_sec.HostImage = "";
                        }
                    }
                }
            }
            var dbNoticeResponseInfo = _business.GetNoticeByClubId(cId);
            responseModel.GetNoticeByClubId = dbNoticeResponseInfo.MapObjects<NoticeModel>();
            foreach (var notice_item in responseModel.GetNoticeByClubId)
            {
                DateTime date = DateTime.ParseExact(notice_item.EventDate, "yyyy年MM月dd日", CultureInfo.InvariantCulture);
                // Get the day name
                notice_item.Day = date.ToString("ddd");
            }
            var dbBasicInfoResponse = _business.GetClubBasicInformation(cId);
            responseModel.GetClubBasicInformation = dbBasicInfoResponse.MapObject<ClubBasicInformationModel>();
            if (!string.IsNullOrEmpty(responseModel.GetClubBasicInformation.InstagramLink) && responseModel.GetClubBasicInformation.InstagramLink != "#")
            {
                if (!responseModel.GetClubBasicInformation.InstagramLink.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) responseModel.GetClubBasicInformation.InstagramLink = "https://" + responseModel.GetClubBasicInformation.InstagramLink;
            }
            if (!string.IsNullOrEmpty(responseModel.GetClubBasicInformation.TwitterLink) && responseModel.GetClubBasicInformation.TwitterLink != "#")
            {
                if (!responseModel.GetClubBasicInformation.TwitterLink.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) responseModel.GetClubBasicInformation.TwitterLink = "https://" + responseModel.GetClubBasicInformation.TwitterLink;
            }
            if (!string.IsNullOrEmpty(responseModel.GetClubBasicInformation.TiktokLink) && responseModel.GetClubBasicInformation.TiktokLink != "#")
            {
                if (!responseModel.GetClubBasicInformation.TiktokLink.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) responseModel.GetClubBasicInformation.TiktokLink = "https://" + responseModel.GetClubBasicInformation.TiktokLink;
            }
            if (!string.IsNullOrEmpty(responseModel.GetClubBasicInformation.LineNumber) && responseModel.GetClubBasicInformation.LineNumber != "#")
            {
                if (!responseModel.GetClubBasicInformation.LineNumber.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) responseModel.GetClubBasicInformation.LineNumber = "https://" + responseModel.GetClubBasicInformation.LineNumber;
            }
            if (!string.IsNullOrEmpty(responseModel.GetClubBasicInformation.WebsiteLink) && responseModel.GetClubBasicInformation.WebsiteLink != "#")
            {
                if (!responseModel.GetClubBasicInformation.WebsiteLink.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) responseModel.GetClubBasicInformation.WebsiteLink = "https://" + responseModel.GetClubBasicInformation.WebsiteLink;
            }
            var dbAllNoticeResponse = _business.GetAllNoticeTabList(cId);
            responseModel.GetAllNoticeTabList = dbAllNoticeResponse.MapObjects<AllNoticeModel>();
            foreach (var allNotice_item in responseModel.GetAllNoticeTabList)
            {
                // Parse the date string using the specified format and culture
                DateTime date = DateTime.ParseExact(allNotice_item.EventDate, "yyyy年MM月dd日", CultureInfo.InvariantCulture);
                // Get the day name
                allNotice_item.DayName = date.ToString("ddd");

            }
            var dbScheduleResponse = _business.GetAllScheduleTabList(cId, sFD);
            responseModel.GetAllScheduleTabList = dbScheduleResponse.MapObjects<AllScheduleModel>();
            foreach (var item_schedule in responseModel.GetAllScheduleTabList)
            {
                DateTime date = DateTime.ParseExact(item_schedule.ScheduleDate, "yyyy年MM月dd日", null);
                string formattedDayOfWeek = date.ToString("dd");

                // Get the day name (e.g., "Sunday")
                string dayName = date.ToString("ddd");
                item_schedule.Day = formattedDayOfWeek;
                item_schedule.DayName = dayName;
                if (!string.IsNullOrEmpty(item_schedule.ScheduleImage))
                {
                    item_schedule.ScheduleImage = ImageHelper.ProcessedImage(item_schedule.ScheduleImage, false);
                }
                else
                {
                    item_schedule.ScheduleImage = "";
                }
            }
            responseModel.GetScheduleDDL = GetScheduleList();
            var dbPlanDetailRes = _business.GetPlanDetail(cId);
            responseModel.GetPlanDetailList = dbPlanDetailRes.MapObjects<PlanDetailModel>();
            var groupedResults = responseModel.GetPlanDetailList
    .GroupBy(planDetail => planDetail.PlanName)
    .Select(group => new
    {
        PlanName = group.Key,
        GetPlanGroupDetail = group.ToList()
    })
    .ToList();
            ViewBag.PlanGroup = groupedResults.MapObjects<PlanGroup>();
            ViewBag.PlanGroup1 = groupedResults.MapObjects<PlanGroup>();
            ViewBag.ActionPageName = "ClubHostDetailNavMenu";
            ViewBag.FileLocationPath = "";
            ViewBag.SFilterDate = ScheduleFilterDate;
            ViewBag.ClubId = ClubId;
            ViewBag.LocationId = LocationId;
            return View(responseModel);
        }

        private List<ScheduleDDLModel> GetScheduleList()
        {
            List<ScheduleDDLModel> scheduleList = new List<ScheduleDDLModel>();

            DateTime currentDate = DateTime.Now;
            DateTime endDate = currentDate.AddMonths(3);

            while (currentDate <= endDate)
            {
                scheduleList.Add(new ScheduleDDLModel
                {
                    Value = currentDate.ToString("yyyy年 M月", CultureInfo.InvariantCulture),
                    Text = currentDate.ToString("yyyy年 M月", CultureInfo.InvariantCulture)
                });

                currentDate = currentDate.AddMonths(1);
            }

            return scheduleList;
        }
        #endregion

        [HttpGet]
        public ActionResult ViewHostDetail(string HostId)
        {
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
            var hId = string.IsNullOrEmpty(HostId) ? string.Empty : HostId.DecryptParameter();
            if (string.IsNullOrEmpty(hId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = "Invalid Details",
                   // Title = NotificationMessage.INFORMATION.ToString()
                });
                return Redirect("/");
            }

            string agentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();
            var dbResponse = _business.ViewHostDetailsV2(hId, agentId);
            if (dbResponse != null)
            {
                var ResponseModel = dbResponse.MapObject<ViewHostDetailModelV2>();
                ResponseModel.ClubId = ResponseModel.ClubId.EncryptParameter();
                ResponseModel.HostId = ResponseModel.HostId.EncryptParameter();
                ResponseModel.LocationId = ResponseModel.LocationId.EncryptParameter();
                ResponseModel.ClubLogo = ImageHelper.ProcessedImage(ResponseModel.ClubLogo);
                if (!string.IsNullOrEmpty(ResponseModel.HostInstagramLink) && ResponseModel.HostInstagramLink != "#")
                {
                    if (!ResponseModel.HostInstagramLink.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) ResponseModel.HostInstagramLink = "https://" + ResponseModel.HostInstagramLink;
                }
                if (!string.IsNullOrEmpty(ResponseModel.HostTwitterLink) && ResponseModel.HostTwitterLink != "#")
                {
                    if (!ResponseModel.HostTwitterLink.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) ResponseModel.HostTwitterLink = "https://" + ResponseModel.HostTwitterLink;
                }
                if (!string.IsNullOrEmpty(ResponseModel.HostTiktokLink) && ResponseModel.HostTiktokLink != "#")
                {
                    if (!ResponseModel.HostTiktokLink.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) ResponseModel.HostTiktokLink = "https://" + ResponseModel.HostTiktokLink;
                }
                if (!string.IsNullOrEmpty(ResponseModel.HostLine) && ResponseModel.HostLine != "#")
                {
                    if (!ResponseModel.HostLine.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) ResponseModel.HostLine = "https://" + ResponseModel.HostLine;
                }
                for (int i = 0; i < ResponseModel.HostGalleryImageList.Count; i++) ResponseModel.HostGalleryImageList[i] = ImageHelper.ProcessedImage(ResponseModel.HostGalleryImageList[i]);
                ResponseModel.HostIdentityDetailsModel.ForEach(x => x.Label = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.LabelEnglish : x.LabelJapanese);
                return View(ResponseModel);
            }
            AddNotificationMessage(new NotificationModel()
            {
                NotificationType = NotificationMessage.INFORMATION,
                Message = "Something went wrong.",
                //Title = NotificationMessage.INFORMATION.ToString()
            });
            return Redirect("/");
        }
    }
}