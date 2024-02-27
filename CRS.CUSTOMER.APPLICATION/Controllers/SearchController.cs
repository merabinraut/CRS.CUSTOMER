using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.Search;
using CRS.CUSTOMER.APPLICATION.Models.SearchFilterManagement;
using CRS.CUSTOMER.BUSINESS.Dashboard;
using CRS.CUSTOMER.BUSINESS.Search;
using System.Linq;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchBusiness _searchBusiness;
        private readonly IDashboardBusiness _oldDashboardBusiness;
        public SearchController(ISearchBusiness searchBusiness, IDashboardBusiness oldDashboardBusiness) => (_searchBusiness, _oldDashboardBusiness) = (searchBusiness, oldDashboardBusiness);
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
                Response.FilteredClubModel.ForEach(x =>
                {
                    x.ClubId = x.ClubId.EncryptParameter();
                    x.ClubLocationId = x.ClubLocationId.EncryptParameter();
                    x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo);
                    x.HostGalleryImage = x.HostGalleryImage.Select(y => ImageHelper.ProcessedImage(y)).ToList();
                });
            }
            else
            {

            }
            return View(Response);
        }

        [HttpGet]
        public ActionResult HostSearchResult()
        {
            ViewBag.ActionPageName = "SearchFilter";
            return View();
        }
    }
}