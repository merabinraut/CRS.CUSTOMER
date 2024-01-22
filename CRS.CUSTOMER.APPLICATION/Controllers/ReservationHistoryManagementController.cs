using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.ReservationHistory;
using CRS.CUSTOMER.APPLICATION.Models.ReservationManagement;
using CRS.CUSTOMER.BUSINESS.ReservationManagement;
using CRS.CUSTOMER.SHARED;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class ReservationHistoryManagementController : CustomController
    {
        private readonly IReservationManagementBusiness _reservationManagementBuss;
        public ReservationHistoryManagementController(IReservationManagementBusiness reservationManagementBuss)
        {
            _reservationManagementBuss = reservationManagementBuss;
        }
        [HttpGet]
        public ActionResult History()
        {
            var FileLocationPath = "";
            var CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();
            var dbResponse = _reservationManagementBuss.GetReservationHistory(CustomerId);
            var ResponseModel = dbResponse.MapObjects<ReservationHistoryModel>();
            foreach (var item in ResponseModel)
            {
                item.ClubId = item.ClubId.EncryptParameter();
                item.ReservationId = item.ReservationId.EncryptParameter();
                item.CustomerId = item.CustomerId.EncryptParameter();
                item.InvoiceId = item.InvoiceId.EncryptParameter();
            }
            if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT") FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
            ResponseModel.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
            return View(ResponseModel);
        }

        #region "Customer Reservation Details"
        public ActionResult CustomerReservationDetail(string ReservationId = "")
        {
            var responseInfo = new CustomerReservationDetailModel();
            var reservationId = !string.IsNullOrEmpty(ReservationId) ? ReservationId.DecryptParameter() : null;
            if (!string.IsNullOrEmpty(reservationId))
            {
                var AgentId = ApplicationUtilities.GetSessionValue("AgentId").ToString()?.DecryptParameter();
                var customerDbResponseInfo = _reservationManagementBuss.GetCustomerReservationDetail(reservationId, AgentId);
                responseInfo = customerDbResponseInfo.MapObject<CustomerReservationDetailModel>();
                if (responseInfo != null && !string.IsNullOrEmpty(responseInfo.NickName))
                {
                    var FileLocationPath = "";
                    if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT") FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
                    responseInfo.LogoImage = FileLocationPath + responseInfo.LogoImage;
                    var dbResponse2 = _reservationManagementBuss.GetReservationHostDetails(reservationId);
                    if (dbResponse2 != null && dbResponse2.Count > 0)
                    {
                        responseInfo.ReservationHostDetailModel = dbResponse2.MapObjects<ReservationHostDetailModel>();
                        responseInfo.ReservationHostDetailModel.ForEach(x => x.HostImagePath = FileLocationPath + x.HostImagePath);
                        return View(responseInfo);
                    }
                }
            }
            AddNotificationMessage(new NotificationModel()
            {
                NotificationType = NotificationMessage.INFORMATION,
                Message = "Invalid request",
                Title = NotificationMessage.INFORMATION.ToString()
            });
            return RedirectToAction("History", "ReservationHistoryManagement");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CancelReservation(string reservationId = "")
        {
            string ReservationId = !string.IsNullOrEmpty(reservationId) ? reservationId.DecryptParameter() : null;
            if (string.IsNullOrEmpty(ReservationId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid Reservation Details",
                    NotificationType = NotificationMessage.WARNING,
                    Title = NotificationMessage.WARNING.ToString(),
                });
                return RedirectToAction("History", "ReservationHistoryManagement");
            }
            string actionUser = Session["username"]?.ToString();
            string actionIp = ApplicationUtilities.GetIP();
            string actionPlatform = "CUSTOMER";

            var dbResponseInfo = _reservationManagementBuss.CancelReservation(ReservationId, actionUser, actionIp, actionPlatform);
            dbResponseInfo.Extra1 = "true";
            if (dbResponseInfo != null)
            {
                if (dbResponseInfo.Code == ResponseCode.Success)
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        Message = dbResponseInfo.Message ?? " Your reservation has been cancelled",
                        NotificationType = NotificationMessage.SUCCESS,
                        Title = NotificationMessage.SUCCESS.ToString(),
                    });
                    return Json(dbResponseInfo.SetMessageInTempData(this));
                }
                else
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        Message = dbResponseInfo.Message ?? "Failed",
                        NotificationType = NotificationMessage.INFORMATION,
                        Title = NotificationMessage.INFORMATION.ToString(),
                    });
                    return Json(JsonRequestBehavior.AllowGet);
                }
            }
            AddNotificationMessage(new NotificationModel()
            {
                Message = "Something went wrong. Please try again later.",
                NotificationType = NotificationMessage.INFORMATION,
                Title = NotificationMessage.INFORMATION.ToString(),
            });
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateReservationTimeView(string reservationId = "", string clubId = "")
        {
            string ReservationId = !string.IsNullOrEmpty(reservationId) ? reservationId.DecryptParameter() : null;
            var cId = !string.IsNullOrEmpty(clubId) ? clubId.DecryptParameter() : null;
            if (string.IsNullOrEmpty(ReservationId) || string.IsNullOrEmpty(cId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid Reservation Details",
                    NotificationType = NotificationMessage.WARNING,
                    Title = NotificationMessage.WARNING.ToString(),
                });
                return RedirectToAction("History", "ReservationHistoryManagement");
            }
            ViewBag.ReservationId = reservationId;
            ViewBag.ClubId = clubId;
            ViewBag.ReservableTimeList = ApplicationUtilities.LoadDropdownList("RESERVABLETIMELIST", cId) as Dictionary<string, string>;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateReservationTime(string reservationId = "", string visitedTime = "", string clubId = "")
        {
            var redirectToUrl = string.Empty;
            string ReservationId = !string.IsNullOrEmpty(reservationId) ? reservationId.DecryptParameter() : null;
            if (string.IsNullOrEmpty(ReservationId) || string.IsNullOrEmpty(visitedTime))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid Reservation Details",
                    NotificationType = NotificationMessage.WARNING,
                    Title = NotificationMessage.WARNING.ToString(),
                });
                redirectToUrl = Url.Action("History", "ReservationHistoryManagement");
                return Json(new { redirectToUrl });
            }
            string actionUser = Session["username"]?.ToString();
            string actionIp = ApplicationUtilities.GetIP();
            string actionPlatform = "CUSTOMER";
            var dbResponseInfo = _reservationManagementBuss.UpdateReservationTime(ReservationId, actionUser, actionIp, actionPlatform, visitedTime);
            dbResponseInfo.Extra1 = "true";
            if (dbResponseInfo != null)
            {
                if (dbResponseInfo.Code == ResponseCode.Success)
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        Message = dbResponseInfo.Message ?? " Your reservation time has been updated",
                        NotificationType = NotificationMessage.SUCCESS,
                        Title = NotificationMessage.SUCCESS.ToString(),
                    });
                    redirectToUrl = Url.Action("History", "ReservationHistoryManagement");
                    return Json(new { redirectToUrl });
                }
                else
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        Message = dbResponseInfo.Message ?? "Failed",
                        NotificationType = NotificationMessage.INFORMATION,
                        Title = NotificationMessage.INFORMATION.ToString(),
                    });
                    return Json(new { redirectToUrl });
                }
            }
            AddNotificationMessage(new NotificationModel()
            {
                Message = "Something went wrong. Please try again later.",
                NotificationType = NotificationMessage.INFORMATION,
                Title = NotificationMessage.INFORMATION.ToString(),
            });
            return Json(new { redirectToUrl });
        }
        #endregion
    }

}