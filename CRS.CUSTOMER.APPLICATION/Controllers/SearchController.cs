using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.DashboardV2;
using CRS.CUSTOMER.APPLICATION.Models.Search;
using CRS.CUSTOMER.APPLICATION.Models.SearchFilterManagement;
using CRS.CUSTOMER.BUSINESS.Dashboard;
using CRS.CUSTOMER.BUSINESS.DashboardV2;
using CRS.CUSTOMER.BUSINESS.Search;
using CRS.CUSTOMER.SHARED.Search;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchBusiness _searchBusiness;
        private readonly IDashboardBusiness _oldDashboardBusiness;
        private readonly IDashboardV2Business _dashboardBusiness;
        public SearchController(ISearchBusiness searchBusiness, IDashboardBusiness oldDashboardBusiness, IDashboardV2Business dashboardBusiness) => (_searchBusiness, _oldDashboardBusiness, _dashboardBusiness) = (searchBusiness, oldDashboardBusiness, dashboardBusiness);
        [HttpGet]
        public ActionResult ClubSearchResult(string LocationId, string SearchFilter, string ClubCategory, string Price, string Shift, string Time, string ClubAvailability, bool NewClub = false)
        {
            ViewBag.ActionPageName = "SearchFilter";
            var lId = !string.IsNullOrEmpty(LocationId) ? LocationId.DecryptParameter() : null;
            var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter();
            var Response = new ClubSearchResultModel();
            var clubRecommendationDBResponse = _oldDashboardBusiness.GetRecommendedClub(lId);
            Response.RecommendedClubModel = clubRecommendationDBResponse.MapObjects<ClubRecommendationListModel>();
            Response.RecommendedClubModel.ForEach(x =>
            {
                x.LocationId = x.LocationId.EncryptParameter();
                x.ClubId = x.ClubId.EncryptParameter();
                x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo);
            });
            if (NewClub)
            {
                var dbResponse = _searchBusiness.GetNewClub(lId, CustomerId);
                Response.FilteredClubModel = dbResponse.MapObjects<SearchFilterClubDetailModel>();
            }
            else
            {
                var dbRequest = new ClubPreferenceFilterRequest
                {
                    LocationId = lId,
                    SearchFilter = SearchFilter,
                    ClubCategory = string.Join(",", ClubCategory.Split(',').Select(x => x.DecryptParameter())),
                    Price = string.Join(",", Price.Split(',').Select(x => x.DecryptParameter())),
                    Shift = string.IsNullOrEmpty(Shift) ? string.Empty : Shift.DecryptParameter(),
                    Time = string.IsNullOrEmpty(Time) ? string.Empty : Time.DecryptParameter(),
                    ClubAvailability = string.Join(",", ClubAvailability.Split(',').Select(x => x.DecryptParameter())),
                    CustomerId = CustomerId
                };
                var dbResponse = _searchBusiness.ClubPreferenceFilter(dbRequest);
                Response.FilteredClubModel = dbResponse.MapObjects<SearchFilterClubDetailModel>();
            }
            Response.FilteredClubModel.ForEach(x =>
            {
                x.ClubId = x.ClubId.EncryptParameter();
                x.ClubLocationId = x.ClubLocationId.EncryptParameter();
                x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo);
                x.HostGalleryImage = x.HostGalleryImage.Select(y => ImageHelper.ProcessedImage(y)).ToList();
            });

            ViewBag.LocationId = LocationId;
            ViewBag.SearchFilter = SearchFilter;
            ViewBag.ClubCategory = ClubCategory;
            ViewBag.Price = Price;
            ViewBag.Shift = Shift;
            ViewBag.Time = Time;
            ViewBag.ClubAvailability = ClubAvailability;

            return View(Response);
        }

        [HttpGet]
        public ActionResult DateTimeFilter(string LocationId, string Date, string Time, string NoOfPeople)
        {
            ViewBag.ActionPageName = "SearchFilter";
            var lId = !string.IsNullOrEmpty(LocationId) ? LocationId.DecryptParameter() : null;
            var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter();
            var Response = new ClubSearchResultModel();
            var clubRecommendationDBResponse = _oldDashboardBusiness.GetRecommendedClub(lId);
            Response.RecommendedClubModel = clubRecommendationDBResponse.MapObjects<ClubRecommendationListModel>();
            Response.RecommendedClubModel.ForEach(x =>
            {
                x.LocationId = x.LocationId.EncryptParameter();
                x.ClubId = x.ClubId.EncryptParameter();
                x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo);
            });
            var dbRequest = new ClubDateTimeAndOtherFilterRequest
            {
                LocationId = lId,
                Date = Date,
                Time = string.IsNullOrEmpty(Time) ? string.Empty : Time.DecryptParameter(),
                NoOfPeople = string.IsNullOrEmpty(NoOfPeople) ? string.Empty : NoOfPeople.DecryptParameter(),
                CustomerId = CustomerId
            };
            var dbResponse = _searchBusiness.ClubFilterViewDateTimeAndOthers(dbRequest);
            Response.FilteredClubModel = dbResponse.MapObjects<SearchFilterClubDetailModel>();
            Response.FilteredClubModel.ForEach(x =>
            {
                x.ClubId = x.ClubId.EncryptParameter();
                x.ClubLocationId = x.ClubLocationId.EncryptParameter();
                x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo);
                x.HostGalleryImage = x.HostGalleryImage.Select(y => ImageHelper.ProcessedImage(y)).ToList();
            });
            ViewBag.LocationId = LocationId;
            ViewBag.Date = Date;
            ViewBag.Time = Time;
            ViewBag.NoOfPeople = NoOfPeople;
            return View("ClubSearchResult", Response);
        }

        [HttpGet]
        public ActionResult HostSearchResult(string LocationId, string SearchFilter, string Height, string Age, string BloodType, string ConstellationGroup, string Occupation, bool NewHost = false)
        {
            ViewBag.ActionPageName = "SearchFilter";
            var Response = new HostSearchResultModel();
            Response.HostRecommendationModel = new List<DashboardV2HostDetailModel>();
            var lId = !string.IsNullOrEmpty(LocationId) ? LocationId.DecryptParameter() : null;
            var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter();
            var hostRecommendationDBResponse = _oldDashboardBusiness.GetRecommendedHost(lId);
            var HostRecommendationModel = new List<DashboardV2HostDetailModel>();
            foreach (var item in hostRecommendationDBResponse)
            {
                Response.HostRecommendationModel.Add(new DashboardV2HostDetailModel
                {
                    ClubId = item.ClubId.EncryptParameter(),
                    ClubNameEnglish = item.ClubNameEnglish,
                    ClubNameJapanese = item.ClubNameJapanese,
                    ClubLogo = ImageHelper.ProcessedImage(item.ClubLogo),
                    HostId = item.HostId.EncryptParameter(),
                    HostNameEnglish = item.HostName,
                    HostNameJapanese = item.HostName,
                    HostLogo = ImageHelper.ProcessedImage(item.HostImage),
                    ClubLocationId = item.LocationId.EncryptParameter(),
                    //IsBookmarked = 
                });
            }
            if (NewHost)
            {
                var dbHostResponse = _dashboardBusiness.GetNewHost(lId, CustomerId);
                Response.FilteredHostModel = dbHostResponse.MapObjects<DashboardV2HostDetailModel>();
            }
            else
            {
                var dbRequest = new HostPreferenceFilterRequest
                {
                    LocationId = lId,
                    SearchFilter = SearchFilter,
                    Height = string.Join(",", Height.Split(',').Select(x => x.DecryptParameter())),
                    Age = string.Join(",", Age.Split(',').Select(x => x.DecryptParameter())),
                    BloodType = string.Join(",", BloodType.Split(',').Select(x => x.DecryptParameter())),
                    ConstellationGroup = string.Join(",", ConstellationGroup.Split(',').Select(x => x.DecryptParameter())),
                    Occupation = (string.IsNullOrEmpty(Occupation) || Occupation.Trim() == "0") ? string.Empty : Occupation.DecryptParameter(),
                    CustomerId = CustomerId
                };
                var dbHostResponse = _searchBusiness.HostPreferenceFilter(dbRequest);
                Response.FilteredHostModel = dbHostResponse.MapObjects<DashboardV2HostDetailModel>();
            }
            Response.FilteredHostModel.ForEach(x => { x.ClubId = x.ClubId.EncryptParameter(); x.HostId = x.HostId.EncryptParameter(); x.ClubLocationId = x.ClubLocationId.EncryptParameter(); x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo); x.HostLogo = ImageHelper.ProcessedImage(x.HostLogo); });
            return View(Response);
        }
    }
}