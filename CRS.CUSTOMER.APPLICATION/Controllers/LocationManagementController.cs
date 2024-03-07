using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.Dashboard;
using CRS.CUSTOMER.APPLICATION.Models.LocationManagement;
using CRS.CUSTOMER.APPLICATION.Models.ReservationHistory;
using CRS.CUSTOMER.APPLICATION.Models.ReservationManagement;
using CRS.CUSTOMER.APPLICATION.Models.SearchFilterManagement;
using CRS.CUSTOMER.APPLICATION.Models.UserProfileManagement;
using CRS.CUSTOMER.BUSINESS.CommonManagement;
using CRS.CUSTOMER.BUSINESS.Dashboard;
using CRS.CUSTOMER.BUSINESS.LocationManagement;
using CRS.CUSTOMER.BUSINESS.ProfileManagement;
using CRS.CUSTOMER.BUSINESS.RecommendedClubHost;
using CRS.CUSTOMER.BUSINESS.ReservationManagement;
using CRS.CUSTOMER.BUSINESS.SearchFilterManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ProfileManagement;
using CRS.CUSTOMER.SHARED.RecommendedClubHost;
using CRS.CUSTOMER.SHARED.ReservationManagement;
using CRS.CUSTOMER.SHARED.SearchFilterManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        public ActionResult Index(LocationClubHostRequestModel RequestModel, string RenderId = "")
        {
            ViewBag.ActionPageName = "Dashboard";
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
            var FileLocationPath = "";
            if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT")
                FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
            var id = !string.IsNullOrEmpty(RequestModel.LocationId) ? RequestModel.LocationId.DecryptParameter() : null;
            if (string.IsNullOrEmpty(id))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = "Invalid location details",
                    Title = NotificationMessage.INFORMATION.ToString()
                });
                return RedirectToAction("Index", "DashboardV2");
            }
            var agentId = ApplicationUtilities.GetSessionValue("AgentId")?.ToString()?.DecryptParameter();
            var Model = new LocationClubHostModel();
            var bannerServiceResp = _dashboardBuss.GetBanners();
            if (bannerServiceResp != null && bannerServiceResp.Count > 0)
            {
                Model.Banners = bannerServiceResp.MapObjects<BannersModel>();
                Model.Banners.ForEach(x => x.BannerId = x.BannerId?.EncryptParameter());
                Model.Banners.ForEach(x => x.BannerImage = FileLocationPath + x.BannerImage);
            }

            var recommendedClubDBRequest = new RecommendedClubRequestCommon()
            {
                PositionId = RequestModel.GroupId.ToString(),
                LocationId = !string.IsNullOrEmpty(RequestModel.LocationId) ? RequestModel.LocationId.DecryptParameter() : null,
                CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter()
            };

            var dbClubResponse = _recommendedClubHostBuss.GetRecommendedClub(recommendedClubDBRequest);
            Model.ClubListModel = dbClubResponse.MapObjects<LocationClubListModel>();
            foreach (var item in Model.ClubListModel)
            {
                item.ClubId = item.ClubId.EncryptParameter();
                item.LocationId = item.LocationId.EncryptParameter();
                item.ClubLogo = FileLocationPath + item.ClubLogo;
                item.ClubCoverPhoto = FileLocationPath + item.ClubCoverPhoto;
                //item.ClubWeeklyScheduleList.ForEach(x => x.DayLabel = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.EnglishDay : x.JapaneseDay);
                item.HostGalleryImage = item.HostGalleryImage.Select(x => FileLocationPath + x).ToList();
            }
            if (Model.ClubListModel != null && Model.ClubListModel.Count > 0)
            {
                var recommendedHostDBRequest = new RecommendedHostRequestCommon()
                {
                    PositionId = RequestModel.GroupId.ToString(),
                    LocationId = !string.IsNullOrEmpty(RequestModel.LocationId) ? RequestModel.LocationId.DecryptParameter() : null,
                    CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter()
                };
                var dbHostResponse = _recommendedClubHostBuss.GetRecommendedHost(recommendedHostDBRequest);
                Model.HostListModel = dbHostResponse.MapObjects<LocationHostListModel>();
                foreach (var item in Model.HostListModel)
                {
                    item.ClubId = item.ClubId.EncryptParameter();
                    item.LocationId = item.LocationId.EncryptParameter();
                    item.HostId = item.HostId.EncryptParameter();
                    item.HostImage = FileLocationPath + item.HostImage;
                    item.ClubLogo = FileLocationPath + item.ClubLogo;
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
            return View(Model);
        }

        #region Club Detail
        //public ActionResult ClubDetail(string LocationId, string ClubId)
        //{
        //    var culture = Request.Cookies["culture"]?.Value;
        //    culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
        //    var FileLocationPath = "";
        //    var cId = !string.IsNullOrEmpty(ClubId) ? ClubId.DecryptParameter() : null;
        //    var lId = !string.IsNullOrEmpty(LocationId) ? LocationId.DecryptParameter() : null;
        //    if (string.IsNullOrEmpty(cId) || string.IsNullOrEmpty(lId))
        //    {
        //        AddNotificationMessage(new NotificationModel()
        //        {
        //            NotificationType = NotificationMessage.WARNING,
        //            Message = "Invalid Details",
        //            Title = NotificationMessage.WARNING.ToString()
        //        });
        //        return RedirectToAction("Index", "DashboardV2");
        //    }
        //    if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT") FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();

        //    string agentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();

        //    var clubDetailResp = _business.GetClubDetailById(cId, agentId);
        //    var responseModel = clubDetailResp.MapObject<ClubDetailModel>();
        //    responseModel.ClubId = responseModel.ClubId.EncryptParameter();
        //    responseModel.LocationId = responseModel.LocationId.EncryptParameter();
        //    var dbHostList = _business.GetHostList(lId, cId);
        //    responseModel.HostListModels = dbHostList.MapObjects<LocationHostListModel>();
        //    foreach (var item in responseModel.HostListModels)
        //    {
        //        item.ClubId = item.ClubId.EncryptParameter();
        //        item.HostId = item.HostId.EncryptParameter();
        //        item.LocationId = item.LocationId.EncryptParameter();
        //        item.HostImage = FileLocationPath + item.HostImage;
        //    }
        //    var dbTopHostList = _business.GetHostList(lId, cId, "", "trhl");
        //    responseModel.TopHostListModels = dbTopHostList.MapObjects<LocationHostListModel>();
        //    foreach (var item in responseModel.TopHostListModels)
        //    {
        //        item.ClubId = item.ClubId.EncryptParameter();
        //        item.HostId = item.HostId.EncryptParameter();
        //        item.LocationId = item.LocationId.EncryptParameter();
        //        item.HostImage = FileLocationPath + item.HostImage;
        //    }
        //    var clubGalleryImageDBResponse = _business.GetClubGalleryImage(responseModel.ClubId.DecryptParameter(), "A");
        //    if (clubGalleryImageDBResponse != null && clubGalleryImageDBResponse.Count > 0)
        //    {
        //        responseModel.ClubGalleryImageList = clubGalleryImageDBResponse;
        //    }
        //    else responseModel.ClubGalleryImageList = new List<string>();
        //    responseModel.ClubCoverPhoto = FileLocationPath + responseModel.ClubCoverPhoto;
        //    responseModel.ClubLogo = FileLocationPath + responseModel.ClubLogo;
        //    responseModel.ClubWeeklyScheduleList.ForEach(x => x.DayLabel = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.EnglishDay : x.JapaneseDay);
        //    var reviewDBResponse = _business.GetClubReviewAndRatings(cId);
        //    if (reviewDBResponse != null && reviewDBResponse.Count > 0)
        //    {
        //        responseModel.ClubReviewsModel = reviewDBResponse.MapObjects<GetClubReviewsModel>();
        //        responseModel.ClubReviewsModel.ForEach(x => x.CustomerImage = FileLocationPath + x.CustomerImage);
        //        foreach (var item in responseModel.ClubReviewsModel)
        //        {
        //            item.GetClubReviewRemarkList.ForEach(x => x.Remark = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.EnglishRemark : x.JapaneseRemark);
        //        }
        //        foreach (var item in responseModel.ClubReviewsModel)
        //        {
        //            item.GetClubReviewHostList.ForEach(x => x.HostImage = FileLocationPath + x.HostImage);
        //        }
        //    }
        //    ViewBag.ActionPageName = "ClubHostDetailNavMenu";
        //    ViewBag.FileLocationPath = FileLocationPath;
        //    return View(responseModel);
        //}


        public ActionResult ClubDetail_V2(string LocationId, string ClubId, string[] ScheduleFilterDate = null)
        {
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
            var FileLocationPath = "";
            var cId = !string.IsNullOrEmpty(ClubId) ? ClubId.DecryptParameter() : null;
            var lId = !string.IsNullOrEmpty(LocationId) ? LocationId.DecryptParameter() : null;
            string sFD = null;//!string.IsNullOrEmpty(ScheduleFilterDate[0]) ? ScheduleFilterDate : null;
            if (ScheduleFilterDate != null)
            {
                sFD = ScheduleFilterDate[0].ToString();
            }
            if (string.IsNullOrEmpty(cId) || string.IsNullOrEmpty(lId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.WARNING,
                    Message = "Invalid Details",
                    Title = NotificationMessage.WARNING.ToString()
                });
                return RedirectToAction("Index", "DashboardV2");
            }
            if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT") FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();

            string agentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();

            var clubDetailResp = _business.GetClubDetailById(cId, agentId);
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
                item.HostImage = FileLocationPath + item.HostImage;
            }
            var dbTopHostList = _business.GetHostList(lId, cId, agentId, "trhl");
            responseModel.TopHostListModels = dbTopHostList.MapObjects<LocationHostListModel>();
            foreach (var item in responseModel.TopHostListModels)
            {
                item.ClubId = item.ClubId.EncryptParameter();
                item.HostId = item.HostId.EncryptParameter();
                item.LocationId = item.LocationId.EncryptParameter();
                item.HostImage = FileLocationPath + item.HostImage;
            }
            var clubGalleryImageDBResponse = _business.GetClubGalleryImage(responseModel.ClubId.DecryptParameter(), "A");
            if (clubGalleryImageDBResponse != null && clubGalleryImageDBResponse.Count > 0)
            {
                responseModel.ClubGalleryImageList = clubGalleryImageDBResponse;
            }
            else responseModel.ClubGalleryImageList = new List<string>();
            responseModel.ClubCoverPhoto = FileLocationPath + responseModel.ClubCoverPhoto;
            responseModel.ClubLogo = FileLocationPath + responseModel.ClubLogo;
            responseModel.ClubWeeklyScheduleList.ForEach(x => x.DayLabel = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.EnglishDay : x.JapaneseDay);
            var reviewDBResponse = _business.GetClubReviewAndRatings(cId);
            if (reviewDBResponse != null && reviewDBResponse.Count > 0)
            {
                responseModel.ClubReviewsModel = reviewDBResponse.MapObjects<GetClubReviewsModel>();
                responseModel.ClubReviewsModel.ForEach(x => x.CustomerImage = FileLocationPath + x.CustomerImage);
                foreach (var item in responseModel.ClubReviewsModel)
                {
                    item.GetClubReviewRemarkList.ForEach(x => x.Remark = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.EnglishRemark : x.JapaneseRemark);
                }
                foreach (var item in responseModel.ClubReviewsModel)
                {
                    item.GetClubReviewHostList.ForEach(x => x.HostImage = FileLocationPath + x.HostImage);
                }
            }
            var dbNoticeResponseInfo = _business.GetNoticeByClubId(cId);
            responseModel.GetNoticeByClubId = dbNoticeResponseInfo.MapObjects<NoticeModel>();
            var dbBasicInfoResponse = _business.GetClubBasicInformation(cId);
            responseModel.GetClubBasicInformation = dbBasicInfoResponse.MapObject<ClubBasicInformationModel>();
            var dbAllNoticeResponse = _business.GetAllNoticeTabList(cId);
            responseModel.GetAllNoticeTabList = dbAllNoticeResponse.MapObjects<AllNoticeModel>();
            var dbScheduleResponse = _business.GetAllScheduleTabList(cId, sFD);
            responseModel.GetAllScheduleTabList = dbScheduleResponse.MapObjects<AllScheduleModel>();
            foreach (var item_schedule in responseModel.GetAllScheduleTabList)
            {
                DateTime date = DateTime.ParseExact(item_schedule.ScheduleDate, "yyyy年MM月dd日", null);
                string formattedDayOfWeek = date.ToString("dd");

                // Get the day name (e.g., "Sunday")
                string dayName = date.ToString("dddd");
                item_schedule.Day = formattedDayOfWeek;
                item_schedule.DayName = dayName;
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
            ViewBag.FileLocationPath = FileLocationPath;
            ViewBag.SFilterDate = ScheduleFilterDate;
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
            var FileLocationPath = "";
            var hId = string.IsNullOrEmpty(HostId) ? string.Empty : HostId.DecryptParameter();
            if (string.IsNullOrEmpty(hId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.INFORMATION,
                    Message = "Invalid Details",
                    Title = NotificationMessage.INFORMATION.ToString()
                });
                return RedirectToAction("Index", "DashboardV2");
            }
            if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT") FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
            string agentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();
            var dbResponse = _business.ViewHostDetailsV2(hId, agentId);
            if (dbResponse != null)
            {
                var ResponseModel = dbResponse.MapObject<ViewHostDetailModelV2>();
                ResponseModel.ClubId = ResponseModel.ClubId.EncryptParameter();
                ResponseModel.HostId = ResponseModel.HostId.EncryptParameter();
                ResponseModel.LocationId = ResponseModel.LocationId.EncryptParameter();
                ResponseModel.ClubLogo = FileLocationPath + ResponseModel.ClubLogo;
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
                for (int i = 0; i < ResponseModel.HostGalleryImageList.Count; i++) ResponseModel.HostGalleryImageList[i] = FileLocationPath + ResponseModel.HostGalleryImageList[i];
                ResponseModel.HostIdentityDetailsModel.ForEach(x => x.Label = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.LabelEnglish : x.LabelJapanese);
                return View(ResponseModel);
            }
            AddNotificationMessage(new NotificationModel()
            {
                NotificationType = NotificationMessage.INFORMATION,
                Message = "Something went wrong.",
                Title = NotificationMessage.INFORMATION.ToString()
            });
            return RedirectToAction("Index", "DashboardV2");
        }

        #region Club Reservation
        //public ActionResult ClubReservation(string ClubId, string SelectedDate = "", string SelectedHost = "")
        //{
        //    var culture = Request.Cookies["culture"]?.Value;
        //    culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
        //    var cId = !string.IsNullOrEmpty(ClubId) ? ClubId.DecryptParameter() : null;
        //    if (string.IsNullOrEmpty(cId))
        //    {
        //        AddNotificationMessage(new NotificationModel()
        //        {
        //            NotificationType = NotificationMessage.WARNING,
        //            Message = "Invalid Details",
        //            Title = NotificationMessage.WARNING.ToString()
        //        });
        //        return RedirectToAction("Index", "DashboardV2");
        //    }
        //    var clubDetailResponse = _business.GetClubDetailById(cId);
        //    var responseModel = new ClubReservationModel()
        //    {
        //        ClubOpeningTime = clubDetailResponse.ClubOpeningTime,
        //        ClubClosingTime = clubDetailResponse.ClubClosingTime,
        //        ClubId = ClubId,
        //        ClubName = clubDetailResponse.ClubNameEng.ToUpper()
        //    };
        //    ViewBag.AllowedNoOfPeopleList = ApplicationUtilities.LoadDropdownList("ALLOWEDNOOFPEOPLELIST") as Dictionary<string, string>;
        //    ViewBag.ReservableTimeList = ApplicationUtilities.LoadDropdownList("RESERVABLETIMELIST", cId) as Dictionary<string, string>;
        //    var scheduleDBResponse = _business.GetClubReservationSchedule(cId);
        //    if (scheduleDBResponse != null && scheduleDBResponse.Count > 0)
        //    {
        //        responseModel.ClubWeeklyScheduleModel = scheduleDBResponse.MapObjects<ClubWeeklyScheduleModel>();
        //        responseModel.ClubWeeklyScheduleModel.ForEach(x => x.DayLabel = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.EnglishDay : x.JapaneseDay);
        //        responseModel.ClubWeeklyScheduleModel.ForEach(x => x.DateLabel = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.Date : x.JapaneseDate);
        //    }
        //    responseModel.SelectedDate = !string.IsNullOrEmpty(SelectedDate) ? SelectedDate : null;
        //    responseModel.SelectedHost = !string.IsNullOrEmpty(SelectedHost) ? SelectedHost : null;
        //    return View(responseModel);
        //}
        #endregion

        #region Plan Detail
        //[HttpGet]
        //public ActionResult PlanDetail(string ClubId)
        //{
        //    var clubId = !string.IsNullOrEmpty(ClubId) ? ClubId.DecryptParameter() : null;
        //    if (string.IsNullOrEmpty(clubId))
        //    {
        //        AddNotificationMessage(new NotificationModel()
        //        {
        //            NotificationType = NotificationMessage.INFORMATION,
        //            Message = "Invalid club details",
        //            Title = NotificationMessage.INFORMATION.ToString()
        //        });
        //        return RedirectToAction("Index", "DashboardV2");
        //    }
        //    var responseModel = new ReservationPlanListModel()
        //    {
        //        ClubId = ClubId
        //    };
        //    var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter();
        //    var dbResponse = _reservationBuss.GetPlanList("", CustomerId, clubId);
        //    if (dbResponse != null && dbResponse.First().Code.Trim() == "0")
        //    {
        //        if (dbResponse.Count > 0)
        //        {
        //            responseModel.ReservationPlanDetailModel = dbResponse.MapObjects<ReservationPlanDetailModel>();
        //            foreach (var item in responseModel.ReservationPlanDetailModel) item.PlanId = item.PlanId.EncryptParameter();
        //            return View(responseModel);
        //        }
        //        AddNotificationMessage(new NotificationModel()
        //        {
        //            NotificationType = NotificationMessage.INFORMATION,
        //            Message = "Invalid details",
        //            Title = NotificationMessage.INFORMATION.ToString()
        //        });
        //        return RedirectToAction("Index", "DashboardV2");
        //    }
        //    else
        //    {
        //        AddNotificationMessage(new NotificationModel()
        //        {
        //            NotificationType = NotificationMessage.INFORMATION,
        //            Message = dbResponse.First().Message.ToString() ?? "Invalid details",
        //            Title = NotificationMessage.INFORMATION.ToString()
        //        });
        //        return RedirectToAction("Index", "DashboardV2");
        //    }

        //}
        #endregion

        #region Host Details
        //public ActionResult HostDetail(HostDetailModel Model, string NoOfHost = "", string SelectedHost = "")
        //{
        //    var clubId = !string.IsNullOrEmpty(Model.ClubId) ? Model.ClubId.DecryptParameter() : null;
        //    if (!string.IsNullOrEmpty(NoOfHost))
        //    {
        //        string[] parts = NoOfHost.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //        ViewBag.NoOfHost = parts[0];
        //    }
        //    else
        //    {
        //        AddNotificationMessage(new NotificationModel()
        //        {
        //            NotificationType = NotificationMessage.WARNING,
        //            Message = "Invalid host details",
        //            Title = NotificationMessage.WARNING.ToString()
        //        });
        //        return RedirectToAction("ClubDetail");
        //    }
        //    var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter();
        //    if (string.IsNullOrEmpty(clubId) || string.IsNullOrEmpty(CustomerId))
        //    {
        //        AddNotificationMessage(new NotificationModel()
        //        {
        //            NotificationType = NotificationMessage.WARNING,
        //            Message = "Invalid club details",
        //            Title = NotificationMessage.WARNING.ToString()
        //        });
        //        return RedirectToAction("ClubDetail");
        //    }
        //    var responseModel = Model.MapObject<HostDetailModel>();
        //    var dbHostList = _business.GetHostList("", clubId, CustomerId);
        //    responseModel.HostListModels = dbHostList.MapObjects<LocationHostListModel>();
        //    foreach (var item in responseModel.HostListModels)
        //    {
        //        item.ClubId = item.ClubId.EncryptParameter();
        //        item.HostId = item.HostId.EncryptParameter();
        //        item.LocationId = item.LocationId.EncryptParameter();
        //    }
        //    if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() == "DEVELOPMENT") ViewBag.FileLocationPath = "";
        //    else ViewBag.FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
        //    return View(responseModel);
        //}
        #endregion
        #region Reservation Detail
        //[HttpGet]
        //public ActionResult ReservationDetail(string ClubId, string PlanId, string HostIdList)
        //{
        //    var FileLocationPath = string.Empty;
        //    var clubId = !string.IsNullOrEmpty(ClubId) ? ClubId.DecryptParameter() : null;
        //    var planId = !string.IsNullOrEmpty(PlanId) ? PlanId.DecryptParameter() : null;
        //    if (string.IsNullOrEmpty(clubId) || string.IsNullOrEmpty(planId))
        //    {
        //        AddNotificationMessage(new NotificationModel() { NotificationType = NotificationMessage.WARNING, Message = "Invalid club details", Title = NotificationMessage.WARNING.ToString() });
        //        return RedirectToAction("ClubDetail");
        //    }
        //    var ResponseModel = new ReservationDetailModel()
        //    {
        //        ClubId = ClubId,
        //        PlanId = PlanId
        //    };
        //    var profileCommon = new UserProfileCommon()
        //    {
        //        ActionUserId = ApplicationUtilities.GetSessionValue("UserId").ToString().DecryptParameter(),
        //        AgentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter(),
        //        Session = Session.SessionID,
        //    };
        //    var profileDBResponse = _profileBuss.GetUserProfileDetail(profileCommon);
        //    if (profileDBResponse != null) ResponseModel.CustomerDetailModel = profileDBResponse.MapObject<UserProfileModel>();
        //    var clubDetailResp = _business.GetClubDetailById(clubId);
        //    ResponseModel.ClubDetailModel = clubDetailResp.MapObject<LocationClubListModel>();
        //    if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT") FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
        //    ResponseModel.ClubDetailModel.ClubLogo = FileLocationPath + ResponseModel.ClubDetailModel.ClubLogo;
        //    var HostIdListSplit = HostIdList.Split(',');
        //    var HostIdListArray = HostIdListSplit.Select(x => x.DecryptParameter()).ToArray();

        //    var HostIdLists = HostIdListArray != null ? string.Join(",", HostIdListArray.ToArray()) : null;
        //    if (HostIdLists != null && HostIdLists != "")
        //    {
        //        var dbResponse2 = _reservationBuss.GetHostDetailsForReservation(HostIdLists);
        //        if (dbResponse2 != null && dbResponse2.Count > 0)
        //        {
        //            ResponseModel.ReservationHostDetailModel = dbResponse2.MapObjects<ReservationHostDetailModel>();
        //            ResponseModel.ReservationHostDetailModel.ForEach(x => x.HostImagePath = FileLocationPath + x.HostImagePath);
        //        }
        //        else ResponseModel.ReservationHostDetailModel = new List<ReservationHostDetailModel>();
        //    }
        //    else ResponseModel.ReservationHostDetailModel = new List<ReservationHostDetailModel>();
        //    return View(ResponseModel);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public JsonResult ReservationDetail(ReservationConfirmationModel Model, string[] HostIdList)
        //{
        //    var clubId = !string.IsNullOrEmpty(Model.ClubId) ? Model.ClubId.DecryptParameter() : null;
        //    var planId = !string.IsNullOrEmpty(Model.PlanId) ? Model.PlanId.DecryptParameter() : null;
        //    var redirectToUrl = string.Empty;
        //    if (string.IsNullOrEmpty(clubId) || string.IsNullOrEmpty(planId))
        //    {
        //        AddNotificationMessage(new NotificationModel() { NotificationType = NotificationMessage.WARNING, Message = "Invalid club details", Title = NotificationMessage.WARNING.ToString() });
        //        redirectToUrl = Url.Action("ClubDetail", "LocationManagement");
        //        return Json(new { redirectToUrl });
        //    }
        //    var reservationCommon = Model.MapObject<CreateReservationDetailCommon>();
        //    reservationCommon.PlanId = planId;
        //    reservationCommon.ClubId = clubId;
        //    var hId = HostIdList.Select(x => x.DecryptParameter()).ToList();
        //    reservationCommon.HostIdList = hId != null ? string.Join(",", hId.ToArray()) : null;
        //    reservationCommon.CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();
        //    reservationCommon.ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString().DecryptParameter();
        //    reservationCommon.ActionIP = ApplicationUtilities.GetIP();
        //    reservationCommon.NoOfPeople = !string.IsNullOrEmpty(reservationCommon.NoOfPeople) ? reservationCommon.NoOfPeople.Replace("人", "").Trim() : "";
        //    var dbResponse = _reservationBuss.CreateReservation(reservationCommon);
        //    if (dbResponse.Code == ResponseCode.Success)
        //    {
        //        var ResponseModel = new PaymentMethodDetailModel
        //        {
        //            ReservationId = dbResponse.Extra1.EncryptParameter(),
        //            CustomerDetail = dbResponse.Extra2,
        //            ReservationDetail = dbResponse.Extra3,
        //            PlannDetail = dbResponse.Extra4,
        //            TotalAmount = dbResponse.Extra5
        //        };
        //        redirectToUrl = Url.Action("PaymentMethod", "LocationManagement", ResponseModel);
        //        return Json(new { redirectToUrl });
        //    }
        //    else
        //    {
        //        AddNotificationMessage(new NotificationModel() { NotificationType = NotificationMessage.WARNING, Message = "Invalid club details", Title = NotificationMessage.WARNING.ToString() });
        //        redirectToUrl = Url.Action("ClubDetail", "LocationManagement");
        //        return Json(new { redirectToUrl });
        //    }
        //}
        #endregion
        #region Payment Method
        //[HttpGet]
        //public ActionResult PaymentMethod(PaymentMethodDetailModel Model)
        //{
        //    var reservationId = !string.IsNullOrEmpty(Model.ReservationId) ? Model.ReservationId.DecryptParameter() : null;
        //    if (string.IsNullOrEmpty(reservationId))
        //    {
        //        AddNotificationMessage(new NotificationModel()
        //        {
        //            NotificationType = NotificationMessage.WARNING,
        //            Message = "Invalid details",
        //            Title = NotificationMessage.WARNING.ToString()
        //        });
        //        return RedirectToAction("ClubDetail");
        //    }
        //    var ResponseModel = Model.MapObject<PaymentMethodDetailModel>();
        //    ViewBag.PaymentMethodList = ApplicationUtilities.LoadDropdownList("PAYMENTMETHODLIST") as Dictionary<string, string>;
        //    return View(ResponseModel);
        //}
        //[HttpGet]
        //public ActionResult PaymentMethodConfirmation(string ReservationId, string PaymentType)
        //{
        //    var rId = !string.IsNullOrEmpty(ReservationId) ? ReservationId.DecryptParameter() : null;
        //    var pId = !string.IsNullOrEmpty(PaymentType) ? PaymentType.DecryptParameter() : null;
        //    if (string.IsNullOrEmpty(rId) || string.IsNullOrEmpty(pId))
        //    {
        //        AddNotificationMessage(new NotificationModel() { NotificationType = NotificationMessage.WARNING, Message = "Invalid club details", Title = NotificationMessage.WARNING.ToString() });
        //        return RedirectToAction("ClubDetail");
        //    }
        //    var dbRequestCommon = new ManageReservationDetailCommon()
        //    {
        //        CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter(),
        //        PaymentType = pId,
        //        ReservationId = rId,
        //        ActionUser = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter(),
        //        ActionPlatform = "Customer",
        //        ActionIP = ApplicationUtilities.GetIP()
        //    };
        //    var dbResponse = _reservationBuss.ManageReservation(dbRequestCommon);
        //    if (dbResponse == null || dbResponse.Code != ResponseCode.Success)
        //    {
        //        AddNotificationMessage(new NotificationModel() { NotificationType = NotificationMessage.WARNING, Message = "Invalid club details", Title = NotificationMessage.WARNING.ToString() });
        //        return RedirectToAction("ReservationDetail");
        //    }
        //    AddNotificationMessage(new NotificationModel()
        //    {
        //        NotificationType = NotificationMessage.SUCCESS,
        //        Message = dbResponse.Message,
        //        Title = NotificationMessage.SUCCESS.ToString()
        //    });
        //    return RedirectToAction("History", "ReservationHistoryManagement");
        //}
        #endregion

        #region "Search Filter"
        public ActionResult SearchFilter(string locationId = "")
        {
            var FileLocationPath = "";
            if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT")
                FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();

            var Model = new SearchFilterViewModel();
            ViewBag.LocationId = locationId;
            ViewBag.LocationDDList = ApplicationUtilities.LoadDropdownList("LOCATIONDDL") as Dictionary<string, string>;
            ViewBag.ClubCategory = ApplicationUtilities.LoadDropdownList("CLUBCATEGORY") as Dictionary<string, string>;
            ViewBag.HostRanks = ApplicationUtilities.LoadDropdownList("HOSTRANK") as Dictionary<string, string>;
            ViewBag.HostHeights = ApplicationUtilities.LoadDropdownList("HOSTHEIGHT") as Dictionary<string, string>;
            ViewBag.HostBloodTypes = ApplicationUtilities.LoadDropdownList("HOSTBLOODTYPE") as Dictionary<string, string>;
            ViewBag.HostLiquorStrengths = ApplicationUtilities.LoadDropdownList("LIQUORSTRENGTH") as Dictionary<string, string>;
            ViewBag.HostPreviousOccupations = ApplicationUtilities.LoadDropdownList("HOSTPREVIOUSOCCUPATIONS") as Dictionary<string, string>;
            ViewBag.AgeRangeDDL = ApplicationUtilities.LoadDropdownList("AGERANGEDDL") as Dictionary<string, string>;
            ViewBag.HostConstellationGroup = _commonManagementBuss.GetDDL("023");
            var clubListDBResponse = _searchBuss.GetNewClubList();
            if (clubListDBResponse.Count > 0)
            {
                Model.ClubModel = clubListDBResponse.MapObjects<SearchFilterViewNewClubModel>();
                Model.ClubModel.ForEach(x => x.ClubId = x.ClubId?.EncryptParameter());
                Model.ClubModel.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
                Model.ClubModel.ForEach(x => x.ClubLocationId = x.ClubLocationId?.EncryptParameter());
            }
            ViewBag.ActionPageName = "SearchNavMenu";
            ViewBag.PageTitle = Resources.Resource.Search_Filter;
            List<RecommendationLocationModel> locationsList = new List<RecommendationLocationModel>();
            var locationServiceResp = _dashboardBuss.GetLocationList();
            if (locationServiceResp != null && locationServiceResp.Count > 0)
            {
                locationServiceResp.ForEach(x => x.LocationID = x.LocationID?.EncryptParameter());
                locationServiceResp.ForEach(x => x.LocationImage = FileLocationPath + x.LocationImage);
            }
            foreach (var item in locationServiceResp)
            {
                locationsList.Add(new RecommendationLocationModel { name = item.LocationName, lat = item.Latitude, lng = item.Longitude, id = item.LocationID });
            }
            ViewBag.JsonLocation = JsonConvert.SerializeObject(locationsList);
            return View(Model);
        }
        #endregion

        #region "Club Filter"
        public ActionResult ClubFilter(string SearchText = "", string AgentId = "", string LocationId = "",
                                            string ClubCategory = "", string Shift = "")
        {
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
            var FileLocationPath = "";
            if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT")
                FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();

            var RequestModel = new ClubSearchManagementRequestModel()
            {
                SearchText = SearchText,
                AgentId = AgentId?.DecryptParameter(),
                LocationId = LocationId?.DecryptParameter(),
                ClubCategory = ClubCategory?.DecryptParameter(),
                Shift = Shift,
                CustomerAgentId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter()
            };

            var ResponseModel = new List<ClubSearchManagementResponseModel>();
            var dbRequestCommon = RequestModel.MapObject<ClubSearchManagementRequestCommon>();
            var dbResponse = _searchBuss.GetSearchedClub(dbRequestCommon);
            if (dbResponse.Count > 0)
            {
                ResponseModel = dbResponse.MapObjects<ClubSearchManagementResponseModel>();
                foreach (var item in ResponseModel)
                {
                    item.ClubId = item.ClubId?.EncryptParameter();
                    item.LocationId = item.LocationId?.EncryptParameter();
                    item.ClubLogo = FileLocationPath + item.ClubLogo;
                    item.ClubWeeklyScheduleList.ForEach(x => x.DayLabel = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.EnglishDay : x.JapaneseDay);
                    item.HostGalleryImage = item.HostGalleryImage.Select(x => FileLocationPath + x).ToList();
                }
            }
            ViewBag.LocationId = ResponseModel.FirstOrDefault()?.LocationId;
            ViewBag.ActionPageName = "SearchNavMenu";
            ViewBag.PageTitle = Resources.Resource.Search_Filter;
            var ClubRecommendationResponse = new List<ClubRecommendationListModel>();
            var clubRecommendationDBResponse = _searchBuss.GetRecommendedClub(LocationId?.DecryptParameter());
            if (clubRecommendationDBResponse.Count > 0)
            {
                ClubRecommendationResponse = clubRecommendationDBResponse.MapObjects<ClubRecommendationListModel>();
                ClubRecommendationResponse.ForEach(x => x.ClubId = x.ClubId.EncryptParameter());
                ClubRecommendationResponse.ForEach(x => x.LocationId = x.LocationId.EncryptParameter());
                ClubRecommendationResponse.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
            }
            ViewBag.ClubRecommendationModel = ClubRecommendationResponse;
            return View(ResponseModel);
        }
        #endregion

        #region "Host Filter"
        public ActionResult HostFilter(HostSearchManagementRequestModel Request)
        {
            var FileLocationPath = "";
            if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT")
                FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
            var ResponseModel = new List<HostSearchManagementResponseModel>();
            var dbRequestCommon = Request.MapObject<HostSearchManagementRequestCommon>();
            dbRequestCommon.AgentId = !string.IsNullOrEmpty(dbRequestCommon.AgentId) ? dbRequestCommon.AgentId.DecryptParameter() : null;
            dbRequestCommon.LocationId = !string.IsNullOrEmpty(dbRequestCommon.LocationId) ? dbRequestCommon.LocationId.DecryptParameter() : null;
            dbRequestCommon.Rank = !string.IsNullOrEmpty(dbRequestCommon.Rank) ? dbRequestCommon.Rank.DecryptParameter() : null;
            dbRequestCommon.Height = !string.IsNullOrEmpty(dbRequestCommon.Height) ? dbRequestCommon.Height.DecryptParameter() : null;
            dbRequestCommon.BloodType = !string.IsNullOrEmpty(dbRequestCommon.BloodType) ? dbRequestCommon.BloodType.DecryptParameter() : null;
            dbRequestCommon.ZodiacGroup = !string.IsNullOrEmpty(dbRequestCommon.ZodiacGroup) ? dbRequestCommon.ZodiacGroup.DecryptParameter() : null;
            dbRequestCommon.LiquorStrength = !string.IsNullOrEmpty(dbRequestCommon.LiquorStrength) ? dbRequestCommon.LiquorStrength.DecryptParameter() : null;
            dbRequestCommon.PreviousOccupation = !string.IsNullOrEmpty(dbRequestCommon.PreviousOccupation) ? dbRequestCommon.PreviousOccupation.DecryptParameter() : null;
            dbRequestCommon.Handsomeness = !string.IsNullOrEmpty(dbRequestCommon.Handsomeness) ? dbRequestCommon.Handsomeness.DecryptParameter() : null;
            dbRequestCommon.AgeRange = !string.IsNullOrEmpty(dbRequestCommon.AgeRange) ? dbRequestCommon.AgeRange.DecryptParameter() : null;
            var dbResponse = _searchBuss.GetSearchedHost(dbRequestCommon);
            if (dbResponse.Count > 0)
            {
                ResponseModel = dbResponse.MapObjects<HostSearchManagementResponseModel>();
                foreach (var item in ResponseModel)
                {
                    item.ClubId = item.ClubId?.EncryptParameter();
                    item.HostId = item.HostId?.EncryptParameter();
                    item.LocationId = item.LocationId?.EncryptParameter();
                    item.HostImage = FileLocationPath + item.HostImage;
                    item.ClubLogo = FileLocationPath + item.ClubLogo;
                }
            }
            ViewBag.LocationId = ResponseModel.FirstOrDefault()?.LocationId;
            ViewBag.ActionPageName = "SearchNavMenu";
            ViewBag.PageTitle = Resources.Resource.Search_Filter;
            var HostRecommendationResponse = new List<HostRecommendationListModel>();
            var hostRecommendationDBResponse = _searchBuss.GetRecommendedHost(Request.LocationId?.DecryptParameter());
            if (hostRecommendationDBResponse.Count > 0)
            {
                HostRecommendationResponse = hostRecommendationDBResponse.MapObjects<HostRecommendationListModel>();
                HostRecommendationResponse.ForEach(x => x.ClubId = x.ClubId.EncryptParameter());
                HostRecommendationResponse.ForEach(x => x.HostId = x.HostId.EncryptParameter());
                HostRecommendationResponse.ForEach(x => x.LocationId = x.LocationId.EncryptParameter());
                HostRecommendationResponse.ForEach(x => x.HostImage = FileLocationPath + x.HostImage);
                HostRecommendationResponse.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
            }
            ViewBag.HostRecommendationModel = HostRecommendationResponse;
            return View(ResponseModel);
        }
        #endregion
    }
}