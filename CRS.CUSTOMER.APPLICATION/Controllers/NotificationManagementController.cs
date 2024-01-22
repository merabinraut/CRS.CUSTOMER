using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.NotificationManagement;
using CRS.CUSTOMER.BUSINESS.NotificationManagement;
using CRS.CUSTOMER.SHARED.NotificationManagement;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class NotificationManagementController : CustomController
    {
        private readonly INotificationManagementBusiness _buss;
        public NotificationManagementController(INotificationManagementBusiness buss) => _buss = buss;
        [HttpGet]
        public ActionResult ViewAllNotifications()
        {
            var requestCommon = new ManageNotificationCommon()
            {
                ActionUserId = ApplicationUtilities.GetSessionValue("UserId").ToString().DecryptParameter(),
                AgentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter(),
            };
            var dbResponse = _buss.GetAllNotification(requestCommon);

            var responseModel = new List<NotificationDetailModel>();
            responseModel = dbResponse.MapObjects<NotificationDetailModel>();
            ViewBag.ActionPageName = "NavMenu";
            //if (responseModel.Count > 0)
            //    ViewBag.PageTitle = $"{Resources.Resource.Notifications} ({responseModel.Count})";
            //else
            ViewBag.PageTitle = Resources.Resource.Notifications;

            return View(responseModel);
        }
    }
}