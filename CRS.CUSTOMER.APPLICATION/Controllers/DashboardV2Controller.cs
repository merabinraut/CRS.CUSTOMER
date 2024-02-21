using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.Dashboard;
using CRS.CUSTOMER.APPLICATION.Models.LocationManagement;
using CRS.CUSTOMER.APPLICATION.Models.SearchFilterManagement;
using CRS.CUSTOMER.BUSINESS.Dashboard;
using CRS.CUSTOMER.BUSINESS.DashboardV2;
using CRS.CUSTOMER.BUSINESS.RecommendedClubHost;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.RecommendedClubHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class DashboardV2Controller : Controller
    {
        private readonly IDashboardBusiness _oldDashboardBusiness;
        private readonly IRecommendedClubHostBusiness _recommendedClubHostBuss;
        private readonly IDashboardV2Business _dashboardBusiness;

        public DashboardV2Controller(IDashboardBusiness oldDashboardBusiness, IRecommendedClubHostBusiness recommendedClubHostBuss, IDashboardV2Business dashboardBusiness)
            => (_oldDashboardBusiness, _recommendedClubHostBuss, _dashboardBusiness) = (oldDashboardBusiness, recommendedClubHostBuss, dashboardBusiness);

        [HttpGet]
        public ActionResult Index()
        {
            var ResponseModel = new DashboardModel();
            var bannerServiceResp = _oldDashboardBusiness.GetBanners();
            if (bannerServiceResp != null && bannerServiceResp.Count > 0)
            {
                ResponseModel.Banners = bannerServiceResp.MapObjects<BannersModel>();
                ResponseModel.Banners.ForEach(x => x.BannerId = x.BannerId?.EncryptParameter());
                ResponseModel.Banners.ForEach(x => x.BannerImage = ImageHelper.ProcessedImage(x.BannerImage));
            }
            var locationServiceResp = _oldDashboardBusiness.GetLocationList();
            if (locationServiceResp != null && locationServiceResp.Count > 0)
            {
                ResponseModel.Location = locationServiceResp.MapObjects<LocationListModel>();
                ResponseModel.Location.ForEach(x => x.LocationID = x.LocationID?.EncryptParameter());
                ResponseModel.Location.ForEach(x => x.LocationImage = ImageHelper.ProcessedImage(x.LocationImage));
            }
            var planListDBResponse = _oldDashboardBusiness.GetPlansList();
            if (planListDBResponse != null && planListDBResponse.Count > 0)
            {
                ResponseModel.PlanModel = planListDBResponse.MapObjects<DashboardPlanModel>();
                ResponseModel.PlanModel.ForEach(x => x.PlanId = x.PlanId?.EncryptParameter());
                ResponseModel.PlanModel.ForEach(x => x.PlanImage = ImageHelper.ProcessedImage(x.PlanImage));
            }
            var recommendedHostListDBResponse = _oldDashboardBusiness.GetRecommendedHost();
            if (recommendedHostListDBResponse.Count > 0)
            {
                ResponseModel.RecommendedHostModel = recommendedHostListDBResponse.MapObjects<DashboardRecommendedHostModel>();
                foreach (var item in ResponseModel.RecommendedHostModel)
                {
                    item.LocationId = item.LocationId?.EncryptParameter();
                    item.ClubId = item.ClubId?.EncryptParameter();
                    item.HostId = item.HostId?.EncryptParameter();
                    item.HostImage = ImageHelper.ProcessedImage(item.HostImage);
                    item.ClubLogo = ImageHelper.ProcessedImage(item.ClubLogo);
                }
            }
            var recommendedClubListDBResponse = _oldDashboardBusiness.GetRecommendedClub();
            if (recommendedClubListDBResponse.Count > 0)
            {
                ResponseModel.RecommendedClubModel = recommendedClubListDBResponse.MapObjects<DashboardRecommendedClubModel>();
                foreach (var item in ResponseModel.RecommendedClubModel)
                {
                    item.LocationId = item.LocationId?.EncryptParameter();
                    item.ClubId = item.ClubId?.EncryptParameter();
                    item.ClubLogo = ImageHelper.ProcessedImage(item.ClubLogo);
                }
            }
            List<RecommendationLocationModel> locationsList = new List<RecommendationLocationModel>();
            foreach (var item in ResponseModel.Location)
                locationsList.Add(new RecommendationLocationModel { name = item.LocationName, lat = item.Latitude, lng = item.Longitude, id = item.LocationID });

            ViewBag.JsonLocation = JsonConvert.SerializeObject(locationsList);
            ViewBag.ActionPageName = "Dashboard";
            return View(ResponseModel);
        }

        [HttpGet]
        public JsonResult GetRecommendedClubAndHost(string LocationId)
        {
            var lId = !string.IsNullOrEmpty(LocationId) ? LocationId.DecryptParameter() : null;
            var Response = new RecommendedClubAndHostModel();
            var clubRecommendationDBResponse = _oldDashboardBusiness.GetRecommendedClub(lId);
            if (clubRecommendationDBResponse.Count > 0)
            {
                Response.RecommendedClubModel = clubRecommendationDBResponse.MapObjects<ClubRecommendationListModel>();
                Response.RecommendedClubModel.ForEach(x => x.ClubId = x.ClubId.EncryptParameter());
                Response.RecommendedClubModel.ForEach(x => x.LocationId = x.LocationId.EncryptParameter());
                Response.RecommendedClubModel.ForEach(x => x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo));
            }
            var hostRecommendationDBResponse = _oldDashboardBusiness.GetRecommendedHost(lId);
            if (hostRecommendationDBResponse.Count > 0)
            {
                Response.RecommendedHostModel = hostRecommendationDBResponse.MapObjects<HostRecommendationListModel>();
                Response.RecommendedHostModel.ForEach(x => x.ClubId = x.ClubId.EncryptParameter());
                Response.RecommendedHostModel.ForEach(x => x.HostId = x.HostId.EncryptParameter());
                Response.RecommendedHostModel.ForEach(x => x.LocationId = x.LocationId.EncryptParameter());
                Response.RecommendedHostModel.ForEach(x => x.HostImage = ImageHelper.ProcessedImage(x.HostImage));
                Response.RecommendedHostModel.ForEach(x => x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo));
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMainPageClubHost(string LocationId)
        {
            var responseData = new Dictionary<string, object> { { "Code", 1 }, { "Message", "Invalid Details" }, { "PartialView", "" } };
            var lId = !string.IsNullOrEmpty(LocationId) ? LocationId.DecryptParameter() : null;
            var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter();
            var Response = new LocationClubHostModel();
            #region Recommended Club
            var recommendedClubDBRequest = new RecommendedClubRequestCommon()
            {
                LocationId = lId,
                CustomerId = CustomerId
            };
            var dbClubResponse = _recommendedClubHostBuss.GetRecommendedClub(recommendedClubDBRequest);
            Response.ClubListModel = dbClubResponse.MapObjects<LocationClubListModel>();
            foreach (var item in Response.ClubListModel)
            {
                item.ClubId = item.ClubId.EncryptParameter();
                item.LocationId = item.LocationId.EncryptParameter();
                item.ClubLogo = ImageHelper.ProcessedImage(item.ClubLogo);
                item.ClubCoverPhoto = ImageHelper.ProcessedImage(item.ClubCoverPhoto);
                item.HostGalleryImage = item.HostGalleryImage.Select(x => ImageHelper.ProcessedImage(x)).ToList();
            }
            #endregion
            #region Recommended Host
            if (Response.ClubListModel != null && Response.ClubListModel.Count > 0)
            {
                var recommendedHostDBRequest = new RecommendedHostRequestCommon()
                {
                    LocationId = lId,
                    CustomerId = CustomerId
                };
                var dbHostResponse = _recommendedClubHostBuss.GetRecommendedHost(recommendedHostDBRequest);
                Response.HostListModel = dbHostResponse.MapObjects<LocationHostListModel>();
                foreach (var item in Response.HostListModel)
                {
                    item.ClubId = item.ClubId.EncryptParameter();
                    item.LocationId = item.LocationId.EncryptParameter();
                    item.HostId = item.HostId.EncryptParameter();
                    item.HostImage = ImageHelper.ProcessedImage(item.HostImage);
                    item.ClubLogo = ImageHelper.ProcessedImage(item.ClubLogo);
                }
            }
            #endregion
            if (Response.ClubListModel != null && Response.ClubListModel.Count > 0)
            {
                var partialViewString = RenderHelper.RenderPartialViewToString(this, "_MainPageClubHost", Response);
                responseData["Code"] = 0;
                responseData["Message"] = "Success";
                responseData["PartialView"] = partialViewString;
            }
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLocationFilterPopUp()
        {
            var responseData = new Dictionary<string, object> { { "Code", 1 }, { "Message", "Invalid Details" }, { "PartialView", "" } };
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }
    }
}