﻿using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models;
using CRS.CUSTOMER.APPLICATION.Models.NotificationManagement;
using CRS.CUSTOMER.BUSINESS.NotificationManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.NotificationManagement;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class NotificationManagementController : CustomController
    {
        private readonly INotificationManagementBusiness _buss;
        private static AmazonS3Configruation _AmazonS3Configruation = ApplicationUtilities.GetAppDataJsonConfigValue<AmazonConfigruation>("AmazonConfigruation").AmazonS3Configruation;
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
            if (responseModel?.FirstOrDefault()?.UnReadNotification != null &&
                    int.TryParse(responseModel.FirstOrDefault().UnReadNotification, out int unReadCount) &&
                    unReadCount > 0)
                ViewBag.PageTitle = $"{Resources.Resource.Notifications} ({responseModel.FirstOrDefault().UnReadNotification})";
            else
                ViewBag.PageTitle = Resources.Resource.Notifications;
            responseModel.ForEach(x =>
            {
                x.NotificationId = x.NotificationId.EncryptParameter();
                x.NotificationURL = (!string.IsNullOrEmpty(x.NotificationURL) && x.NotificationURL.Trim() != "#") ? URLHelper.EncryptQueryParams(x.NotificationURL) : "#";
                x.NotificationImage = ImageHelper.ProcessedImage(x.NotificationImage, false, $"{_AmazonS3Configruation.BaseURL}/{_AmazonS3Configruation.BucketName}/{_AmazonS3Configruation.NotificationNoImageURL.TrimStart('/')}");
            });
            return View(responseModel);
        }

        [HttpPost]
        public JsonResult HasUnReadNotification()
        {
            var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString();
            CustomerId = !string.IsNullOrEmpty(CustomerId) ? CustomerId.DecryptParameter() : null;
            if (!string.IsNullOrEmpty(CustomerId))
            {
                var dbResponse = _buss.HasUnReadNotification(CustomerId);
                if (dbResponse)
                {
                    return Json(new { HasUnReadNotification = true });
                }
            }
            return Json(new { HasUnReadNotification = false });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ManageNotificationReadStatus(string notificationId = "")
        {
            string NotificationId = "";
            if (!string.IsNullOrEmpty(notificationId))
                NotificationId = notificationId.DecryptParameter();
            var dbRequest = new Common()
            {
                AgentId = !string.IsNullOrEmpty(ApplicationUtilities.GetSessionValue("AgentId").ToString()) ? ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter() : null,
                ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString(),
                ActionIP = ApplicationUtilities.GetIP()
            };
            if (!string.IsNullOrEmpty(dbRequest.AgentId) && !string.IsNullOrEmpty(dbRequest.ActionUser))
            {
                var dbResponse = _buss.ManageNotificationReadStatus(dbRequest, NotificationId);
                if (dbResponse != null && dbResponse.Code == ResponseCode.Success) return Json(new { Code = "0", Message = dbResponse.Message ?? "Success", PageTitle = Resources.Resource.Notifications });
                return Json(new { Code = "1", Message = dbResponse.Message ?? "Invalid request" });
            }
            return Json(new { Code = "1", Message = "Something went wrong. Please try again later." });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ManageSingleNotificationReadStatus(string notificationId = "")
        {
            string NotificationId = "";
            if (!string.IsNullOrEmpty(notificationId))
                NotificationId = notificationId.DecryptParameter();
            var dbRequest = new Common()
            {
                AgentId = !string.IsNullOrEmpty(ApplicationUtilities.GetSessionValue("AgentId").ToString()) ? ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter() : null,
                ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString(),
                ActionIP = ApplicationUtilities.GetIP()
            };
            if (!string.IsNullOrEmpty(dbRequest.AgentId) && !string.IsNullOrEmpty(dbRequest.ActionUser) && !string.IsNullOrEmpty(NotificationId))
            {
                var dbResponse = _buss.ManageSingleNotificationReadStatus(dbRequest, NotificationId);
                if (dbRequest != null && dbResponse.Code == ResponseCode.Success) return Json(new { Code = "0", Message = dbResponse.Message ?? "Success", PageTitle = Resources.Resource.Notifications });
                return Json(new
                {
                    Code = "1",
                    Message = dbResponse.Message ?? "Invalid request"
                });
            }
            return Json(new { Code = "1", Message = "Something went wrong. Please try again later" });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult CustomerReservationCancelRemark(string NotificationId, string CustomerRemarks)
        {
            var dbRequest = new Common()
            {
                AgentId = !string.IsNullOrEmpty(ApplicationUtilities.GetSessionValue("AgentId").ToString()) ? ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter() : null,
                ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString(),
                //NotificationId=NotificationId.DecryptParameter()
            };
            if (!string.IsNullOrEmpty(dbRequest.AgentId) && !string.IsNullOrEmpty(dbRequest.ActionUser))
            {
                var dbResponse = _buss.ManageReservationCancelRemark(dbRequest, NotificationId.DecryptParameter(), CustomerRemarks);
                if (dbResponse != null && dbResponse.Code == ResponseCode.Success) return Json(new { Code = "0", Message = dbResponse.Message ?? "Success", PageTitle = Resources.Resource.Notifications });
                return Json(new { Code = "1", Message = dbResponse.Message ?? "Invalid request" });
            }
            return Json(new { Code = "1", Message = "Something went wrong. Please try again later." });
        }
    }
}