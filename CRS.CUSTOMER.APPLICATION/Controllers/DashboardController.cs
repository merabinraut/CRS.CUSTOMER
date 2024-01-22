using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.Dashboard;
using CRS.CUSTOMER.APPLICATION.Models.SearchFilterManagement;
using CRS.CUSTOMER.BUSINESS.Dashboard;
using CRS.CUSTOMER.BUSINESS.SearchFilterManagement;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class DashboardController : CustomController
    {
        private readonly IDashboardBusiness _dashboardBusiness;
        public DashboardController(IDashboardBusiness dashboardBusiness)
        {
            _dashboardBusiness = dashboardBusiness;
        }
        [HttpGet]
        public ActionResult Index()
        {
            var Username = ApplicationUtilities.GetSessionValue("Username").ToString();
            if (string.IsNullOrEmpty(Username)) return RedirectToAction("LogOff", "Home");
            var viewModel = new DashboardModel();
            var bannerServiceResp = _dashboardBusiness.GetBanners();
            var locationServiceResp = _dashboardBusiness.GetLocationList();
            var FileLocationPath = "";
            if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() == "DEVELOPMENT") FileLocationPath = "";
            else FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
            if (bannerServiceResp != null && bannerServiceResp.Count > 0)
            {
                viewModel.Banners = bannerServiceResp.MapObjects<BannersModel>();
                viewModel.Banners.ForEach(x => x.BannerId = x.BannerId?.EncryptParameter());
                viewModel.Banners.ForEach(x => x.BannerImage = FileLocationPath + x.BannerImage);
            }
            if (locationServiceResp != null && locationServiceResp.Count > 0)
            {
                viewModel.Location = locationServiceResp.MapObjects<LocationListModel>();
                viewModel.Location.ForEach(x => x.LocationID = x.LocationID?.EncryptParameter());
                viewModel.Location.ForEach(x => x.LocationImage = FileLocationPath + x.LocationImage);
            }
            else viewModel.Location = new List<LocationListModel>();
            var planListDBResponse = _dashboardBusiness.GetPlansList();
            if (planListDBResponse != null && planListDBResponse.Count > 0)
            {
                viewModel.PlanModel = planListDBResponse.MapObjects<DashboardPlanModel>();
                viewModel.PlanModel.ForEach(x => x.PlanId = x.PlanId?.EncryptParameter());
                viewModel.PlanModel.ForEach(x => x.PlanImage = FileLocationPath + x.PlanImage);
            }
            var recommendedHostListDBResponse = _dashboardBusiness.GetRecommendedHost();
            if (recommendedHostListDBResponse.Count > 0)
            {
                viewModel.RecommendedHostModel = recommendedHostListDBResponse.MapObjects<DashboardRecommendedHostModel>();
                foreach (var item in viewModel.RecommendedHostModel)
                {
                    item.LocationId = item.LocationId?.EncryptParameter();
                    item.ClubId = item.ClubId?.EncryptParameter();
                    item.HostId = item.HostId?.EncryptParameter();
                    item.HostImage = FileLocationPath + item.HostImage;
                    item.ClubLogo = FileLocationPath + item.ClubLogo;
                }
            }
            var recommendedClubListDBResponse = _dashboardBusiness.GetRecommendedClub();
            if (recommendedClubListDBResponse.Count > 0)
            {
                viewModel.RecommendedClubModel = recommendedClubListDBResponse.MapObjects<DashboardRecommendedClubModel>();
                foreach (var item in viewModel.RecommendedClubModel)
                {
                    item.LocationId = item.LocationId?.EncryptParameter();
                    item.ClubId = item.ClubId?.EncryptParameter();
                    item.ClubLogo = FileLocationPath + item.ClubLogo;
                }
            }
            ViewBag.ActionPageName = "Dashboard";

            List<RecommendationLocationModel> locationsList = new List<RecommendationLocationModel>();
            foreach (var item in viewModel.Location)
            {
                locationsList.Add(new RecommendationLocationModel { name = item.LocationName, lat = item.Latitude, lng = item.Longitude, id = item.LocationID });
                //locationsList.Add(new RecommendationLocationModel { name = item.LocationName, lat = double.Parse(item.Latitude), lng = double.Parse(item.Longitude), id = item.LocationID });
            }
            ViewBag.JsonLocation = JsonConvert.SerializeObject(locationsList);
            return View(viewModel);
        }

        [HttpGet]
        public JsonResult GetRecommendedClubAndHost(string LocationId)
        {
            var lId = !string.IsNullOrEmpty(LocationId) ? LocationId.DecryptParameter() : null;
            var FileLocationPath = "";
            if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT")
                FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
            var Response = new RecommendedClubAndHostModel();
            var clubRecommendationDBResponse = _dashboardBusiness.GetRecommendedClub(lId);
            if (clubRecommendationDBResponse.Count > 0)
            {
                Response.RecommendedClubModel = clubRecommendationDBResponse.MapObjects<ClubRecommendationListModel>();
                Response.RecommendedClubModel.ForEach(x => x.ClubId = x.ClubId.EncryptParameter());
                Response.RecommendedClubModel.ForEach(x => x.LocationId = x.LocationId.EncryptParameter());
                Response.RecommendedClubModel.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
            }
            var hostRecommendationDBResponse = _dashboardBusiness.GetRecommendedHost(lId);
            if (hostRecommendationDBResponse.Count > 0)
            {
                Response.RecommendedHostModel = hostRecommendationDBResponse.MapObjects<HostRecommendationListModel>();
                Response.RecommendedHostModel.ForEach(x => x.ClubId = x.ClubId.EncryptParameter());
                Response.RecommendedHostModel.ForEach(x => x.HostId = x.HostId.EncryptParameter());
                Response.RecommendedHostModel.ForEach(x => x.LocationId = x.LocationId.EncryptParameter());
                Response.RecommendedHostModel.ForEach(x => x.HostImage = FileLocationPath + x.HostImage);
                Response.RecommendedHostModel.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }
    }
}