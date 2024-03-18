using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.BookmarkManagement;
using CRS.CUSTOMER.BUSINESS.BookmarkManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.BookmarkManagement;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class BookmarkManagementController : CustomController
    {
        private readonly IBookmarkManagementBusiness _buss;

        public BookmarkManagementController(IBookmarkManagementBusiness buss) => _buss = buss;

        [HttpGet]
        [Route("Bookmark")]
        public ActionResult Index()
        {
            var culture = Request.Cookies["culture"]?.Value;
            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
            var viewModel = new BookmarkManagementModel();
            string agentId = ApplicationUtilities.GetSessionValue("AgentId").ToString();
            agentId = agentId.DecryptParameter();
            var FileLocationPath = "";
            if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT")
                FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();

            var bookmarkedClubs = _buss.GetBookmarkedClubList(agentId);
            viewModel.BookmarkedClubs = bookmarkedClubs.MapObjects<BookmarkedClubModel>();
            viewModel.BookmarkedClubs.ForEach(x =>
            {
                x.ClubId = x.ClubId.EncryptParameter();
                x.LocationId = x.LocationId.EncryptParameter();
                x.ClubCoverPhoto = FileLocationPath + x.ClubCoverPhoto;
                x.ClubLogo = FileLocationPath + x.ClubLogo;
                x.ClubWeeklyScheduleList.ForEach(y => y.DayLabel = (!string.IsNullOrEmpty(culture) && culture == "en") ? y.EnglishDay : y.JapaneseDay);
                x.HostGalleryImage = x.HostGalleryImage.Select(y => FileLocationPath + y).ToList();
                //x.HostGalleryImage.Take(3).ToList().ForEach(y => y = FileLocationPath + y);
                //for (int i = 0; i < x.ClubGalleryImage.Count; i++) x.ClubGalleryImage[i] = FileLocationPath + x.ClubGalleryImage[i];
            });

            var bookmarkedHosts = _buss.GetBookmarkedHostList(agentId);
            viewModel.BookmarkedHosts = bookmarkedHosts.MapObjects<BookmarkedHostModel>();
            viewModel.BookmarkedHosts.ForEach(x =>
            {
                x.ClubId = x.ClubId.EncryptParameter();
                x.HostId = x.HostId.EncryptParameter();
                x.LocationId = x.LocationId.EncryptParameter();
                x.ClubLogo = FileLocationPath + x.ClubLogo;
                x.HostImage = FileLocationPath + x.HostImage;
            });
            ViewBag.ActionPageName = "NavMenu";
            ViewBag.PageTitle = Resources.Resource.Bookmark;
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult AddBookmarks(string clubId, string hostId, string agentType)
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
                HostId = hId,
                Status = "A"
            };

            var dbResp = _buss.ManageBoookmark(common, agentType);
            if (dbResp != null && dbResp.Code == ResponseCode.Success)
                return Json(new { success = true, message = "" });
            else
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.ERROR,
                    Message = dbResp?.Message ?? "Something went wrong",
                    Title = NotificationMessage.ERROR.ToString()
                });
                return Json(new { success = false, message = "Something went wrong" });
            }

            //if (dbResp != null && dbResp.Code == ResponseCode.Success)
            //{
            //    AddNotificationMessage(new NotificationModel()
            //    {
            //        NotificationType = NotificationMessage.SUCCESS,
            //        Message = dbResp?.Message,
            //        Title = NotificationMessage.SUCCESS.ToString()
            //    });
            //    return Json(new { success = true, message = dbResp?.Message });
            //}
            //else
            //{
            //    AddNotificationMessage(new NotificationModel()
            //    {
            //        NotificationType = NotificationMessage.ERROR,
            //        Message = dbResp?.Message ?? "Something went wrong",
            //        Title = NotificationMessage.ERROR.ToString()
            //    });
            //    return Json(new { success = false, message = dbResp?.Message ?? "Something went wrong" });
            //}

        }

        [HttpPost]
        public JsonResult RemoveBookmarks(string clubId, string hostId, string agentType)
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
                HostId = hId,
                Status = "B"
            };

            var dbResp = _buss.ManageBoookmark(common, agentType);
            if (dbResp != null && dbResp.Code == ResponseCode.Success)
                return Json(new { success = true, message = "" });
            else
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.ERROR,
                    Message = dbResp?.Message ?? "Something went wrong",
                    Title = NotificationMessage.ERROR.ToString()
                });
                return Json(new { success = false, message = "Something went wrong" });
            }
            //if (dbResp != null && dbResp.Code == ResponseCode.Success)
            //{
            //    AddNotificationMessage(new NotificationModel()
            //    {
            //        NotificationType = NotificationMessage.SUCCESS,
            //        Message = dbResp?.Message,
            //        Title = NotificationMessage.SUCCESS.ToString()
            //    });
            //    return Json(new { success = true, message = dbResp?.Message });
            //}
            //else
            //{
            //    AddNotificationMessage(new NotificationModel()
            //    {
            //        NotificationType = NotificationMessage.ERROR,
            //        Message = dbResp?.Message ?? "Something went wrong",
            //        Title = NotificationMessage.ERROR.ToString()
            //    });
            //    return Json(new { success = false, message = dbResp?.Message ?? "Something went wrong" });
            //}
        }
    }
}