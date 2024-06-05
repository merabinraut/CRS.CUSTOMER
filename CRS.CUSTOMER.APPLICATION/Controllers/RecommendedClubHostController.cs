using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.LocationManagement;
using CRS.CUSTOMER.BUSINESS.RecommendedClubHost;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.RecommendedClubHost;
using System.Linq;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class RecommendedClubHostController : CustomController
    {
        private readonly IRecommendedClubHostBusiness _recommendedClubHostBuss;
        public RecommendedClubHostController(IRecommendedClubHostBusiness recommendedClubHostBuss)
        {
            _recommendedClubHostBuss = recommendedClubHostBuss;
        }
        [HttpGet, Route("RecommendedClubHost/GetRecommendedClubAndHost")]
        public JsonResult GetRecommendedClubAndHost(string PositionId, string LocationId)
        {
            ViewBag.LocationId = LocationId;
            var redirectToUrl = string.Empty;
            var culture = ApplicationUtilities.GetSessionValue("culture")?.ToString()?.ToLower();
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
            ViewBag.ActionPageName = "Dashboard";
            var Response = new LocationClubHostModel();
            var recommendedClubDBRequest = new RecommendedClubRequestCommon()
            {
                PositionId = PositionId,
                LocationId = !string.IsNullOrEmpty(LocationId) ? LocationId.DecryptParameter() : null,
                CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter()
            };
            var recommendedHostDBRequest = new RecommendedHostRequestCommon()
            {
                PositionId = PositionId,
                LocationId = !string.IsNullOrEmpty(LocationId) ? LocationId.DecryptParameter() : null,
                CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter()
            };
            if (string.IsNullOrEmpty(recommendedClubDBRequest.PositionId) || string.IsNullOrEmpty(recommendedClubDBRequest.LocationId)
                || string.IsNullOrEmpty(recommendedClubDBRequest.CustomerId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.WARNING,
                    Message = "Invalid request",
                    Title = NotificationMessage.WARNING.ToString()
                });
                redirectToUrl = "/";
                return Json(new { Response, redirectToUrl }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(recommendedHostDBRequest.PositionId) || string.IsNullOrEmpty(recommendedHostDBRequest.LocationId)
                || string.IsNullOrEmpty(recommendedHostDBRequest.CustomerId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.WARNING,
                    Message = "Invalid request",
                    Title = NotificationMessage.WARNING.ToString()
                });
                redirectToUrl = "/";
                return Json(new { Response, redirectToUrl }, JsonRequestBehavior.AllowGet);
            }
            var dbClubResponse = _recommendedClubHostBuss.GetRecommendedClub(recommendedClubDBRequest);
            var dbHostResponse = _recommendedClubHostBuss.GetRecommendedHost(recommendedHostDBRequest);
            Response.ClubListModel = dbClubResponse.MapObjects<LocationClubListModel>();
            Response.HostListModel = dbHostResponse.MapObjects<LocationHostListModel>();
            foreach (var item in Response.ClubListModel)
            {
                item.ClubId = item.ClubId.EncryptParameter();
                item.LocationId = item.LocationId.EncryptParameter();
                item.ClubLogo = ImageHelper.ProcessedImage(item.ClubLogo);
                item.ClubCoverPhoto = ImageHelper.ProcessedImage(item.ClubCoverPhoto);
                for (int i = 0; i < item.ClubGalleryImage.Count(); i++) item.ClubGalleryImage[i] = ImageHelper.ProcessedImage(item.ClubGalleryImage[i]);
                item.ClubWeeklyScheduleList.ForEach(x => x.DayLabel = (!string.IsNullOrEmpty(culture) && culture == "en") ? x.EnglishDay : x.JapaneseDay);
            }
            foreach (var item in Response.HostListModel)
            {
                item.ClubId = item.ClubId.EncryptParameter();
                item.LocationId = item.LocationId.EncryptParameter();
                item.HostId = item.HostId.EncryptParameter();
                item.HostImage = ImageHelper.ProcessedImage(item.HostImage);
            }
            return Json(new { Response, redirectToUrl }, JsonRequestBehavior.AllowGet);
        }
    }
}