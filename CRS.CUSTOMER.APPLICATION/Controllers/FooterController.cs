﻿using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.Enquiry;
using CRS.CUSTOMER.BUSINESS.CommonManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.Enquiry;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    [OverrideActionFilters]
    public class FooterController : CustomController
    {
        private readonly ICommonManagementBusiness _commonBusiness;
        public FooterController(ICommonManagementBusiness commonBusiness)
        {
            _commonBusiness = commonBusiness;
        }
        private void PopulateMetaTagInfo()
        {
            var metaTagDBResponse = _commonBusiness.GetMetaTagInfo("1");
            ViewBag.MetaClubCount = metaTagDBResponse.Item1;
            ViewBag.MetaHostCount = metaTagDBResponse.Item2;
        }

        [HttpGet, Route("customervoice")]
        public ActionResult customervoice()
        {
            PopulateMetaTagInfo();
            return View();
        }

        [HttpGet, Route("faq")]
        public ActionResult faq()
        {
            PopulateMetaTagInfo();
            return View("faqv2");
        }

        [HttpGet, Route("support")]
        public ActionResult Inquiry()
        {
            PopulateMetaTagInfo();
            return View();
        }

        [HttpPost, Route("support")]
        public ActionResult Inquiry(EnquiryRequestModel request)
        {
            var mappedRequest = request.MapObject<EnquiryRequestcommon>();
            var resp = _commonBusiness.PostEnquiryAsync(mappedRequest);
            if (resp.Code == 0)
            {
                AddNotificationMessage(new NotificationModel()
                {
                    NotificationType = NotificationMessage.SUCCESS ,
                    Message = resp.Message,
                    Title = NotificationMessage.SUCCESS.ToString()
                });
                return RedirectToAction("Index", "DashboardV2");
            }
            return RedirectToAction("Index", "DashboardV2");
        }



        [HttpGet, Route("company")]
        public ActionResult operatingcompany()
        {
            PopulateMetaTagInfo();
            return View();
        }

        [HttpGet, Route("policy")]
        public ActionResult privacypolicy()
        {
            PopulateMetaTagInfo();
            return View();
        }

        [HttpGet, Route("rule")]
        public ActionResult termsAndCondition()
        {
            PopulateMetaTagInfo();
            return View();
        }
    }
}