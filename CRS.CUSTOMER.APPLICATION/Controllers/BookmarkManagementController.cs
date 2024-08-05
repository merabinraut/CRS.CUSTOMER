using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.BookmarkManagement;
using CRS.CUSTOMER.BUSINESS.BookmarkManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.BookmarkManagement;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class BookmarkManagementController : CustomController
    {
        private readonly IBookmarkManagementBusiness _buss;

        public BookmarkManagementController(IBookmarkManagementBusiness buss) => _buss = buss;

        [HttpGet, Route("user/account/bookmark")]
        public ActionResult Index(string bmktab = "01")
        {
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
            var viewModel = new BookmarkManagementModel();
            string agentId = ApplicationUtilities.GetSessionValue("AgentId").ToString();
            agentId = agentId.DecryptParameter();

            if (!string.IsNullOrEmpty(bmktab) && bmktab.Trim() == "02")
            {
                viewModel.bmktab = "02";
                var bookmarkedHosts = _buss.GetBookmarkedHostList(agentId);
                viewModel.BookmarkedHosts = bookmarkedHosts.MapObjects<BookmarkedHostModel>();
                viewModel.BookmarkedHosts.ForEach(x =>
                {
                    x.ClubId = x.ClubId.EncryptParameter();
                    x.HostId = x.HostId.EncryptParameter();
                    x.LocationId = x.LocationId.EncryptParameter();
                    x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo);
                    x.HostImage = ImageHelper.ProcessedImage(x.HostImage);
                });
            }
            else
            {
                viewModel.bmktab = "01";
                var bookmarkedClubs = _buss.GetBookmarkedClubList(agentId);
                viewModel.BookmarkedClubs = bookmarkedClubs.MapObjects<BookmarkedClubModel>();
                viewModel.BookmarkedClubs.ForEach(x =>
                {
                    x.ClubId = x.ClubId.EncryptParameter();
                    x.LocationId = x.LocationId.EncryptParameter();
                    x.ClubCoverPhoto = ImageHelper.ProcessedImage(x.ClubCoverPhoto);
                    x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo);
                    x.ClubWeeklyScheduleList.ForEach(y => y.DayLabel = (!string.IsNullOrEmpty(culture) && culture == "en") ? y.EnglishDay : y.JapaneseDay);
                    x.HostGalleryImage = x.HostGalleryImage.Select(y => ImageHelper.ProcessedImage(y)).ToList();
                });
            }
            ViewBag.ActionPageName = "NavMenu";
            ViewBag.PageTitle = Resources.Resource.Bookmark;
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult ManageBookmark(string clubId, string hostId, string agentType)
        {
            var agentTypes = new List<string>() { "CLUB", "HOST" };
            var cId = string.IsNullOrEmpty(clubId) ? string.Empty : clubId.DecryptParameter();
            var hId = string.IsNullOrEmpty(hostId) ? string.Empty : hostId.DecryptParameter();
            if (string.IsNullOrEmpty(cId)
                || !agentTypes.Contains(agentType.ToUpper()))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.WARNING,
                    Message = "Invalid Details",
                    Title = NotificationMessage.WARNING.ToString()
                });
                return Json(new { success = false, message = "Invalid Details" });
            }

            var common = new ClubHostManagementCommon()
            {
                AgentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter(),
                ClubId = cId,
                HostId = hId
            };

            var dbResp = _buss.ManageBoookmark(common, agentType);
            if (dbResp != null && dbResp.Code == ResponseCode.Success)
                return Json(new { success = true, message = "", type = dbResp?.Extra1 ?? "" });
            else
            {
                //AddNotificationMessage(new NotificationModel()
                //{
                //    NotificationType = NotificationMessage.ERROR,
                //    Message = dbResp?.Message ?? "Something went wrong",
                //    Title = NotificationMessage.ERROR.ToString()
                //});
                return Json(new { success = false, message = dbResp?.Message ?? "Something went wrong" });
            }
        }
    }
}