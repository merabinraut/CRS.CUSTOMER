using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.BUSINESS.CommonManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.CommonManagement;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class CustomController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            HttpCookie langCookie = Request.Cookies["culture"];
            string lang;
            if (langCookie != null)
                lang = langCookie.Value;

            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                    lang = userLang;
                else
                    lang = LanguageMang.GetDefaultLanguage();
            }
            new LanguageMang().SetLanguage(lang);
            //if (Session["AdvertisementImage"] == null)
            //{
            //    var CommonBusiness = new CommonManagementBusiness();
            //    List<AdvertisementCommon> advertisementimage = CommonBusiness.GetAdvertisement();
            //    advertisementimage = advertisementimage
            //        .Select(item => 
            //        {
            //            item.image = ImageHelper.ProcessedImage(item.image);
            //            return item;
            //        })
            //        .ToList();
            //    Session["AdvertisementImage"] = advertisementimage;
            //}
            var CommonBusiness = new CommonManagementBusiness();
            List<AdvertisementCommon> advertisementimage = CommonBusiness.GetAdvertisement();
            advertisementimage = advertisementimage
                .Select(item =>
                {
                    item.image = ImageHelper.ProcessedImage(item.image);
                    return item;
                })
                .ToList();
            Session["AdvertisementImage"] = advertisementimage;
            return base.BeginExecuteCore(callback, state);
        }

        public void AddNotificationMessage(params NotificationModel[] notificationModels)
        {
            // Get existing notifications from TempData or initialize a new list
            var existingNotifications = TempData["Notifications"] as List<NotificationModel> ?? new List<NotificationModel>();

            // Add the new notification models to the existing list
            existingNotifications.AddRange(notificationModels);

            TempData["Notifications"] = existingNotifications;
        }

        public void ClearSessionData(bool ForceCleanCulture = true)
        {
            Session.Abandon();
            Session.RemoveAll();
            Session.Clear();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["culture"] != null && ForceCleanCulture)
            {
                Response.Cookies["culture"].Value = string.Empty;
                Response.Cookies["culture"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Session["AdvertisementImage"] == null)
            {
                var CommonBusiness = new CommonManagementBusiness();
                //var advertisementimage = CommonBusiness.GetAdvertisement();
                //advertisementimage = advertisementimage
                //    .Select(item => ImageHelper.ProcessedImage(item))
                //    .ToList();
                List<AdvertisementCommon> advertisementimage = CommonBusiness.GetAdvertisement();
                advertisementimage = advertisementimage
                    .Select(item =>
                    {
                        item.image = ImageHelper.ProcessedImage(item.image);
                        return item;
                    })
                    .ToList();
                Session["AdvertisementImage"] = advertisementimage;
            }
        }
        public string[] AllowedImageContentType()
        {
            return new[] { "image/png", "image/jpeg", "image/HEIF", "image/heif" };
        }
    }
}