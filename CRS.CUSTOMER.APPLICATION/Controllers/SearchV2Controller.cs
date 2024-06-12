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
using DocumentFormat.OpenXml.Spreadsheet;
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
        public ActionResult Index(string prefectures, string area, string target)
        {
            ViewBag.PrefecturesArea = $"/{prefectures}/{area}";
            var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter();
            var locationId = ApplicationUtilities.GetKeyValueFromDictionary(LocationHelper, ViewBag.PrefecturesArea);
            var Response = new SearchV2FilterRequestModel();
            if (!string.IsNullOrEmpty(target))
            {
                if (target.Trim().ToLower() == "host")
                {
                    ViewBag.target = "host";
                    Response.SearchV2FilterHostTabModel.LocationModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("10"));
                    Response.SearchV2FilterHostTabModel.BloodTypeModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("5"));
                    Response.SearchV2FilterHostTabModel.HeightModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("6"));
                    Response.SearchV2FilterHostTabModel.AgeModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("7"));
                    Response.SearchV2FilterHostTabModel.ConstellationModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("8"));
                    Response.SearchV2FilterHostTabModel.OccupationModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("9"), true);
                    var HostConstellationGroup = _commonBusiness.GetDDL("023");
                    HostConstellationGroup.ForEach(x => x.Value = x.Value.EncryptParameter());
                    ViewBag.HostConstellationGroup = HostConstellationGroup;
                }
                else if (target.Trim().ToLower() == "new")
                {
                    ViewBag.target = "new";
                    var dbClubResponse = _dashboardBusiness.GetNewClub(locationId, CustomerId, "1");
                    Response.SearchV2FilterNewTabModel.ClubModel = ApplicationUtilities.MapObjects<DashboardV2ClubDetailModel>(dbClubResponse);
                    Response.SearchV2FilterNewTabModel.ClubModel.ForEach(x => { x.ClubId = x.ClubId.EncryptParameter(); x.ClubLocationId = x.ClubLocationId.EncryptParameter(); x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo); });
                    var dbHostResponse = _dashboardBusiness.GetNewHost(locationId, CustomerId, "1");
                    Response.SearchV2FilterNewTabModel.HostModel = ApplicationUtilities.MapObjects<DashboardV2HostDetailModel>(dbHostResponse);
                    Response.SearchV2FilterNewTabModel.HostModel.ForEach(x => { x.ClubId = x.ClubId.EncryptParameter(); x.HostId = x.HostId.EncryptParameter(); x.ClubLocationId = x.ClubLocationId.EncryptParameter(); x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo); x.HostLogo = ImageHelper.ProcessedImage(x.HostLogo); });
                }
                else if (target.Trim().ToLower() == "map")
                {
                    ViewBag.target = "map";
                    Response.SearchV2FilterMapTabModel.LocationModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("10"));
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
                    ViewBag.target = "club";
                    Response.SearchV2FilterClubTabModel.LocationModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("10"));
                    Response.SearchV2FilterClubTabModel.ClubCategoryModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("2"));
                    Response.SearchV2FilterClubTabModel.PlanPriceModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("3"));
                    Response.SearchV2FilterClubTabModel.ClubAvailabilityModel = DDLHelper.ConvertDictionaryToList(DDLHelper.LoadDropdownList("4"));
                }
            }
            return View(Response);
        }

        [HttpPost, Route("search/{prefectures}/{area}")]
        public ActionResult Index(string prefectures, string area, string target, string scftab, SearchV2FilterClubTabRequestModel ClubTabRequest = null, bool NewClub = false, SearchV2FilterHostTabRequestModel HostTabRequest = null, bool NewHost = false)
        {
            ViewBag.PrefecturesArea = $"/{prefectures}/{area}";
            var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter();
            var locationId = ApplicationUtilities.GetKeyValueFromDictionary(LocationHelper, ViewBag.PrefecturesArea);
            if (!string.IsNullOrEmpty(target) || (!string.IsNullOrEmpty(target) && NewHost) || (!string.IsNullOrEmpty(target) || NewClub))
            {
                if (target.Trim().ToLower() == "host" || NewHost)
                {
                    ViewBag.target = "host";
                    var Response = new SearchV2FilterHostTabResponseModel();
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
                        ViewBag.target = "new";
                        var dbHostResponse = _dashboardBusiness.GetNewHost(locationId, CustomerId);
                        Response.FilteredHostModel = ApplicationUtilities.MapObjects<DashboardV2HostDetailModel>(dbHostResponse);
                    }
                    else
                    {
                        var dbRequest = new HostPreferenceFilterRequest
                        {
                            LocationId = locationId,
                            SearchFilter = HostTabRequest.SearchFilter,
                            Height = !string.IsNullOrEmpty(HostTabRequest.Height) ? string.Join(",", HostTabRequest.Height.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            Age = !string.IsNullOrEmpty(HostTabRequest.Age) ? string.Join(",", HostTabRequest.Age.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            BloodType = !string.IsNullOrEmpty(HostTabRequest.BloodType) ? string.Join(",", HostTabRequest.BloodType.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            ConstellationGroup = !string.IsNullOrEmpty(HostTabRequest.ConstellationGroup) ? string.Join(",", HostTabRequest.ConstellationGroup.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            Occupation = (string.IsNullOrEmpty(HostTabRequest.Occupation) || HostTabRequest.Occupation.Trim() == "0") ? string.Empty : HostTabRequest.Occupation.DecryptParameter(),
                            CustomerId = CustomerId,
                            Type = "1",
                            Skip = HostTabRequest.StartIndex,
                            Take = HostTabRequest.PageSize,
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
                    Response.RequestModel = ApplicationUtilities.MapObject<HostSearchFilterRequestModel>(HostTabRequest);
                    ViewBag.StartIndex = HostTabRequest.StartIndex;
                    ViewBag.TotalRecords = (Response.FilteredHostModel.Count > 0 && !string.IsNullOrEmpty(Response.FilteredHostModel.FirstOrDefault().TotalRecords)) ? Convert.ToInt32(Response.FilteredHostModel.FirstOrDefault().TotalRecords) : 0;
                    return View("HostFilter", Response);
                }
                else
                {
                    ViewBag.target = "club";
                    var Response = new SearchV2FilterClubTabResponseModel();
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
                        ViewBag.target = "new";
                        var dbResponse = _searchBusiness.GetNewClub(locationId, CustomerId);
                        Response.FilteredClubModel = ApplicationUtilities.MapObjects<CRS.CUSTOMER.APPLICATION.Models.SearchV2.SearchFilterClubDetailModel>(dbResponse);
                    }
                    else
                    {
                        var dbRequest = new ClubPreferenceFilterRequest
                        {
                            LocationId = locationId,
                            SearchFilter = ClubTabRequest.SearchFilter,
                            ClubCategory = !string.IsNullOrEmpty(ClubTabRequest.ClubCategory) ? string.Join(",", ClubTabRequest.ClubCategory.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            Price = !string.IsNullOrEmpty(ClubTabRequest.Price) ? string.Join(",", ClubTabRequest.Price.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            Shift = string.IsNullOrEmpty(ClubTabRequest.Shift) ? string.Empty : ClubTabRequest.Shift.DecryptParameter(),
                            Time = !string.IsNullOrEmpty(ClubTabRequest.Time) ? string.Join(",", ClubTabRequest.Time.Split(',').Select(x => x.DecryptParameter()).Where(x => x != null)).Trim(',')
                               : string.Empty,
                            ClubAvailability = !string.IsNullOrEmpty(ClubTabRequest.ClubAvailability) ? string.Join(",", ClubTabRequest.ClubAvailability.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
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
            else if (!string.IsNullOrEmpty(scftab))
            {
                ViewBag.LocationLabel = $"{prefectures}/{area}";
                if (scftab.Trim() == "02" || NewHost)
                {
                    ViewBag.scftab = "02";
                    ViewBag.ActionPageName = "SearchFilter";
                    var Response = new SearchV2FilterHostTabResponseModel();
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
                            SearchFilter = HostTabRequest.SearchFilter,
                            Height = !string.IsNullOrEmpty(HostTabRequest.Height) ? string.Join(",", HostTabRequest.Height.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            Age = !string.IsNullOrEmpty(HostTabRequest.Age) ? string.Join(",", HostTabRequest.Age.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            BloodType = !string.IsNullOrEmpty(HostTabRequest.BloodType) ? string.Join(",", HostTabRequest.BloodType.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            ConstellationGroup = !string.IsNullOrEmpty(HostTabRequest.ConstellationGroup) ? string.Join(",", HostTabRequest.ConstellationGroup.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            Occupation = (string.IsNullOrEmpty(HostTabRequest.Occupation) || HostTabRequest.Occupation.Trim() == "0") ? string.Empty : HostTabRequest.Occupation.DecryptParameter(),
                            CustomerId = CustomerId
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
                    return View("HostSearchResult", Response);
                }
                else
                {
                    ViewBag.scftab = "01";
                    ViewBag.ActionPageName = "SearchFilter";
                    var Response = new SearchV2FilterClubTabResponseModel();
                    var clubRecommendationDBResponse = _searchBusinessOld.GetRecommendedClub(locationId);
                    Response.RecommendedClubModel = clubRecommendationDBResponse.MapObjects<ClubRecommendationListModel>();
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
                        Response.FilteredClubModel = ApplicationUtilities.MapObjects<Models.SearchV2.SearchFilterClubDetailModel>(dbResponse);
                    }
                    else
                    {
                        var dbRequest = new ClubPreferenceFilterRequest
                        {
                            LocationId = locationId,
                            SearchFilter = ClubTabRequest.SearchFilter,
                            ClubCategory = !string.IsNullOrEmpty(ClubTabRequest.ClubCategory) ? string.Join(",", ClubTabRequest.ClubCategory.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            Price = !string.IsNullOrEmpty(ClubTabRequest.Price) ? string.Join(",", ClubTabRequest.Price.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            Shift = string.IsNullOrEmpty(ClubTabRequest.Shift) ? string.Empty : ClubTabRequest.Shift.DecryptParameter(),
                            Time = !string.IsNullOrEmpty(ClubTabRequest.Time) ? string.Join(",", ClubTabRequest.Time.Split(',').Select(x => x.DecryptParameter()).Where(x => x != null)).Trim(',')
                               : string.Empty,
                            ClubAvailability = !string.IsNullOrEmpty(ClubTabRequest.ClubAvailability) ? string.Join(",", ClubTabRequest.ClubAvailability.Split(',').Select(x => x.DecryptParameter())).Trim(',') : string.Empty,
                            CustomerId = CustomerId
                        };
                        var dbResponse = _searchBusiness.ClubPreferenceFilter(dbRequest);
                        Response.FilteredClubModel = ApplicationUtilities.MapObjects<Models.SearchV2.SearchFilterClubDetailModel>(dbResponse);
                    }
                    Response.FilteredClubModel.ForEach(x =>
                    {
                        x.ClubId = x.ClubId.EncryptParameter();
                        x.ClubLocationId = x.ClubLocationId.EncryptParameter();
                        x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo);
                        x.HostGalleryImage = x.HostGalleryImage.Select(y => ImageHelper.ProcessedImage(y)).ToList();
                    });
                    return View("ClubSearchResult", Response);
                }
            }
            return Redirect("/");
        }
    }
}