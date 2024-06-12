using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.DashboardV2;
using CRS.CUSTOMER.APPLICATION.Models.Search;
using CRS.CUSTOMER.APPLICATION.Models.SearchFilterManagement;
using CRS.CUSTOMER.APPLICATION.Models.SearchV2;
using CRS.CUSTOMER.BUSINESS.CommonManagement;
using CRS.CUSTOMER.BUSINESS.DashboardV2;
using CRS.CUSTOMER.BUSINESS.Search;
using CRS.CUSTOMER.BUSINESS.SearchFilterManagement;
using CRS.CUSTOMER.SHARED.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class SearchV2Controller : Controller
    {
        private readonly Dictionary<string, string> LocationHelper = ApplicationUtilities.MapJsonDataToDictionaryViaKeyName("URLManagementConfigruation", "Location");
        private readonly ISearchFilterManagementBusiness _searchBusinessOld;
        private readonly ISearchBusiness _searchBusiness;
        private readonly ICommonManagementBusiness _commonBusiness;
        private readonly IDashboardV2Business _dashboardBusiness;
        public SearchV2Controller(ISearchFilterManagementBusiness searchBusinessOld, ISearchBusiness searchBusiness, IDashboardV2Business dashboardBusiness, ICommonManagementBusiness commonBusiness)
        {
            _searchBusinessOld = searchBusinessOld;
            _searchBusiness = searchBusiness;
            _dashboardBusiness = dashboardBusiness;
            _commonBusiness = commonBusiness;
        }

        [HttpGet, Route("search/{prefectures}/{area}")]
        public ActionResult Index(string prefectures, string area, string scftab)
        {
            scftab = string.IsNullOrEmpty(scftab) ? "01" : scftab;
            ViewBag.scftab = scftab;
            ViewBag.PrefecturesArea = $"/{prefectures}/{area}";
            var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter();
            var locationId = ApplicationUtilities.GetKeyValueFromDictionary(LocationHelper, ViewBag.PrefecturesArea);
            var Response = new SearchV2FilterRequestModel();
            if (!string.IsNullOrEmpty(scftab) && scftab.Trim() == "02")
            {
                ViewBag.scftab = "02";
                Response.SearchV2FilterTab2Model.LocationModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("10"));
                Response.SearchV2FilterTab2Model.BloodTypeModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("5"));
                Response.SearchV2FilterTab2Model.HeightModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("6"));
                Response.SearchV2FilterTab2Model.AgeModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("7"));
                Response.SearchV2FilterTab2Model.ConstellationModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("8"));
                Response.SearchV2FilterTab2Model.OccupationModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("9"), true);
                var HostConstellationGroup = _commonBusiness.GetDDL("023");
                HostConstellationGroup.ForEach(x => x.Value = x.Value.EncryptParameter());
                ViewBag.HostConstellationGroup = HostConstellationGroup;
            }
            else if (!string.IsNullOrEmpty(scftab) && scftab.Trim() == "03")
            {
                ViewBag.scftab = "03";
                var dbClubResponse = _dashboardBusiness.GetNewClub(locationId, CustomerId, "1");
                Response.SearchV2FilterTab3Model.ClubModel = ApplicationUtilities.MapObjects<DashboardV2ClubDetailModel>(dbClubResponse);
                Response.SearchV2FilterTab3Model.ClubModel.ForEach(x => { x.ClubId = x.ClubId.EncryptParameter(); x.ClubLocationId = x.ClubLocationId.EncryptParameter(); x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo); });
                var dbHostResponse = _dashboardBusiness.GetNewHost(locationId, CustomerId, "1");
                Response.SearchV2FilterTab3Model.HostModel = ApplicationUtilities.MapObjects<DashboardV2HostDetailModel>(dbHostResponse);
                Response.SearchV2FilterTab3Model.HostModel.ForEach(x => { x.ClubId = x.ClubId.EncryptParameter(); x.HostId = x.HostId.EncryptParameter(); x.ClubLocationId = x.ClubLocationId.EncryptParameter(); x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo); x.HostLogo = ImageHelper.ProcessedImage(x.HostLogo); });
            }
            else if (!string.IsNullOrEmpty(scftab) && scftab.Trim() == "04")
            {
                ViewBag.scftab = "04";
                Response.SearchV2FilterTab4Model.LocationModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("10"));
                var ClubDetailMapModel = new List<ClubMapDetailModel>();
                var ClubDetailDbResponse = _searchBusiness.GetClubMapDetail(locationId);
                ClubDetailMapModel = ApplicationUtilities.MapObjects<ClubMapDetailModel>(ClubDetailDbResponse);
                string CurrentUrl = ApplicationUtilities.GetAddressFromUrl(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                List<dynamic> mappedData = new List<dynamic>();
                foreach (var item in ClubDetailMapModel)
                {
                    dynamic mappedItem = new System.Dynamic.ExpandoObject();
                    if (!string.IsNullOrEmpty(CurrentUrl))
                    {
                        if (!CurrentUrl.Contains("/LocationManagement/ClubDetail_V2"))
                            CurrentUrl += "/LocationManagement/ClubDetail_V2";
                        var parameters = new List<string>();
                        if (!string.IsNullOrEmpty(item.LocationId))
                            parameters.Add($"LocationId={item.LocationId.EncryptParameter()}");

                        if (!string.IsNullOrEmpty(item.ClubId))
                            parameters.Add($"ClubId={item.ClubId.EncryptParameter()}");

                        string queryString = string.Join("&", parameters);

                        StringBuilder urlBuilder = new StringBuilder(CurrentUrl);
                        urlBuilder.Append(CurrentUrl.Contains("?") ? "&" : "?");
                        urlBuilder.Append(queryString);

                        mappedItem.URL = urlBuilder.ToString();
                    }

                    if (float.TryParse(item.Latitude, out float latitude) && float.TryParse(item.Longitude, out float longitude))
                    {
                        mappedItem.lat = latitude;
                        mappedItem.lng = longitude;
                    }
                    else continue;
                    mappedItem.clubNameEnglish = item.ClubNameEnglish;
                    mappedItem.clubNameJapanese = item.ClubNameJapanese;
                    mappedItem.ratingScale = item.RatingScale;
                    mappedItem.clubLogo = ImageHelper.ProcessedImage(item.ClubLogo);

                    mappedData.Add(mappedItem);
                }
                ViewBag.ClubDetailMapData = mappedData;
            }
            else
            {
                ViewBag.scftab = "01";
                Response.SearchV2FilterTab1Model.LocationModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("10"));
                Response.SearchV2FilterTab1Model.ClubCategoryModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("2"));
                Response.SearchV2FilterTab1Model.PlanPriceModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("3"));
                Response.SearchV2FilterTab1Model.ClubAvailabilityModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("4"));
            }
            return View(Response);
        }

        [HttpPost, Route("search/{prefectures}/{area}")]
        public ActionResult Index(string prefectures, string area, string scftab, SearchV2FilterTab1RequestModel Tab1Request = null, bool NewClub = false, SearchV2FilterTab2RequestModel Tab2Request = null, bool NewHost = false)
        {
            scftab = string.IsNullOrEmpty(scftab) ? "01" : scftab;
            ViewBag.PrefecturesArea = $"/{prefectures}/{area}";
            var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter();
            var locationId = ApplicationUtilities.GetKeyValueFromDictionary(LocationHelper, ViewBag.PrefecturesArea);
            if ((!string.IsNullOrEmpty(scftab) && scftab.Trim() == "02") || NewHost)
            {
                ViewBag.scftab = "02";
                var Response = new SearchV2FilterTab2ResponseModel();
                var hostRecommendationDBResponse = _searchBusinessOld.GetRecommendedHost(locationId, CustomerId);
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
                        HostNameJapanese = item.HostNameJapanese,
                        HostLogo = ImageHelper.ProcessedImage(item.HostImage),
                        ClubLocationId = item.LocationId.EncryptParameter(),
                        IsBookmarked = item.IsBookmarked
                    });
                }
                if (NewHost)
                {
                    ViewBag.scftab = "03";
                    var dbHostResponse = _dashboardBusiness.GetNewHost(locationId, CustomerId);
                    Response.FilteredHostModel = ApplicationUtilities.MapObjects<DashboardV2HostDetailModel>(dbHostResponse);
                }
                else
                {
                    var dbRequest = new HostPreferenceFilterRequest
                    {
                        LocationId = locationId,
                        SearchFilter = Tab2Request.SearchFilter,
                        Height = !string.IsNullOrEmpty(Tab2Request.Height) ? string.Join(",", Tab2Request.Height.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                        Age = !string.IsNullOrEmpty(Tab2Request.Age) ? string.Join(",", Tab2Request.Age.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                        BloodType = !string.IsNullOrEmpty(Tab2Request.BloodType) ? string.Join(",", Tab2Request.BloodType.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                        ConstellationGroup = !string.IsNullOrEmpty(Tab2Request.ConstellationGroup) ? string.Join(",", Tab2Request.ConstellationGroup.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                        Occupation = (string.IsNullOrEmpty(Tab2Request.Occupation) || Tab2Request.Occupation.Trim() == "0") ? string.Empty : Tab2Request.Occupation.DecryptParameter(),
                        CustomerId = CustomerId,
                        Type = "1",
                        Skip = Tab2Request.StartIndex,
                        Take = Tab2Request.PageSize,
                    };
                    var dbHostResponse = _searchBusiness.HostPreferenceFilter(dbRequest);
                    Response.FilteredHostModel = ApplicationUtilities.MapObjects<DashboardV2HostDetailModel>(dbHostResponse);
                }
                Response.FilteredHostModel.ForEach(x =>
                {
                    x.ClubId = x.ClubId.EncryptParameter();
                    x.HostId = x.HostId.EncryptParameter();
                    x.ClubLocationId = x.ClubLocationId.EncryptParameter();
                    x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo);
                    x.HostLogo = ImageHelper.ProcessedImage(x.HostLogo);
                });
                Response.RequestModel = ApplicationUtilities.MapObject<HostSearchFilterRequestModel>(Tab2Request);
                ViewBag.StartIndex = Tab2Request.StartIndex;
                ViewBag.TotalRecords = (Response.FilteredHostModel.Count > 0 && !string.IsNullOrEmpty(Response.FilteredHostModel.FirstOrDefault().TotalRecords)) ? Convert.ToInt32(Response.FilteredHostModel.FirstOrDefault().TotalRecords) : 0;
                return View("HostFilter", Response);
            }
            else
            {
                ViewBag.scftab = "01";
                var Response = new SearchV2FilterTab1ResponseModel();
                var clubRecommendationDBResponse = _searchBusinessOld.GetRecommendedClub(locationId);
                Response.RecommendedClubModel = ApplicationUtilities.MapObjects<ClubRecommendationListModel>(clubRecommendationDBResponse);
                Response.RecommendedClubModel.ForEach(x =>
                {
                    x.LocationId = x.LocationId.EncryptParameter();
                    x.ClubId = x.ClubId.EncryptParameter();
                    x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo);
                });
                if (NewClub)
                {
                    ViewBag.scftab = "03";
                    var dbResponse = _searchBusiness.GetNewClub(locationId, CustomerId);
                    Response.FilteredClubModel = ApplicationUtilities.MapObjects<CRS.CUSTOMER.APPLICATION.Models.SearchV2.SearchFilterClubDetailModel>(dbResponse);
                }
                else
                {
                    var dbRequest = new ClubPreferenceFilterRequest
                    {
                        LocationId = locationId,
                        SearchFilter = Tab1Request.SearchFilter,
                        ClubCategory = !string.IsNullOrEmpty(Tab1Request.ClubCategory) ? string.Join(",", Tab1Request.ClubCategory.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                        Price = !string.IsNullOrEmpty(Tab1Request.Price) ? string.Join(",", Tab1Request.Price.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                        Shift = string.IsNullOrEmpty(Tab1Request.Shift) ? string.Empty : Tab1Request.Shift.DecryptParameter(),
                        Time = !string.IsNullOrEmpty(Tab1Request.Time) ? string.Join(",", Tab1Request.Time.Split(',').Select(x => x.DecryptParameter()).Where(x => x != null)).Trim(',')
                           : string.Empty,
                        ClubAvailability = !string.IsNullOrEmpty(Tab1Request.ClubAvailability) ? string.Join(",", Tab1Request.ClubAvailability.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                        CustomerId = CustomerId
                    };
                    var dbResponse = _searchBusiness.ClubPreferenceFilter(dbRequest);
                    Response.FilteredClubModel = ApplicationUtilities.MapObjects<CRS.CUSTOMER.APPLICATION.Models.SearchV2.SearchFilterClubDetailModel>(dbResponse);
                }
                Response.FilteredClubModel.ForEach(x =>
                {
                    x.ClubId = x.ClubId.EncryptParameter();
                    x.ClubLocationId = x.ClubLocationId.EncryptParameter();
                    x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo);
                    x.HostGalleryImage = x.HostGalleryImage.Select(y => ImageHelper.ProcessedImage(y)).ToList();
                });
                return View("ClubFilter", Response);
            }
        }
    }
}