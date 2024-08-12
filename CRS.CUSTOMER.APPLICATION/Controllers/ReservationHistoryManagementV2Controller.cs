using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Amazon.Runtime.Internal.Transform;
using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.ReservationHistoryV2;
using CRS.CUSTOMER.APPLICATION.Models.ReservationManagementV2;
using CRS.CUSTOMER.BUSINESS.ReservationHistoryManagementV2;
using CRS.CUSTOMER.BUSINESS.ReservationManagementV2;
using CRS.CUSTOMER.SHARED;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class ReservationHistoryManagementV2Controller : CustomController
    {
        private readonly IReservationHistoryManagementV2Business _buss;
        private readonly IReservationManagementV2Business _reservationBuss;
        public ReservationHistoryManagementV2Controller(IReservationHistoryManagementV2Business buss, IReservationManagementV2Business reservationBuss)
        {
            _buss = buss;
            _reservationBuss = reservationBuss;
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [HttpGet, Route("user/account/reservation")]
        public ActionResult ReservationHistory(string rsvtab = "04")
        {
            TempData["BackFromMenuBar"] = "BookingHistory";
            ReservationCommonModel responseInfo = new ReservationCommonModel();
            var customerId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();

            if (!string.IsNullOrEmpty(rsvtab) && rsvtab.Trim() == "02")
            {
                #region "Visited History"
                responseInfo.rsvtab = "02";
                var dbVisitedInfo = _buss.GetVisitedHistoryList(customerId);
                responseInfo.GetVisitedHistoryList = dbVisitedInfo.MapObjects<VisitedHistoryModel>();
                foreach (var visitedItem in responseInfo.GetVisitedHistoryList)
                {
                    visitedItem.ClubId = visitedItem.ClubId.EncryptParameter();
                    visitedItem.ReservationId = visitedItem.ReservationId.EncryptParameter();
                    visitedItem.CustomerId = visitedItem.CustomerId.EncryptParameter();
                    if (!string.IsNullOrEmpty(visitedItem.LocationURL) && visitedItem.LocationURL != "#")
                    {
                        if (!visitedItem.LocationURL.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) visitedItem.LocationURL = "https://" + visitedItem.LocationURL;
                    }
                }
                responseInfo.GetVisitedHistoryList.ForEach(x => x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo));
                #endregion
            }
            else if (!string.IsNullOrEmpty(rsvtab) && rsvtab.Trim() == "03")
            {
                #region "Cancelled History"
                responseInfo.rsvtab = "03";
                var dbCancelledInfo = _buss.GetCancelledHistory(customerId);
                responseInfo.GetCancelledHistoryList = dbCancelledInfo.MapObjects<CancelledHistoryModel>();
                foreach (var cancelItem in responseInfo.GetCancelledHistoryList)
                {
                    cancelItem.ClubId = cancelItem.ClubId.EncryptParameter();
                    cancelItem.ReservationId = cancelItem.ReservationId.EncryptParameter();
                    cancelItem.CustomerId = cancelItem.CustomerId.EncryptParameter();
                    cancelItem.LocationId = cancelItem.LocationId.EncryptParameter();
                    if (!string.IsNullOrEmpty(cancelItem.LocationURL) && cancelItem.LocationURL != "#")
                    {
                        if (!cancelItem.LocationURL.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) cancelItem.LocationURL = "https://" + cancelItem.LocationURL;
                    }
                }
                responseInfo.GetCancelledHistoryList.ForEach(x => x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo));
                #endregion
            }
            else if (!string.IsNullOrEmpty(rsvtab) && rsvtab.Trim() == "04")
            {
                #region "All History"
                responseInfo.rsvtab = "04";
                var dbAllInfo = _buss.GetAllHistoryList(customerId);
                responseInfo.GetAllHistoryList = dbAllInfo.MapObjects<AllHistoryModel>();
                foreach (var allItem in responseInfo.GetAllHistoryList)
                {
                    allItem.ClubId = allItem.ClubId.EncryptParameter();
                    allItem.ReservationId = allItem.ReservationId.EncryptParameter();
                    allItem.CustomerId = allItem.CustomerId.EncryptParameter();
                    allItem.LocationId = allItem.LocationId.EncryptParameter();
                    if (!string.IsNullOrEmpty(allItem.LocationURL) && allItem.LocationURL != "#")
                    {
                        if (!allItem.LocationURL.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) allItem.LocationURL = "https://" + allItem.LocationURL;
                    }
                }
                responseInfo.GetAllHistoryList.ForEach(x => x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo));
                #endregion
            }
            else
            {
                #region "Reserved History"
                responseInfo.rsvtab = "01";
                var dbReservedInfo = _buss.GetReservedList(customerId);
                responseInfo.GetReservedList = dbReservedInfo.MapObjects<ReservationHistoryV2Model>();
                foreach (var item in responseInfo.GetReservedList)
                {
                    item.ClubId = item.ClubId.EncryptParameter();
                    item.ReservationId = item.ReservationId.EncryptParameter();
                    item.CustomerId = item.CustomerId.EncryptParameter();
                    if (!string.IsNullOrEmpty(item.LocationURL) && item.LocationURL != "#")
                    {
                        if (!item.LocationURL.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) item.LocationURL = "https://" + item.LocationURL;
                    }
                }
                responseInfo.GetReservedList.ForEach(x => x.ClubLogo = ImageHelper.ProcessedImage(x.ClubLogo));
                #endregion
            }

            return View(responseInfo);
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [HttpGet, Route("user/account/reservation/detail")]
        public ActionResult ViewHistoryDetail(string ReservationId = "")
        {
            string CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();
            string reservationId = "";

            if (!string.IsNullOrEmpty(ReservationId)) reservationId = ReservationId.DecryptParameter();
            ReservationHistoryDetailModel responseinfo = new ReservationHistoryDetailModel();
            var dbResponseInfo = _buss.GetReservationHistoryDetail(CustomerId, reservationId);
            responseinfo = dbResponseInfo.MapObject<ReservationHistoryDetailModel>();
            if (responseinfo.ClubId != null && responseinfo.ClubId != "")
            {
                responseinfo.HImages = responseinfo.HostImages.Split(',');
                responseinfo.ClubLogo = ImageHelper.ProcessedImage(responseinfo.ClubLogo);
                responseinfo.HostIdList = responseinfo.HostId.Split(',');
                if (responseinfo.HImages != null)
                {
                    List<string> updatedImages = new List<string>();
                    foreach (var item in responseinfo.HImages)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            updatedImages.Add(ImageHelper.ProcessedImage(item));
                        }
                        else
                        {
                            updatedImages.Add(item);
                        }

                    }
                    responseinfo.HImages = updatedImages.ToArray();
                }
                if (!string.IsNullOrEmpty(responseinfo.HostId))
                {
                    List<string> UpdatedHostIdList = new List<string>();
                    foreach (var item in responseinfo.HostIdList)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            UpdatedHostIdList.Add(item.Trim());
                        }
                        else
                        {
                            //UpdatedHostIdList.Add(item.Trim());
                        }
                    }
                }
            }
            else
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid Reservation Details",
                    NotificationType = NotificationMessage.WARNING,
                    Title = NotificationMessage.WARNING.ToString(),
                });
                return RedirectToAction("ReservationHistory", "ReservationHistoryManagementV2");
            }

            return View(responseinfo);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult RescheduleReservation(string Selectedhour = "", string Selectedminute = "", string ReservationID = "")
        {
            var redirectToUrl = Url.Action("ReservationHistory", "ReservationHistoryManagementV2");
            if (!string.IsNullOrEmpty(Selectedhour) && !string.IsNullOrEmpty(Selectedminute) && !string.IsNullOrEmpty(ReservationID))
            {
                var commonDBRequest = new Common();
                commonDBRequest.ActionIP = ApplicationUtilities.GetIP();
                commonDBRequest.ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString();
                commonDBRequest.AgentId = ReservationID.DecryptParameter();

                string time = Selectedhour + ":" + Selectedminute;
                var dbResponseInfo = _buss.RescheduleReservation(commonDBRequest, time);
                if (dbResponseInfo != null)
                {
                    if (dbResponseInfo.Code == ResponseCode.Success)
                    {
                        AddNotificationMessage(new NotificationModel()
                        {
                            Message = dbResponseInfo.Message ?? " 予約が更新されました",
                            NotificationType = NotificationMessage.SUCCESS,
                            Title = NotificationMessage.SUCCESS.ToString(),
                        });
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
            }
            else
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid Details",
                    NotificationType = NotificationMessage.WARNING,
                    Title = NotificationMessage.WARNING.ToString(),
                });
            }
            return Json(new { redirectToUrl });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CancelReservation(string RID = "")
        {
            string ReservationId = !string.IsNullOrEmpty(RID) ? ReservationId = RID.DecryptParameter() : null;
            if (string.IsNullOrEmpty(ReservationId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid Reservation Details",
                    NotificationType = NotificationMessage.WARNING,
                    Title = NotificationMessage.WARNING.ToString(),
                });
                return RedirectToAction("ReservationHistory", "ReservationHistoryManagementV2");
            }
            var commonDBRequest = new Common();
            commonDBRequest.ActionIP = ApplicationUtilities.GetIP();
            commonDBRequest.ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString();
            commonDBRequest.AgentId = ReservationId;
            var dbResponseInfo = _buss.CancelReservation(commonDBRequest);
            dbResponseInfo.Extra1 = "true";
            if (dbResponseInfo != null)
            {
                if (dbResponseInfo.Code == ResponseCode.Success)
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        Message = dbResponseInfo.Message ?? "Your reservation has been cancelled",
                        NotificationType = NotificationMessage.SUCCESS,
                        Title = NotificationMessage.SUCCESS.ToString()
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
            else
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Something went wrong. Please try again later.",
                    NotificationType = NotificationMessage.INFORMATION,
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
                return Json(JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult RedoReservation(string RID = "")
        {
            string ReservationId = !string.IsNullOrEmpty(RID) ? ReservationId = RID.DecryptParameter() : null;
            if (string.IsNullOrEmpty(ReservationId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid Reservation details",
                    NotificationType = NotificationMessage.WARNING,
                    Title = NotificationMessage.WARNING.ToString(),
                });
                return RedirectToAction("ReservationHistory", "ReservationHistoryManagementV2");
            }
            var commonDbRequest = new Common();
            commonDbRequest.ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString();
            commonDbRequest.ActionIP = ApplicationUtilities.GetIP();
            commonDbRequest.AgentId = ReservationId;
            var dbResponseInfo = _buss.RedoReservation(commonDbRequest);
            dbResponseInfo.Extra1 = "true";
            if (dbResponseInfo != null)
            {
                if (dbResponseInfo.Code == ResponseCode.Success)
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        Message = dbResponseInfo.Message ?? " Your reservation has been redo",
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
            else
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Something went wrong. Please try again later.",
                    NotificationType = NotificationMessage.INFORMATION,
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
                return Json(JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteReservation(string RID = "")
        {
            string ReservationId = !string.IsNullOrEmpty(RID) ? ReservationId = RID.DecryptParameter() : null;
            if (string.IsNullOrEmpty(ReservationId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid Reservation details",
                    NotificationType = NotificationMessage.WARNING,
                    Title = NotificationMessage.WARNING.ToString(),
                });
                return RedirectToAction("ReservationHistory", "ReservationHistoryManagementV2");
            }
            var commonDbRequest = new Common();
            commonDbRequest.ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString();
            commonDbRequest.ActionIP = ApplicationUtilities.GetIP();
            commonDbRequest.AgentId = ReservationId;
            var dbResponseInfo = _buss.DeleteReservation(commonDbRequest);
            if (dbResponseInfo != null)
            {
                if (dbResponseInfo.Code == ResponseCode.Success)
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        Message = dbResponseInfo.Message ?? " 予約が削除されました",
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
            else
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Something went wrong. Please try again later.",
                    NotificationType = NotificationMessage.INFORMATION,
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        #region Reschedule Reservation
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult RescheduleClubReservation(string Time = "", string ReservationId = "")
        {
            var redirectToUrl = Url.Action("ReservationHistory", "ReservationHistoryManagementV2");
            if (!string.IsNullOrEmpty(Time) && !string.IsNullOrEmpty(ReservationId))
            {
                var commonDBRequest = new Common();
                commonDBRequest.ActionIP = ApplicationUtilities.GetIP();
                commonDBRequest.ActionUser = ApplicationUtilities.GetSessionValue("Username").ToString();
                commonDBRequest.AgentId = ReservationId.DecryptParameter();

                string time = Time;
                var dbResponseInfo = _buss.RescheduleReservation(commonDBRequest, time);
                if (dbResponseInfo != null)
                {
                    if (dbResponseInfo.Code == ResponseCode.Success)
                    {
                        AddNotificationMessage(new NotificationModel()
                        {
                            Message = dbResponseInfo.Message ?? " 予約が更新されました",
                            NotificationType = NotificationMessage.SUCCESS,
                            Title = NotificationMessage.SUCCESS.ToString(),
                        });
                        return Redirect(redirectToUrl);
                    }
                    else
                    {
                        AddNotificationMessage(new NotificationModel()
                        {
                            Message = dbResponseInfo.Message ?? "Failed",
                            NotificationType = NotificationMessage.INFORMATION,
                            Title = NotificationMessage.INFORMATION.ToString(),
                        });
                        return Redirect(redirectToUrl);
                    }
                }
            }
            else
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid Details",
                    NotificationType = NotificationMessage.WARNING,
                    Title = NotificationMessage.WARNING.ToString(),
                });
            }
            return Redirect(redirectToUrl);
        }

        [HttpGet]
        public JsonResult InitateRescheduleReservationProcess(string clubId, string reservedDate, string noOfPeople, string reservationId, string time)
        {
            var culture = Request.Cookies["culture"]?.Value ?? "ja";
            var ResponseModel = new InitateRescheduleReservationModel
            {
                SelectedDate = reservedDate,
                ClubId = clubId,
                ReservationId = reservationId,
                NoOfPeople = noOfPeople,
                Time = time
            };
            var cId = !string.IsNullOrEmpty(clubId) ? clubId.DecryptParameter() : string.Empty;
            var formattedReservedDate = !string.IsNullOrEmpty(reservedDate) ? Convert.ToDateTime(reservedDate).ToString("yyyy-MM-dd") : string.Empty;
            var responseData = new Dictionary<string, object>() { { "code", 1 }, { "message", "Invalid Details" }, { "PartialView", "" }, { "SelectedDate", "" } };
            var clubReservableTimeDBResponse = _reservationBuss.GetClubReservationTime(cId);
            var ReservedTimeSlotDBResponse = _reservationBuss.GetReservedTimeSlot(cId, formattedReservedDate);
            ResponseModel.ClubReservableTimeModel = ApplicationUtilities.MapObjects<ClubReservableTimeModel>(clubReservableTimeDBResponse);
            ResponseModel.ReservedTimeSlotModel = ApplicationUtilities.MapObjects<ReservedTimeSlotModel>(ReservedTimeSlotDBResponse);
            var partialViewString = RenderHelper.RenderPartialViewToString(this, "_RescheduleReservationPopup", ResponseModel);
            responseData["PartialView"] = partialViewString;
            responseData["Code"] = 0;
            responseData["Message"] = "Success";
            responseData["SelectedDate"] = formattedReservedDate;
            responseData["TimeIntervalBySelectedDate"] = Newtonsoft.Json.JsonConvert.SerializeObject(clubReservableTimeDBResponse);
            responseData["ReservedTimeSlot"] = Newtonsoft.Json.JsonConvert.SerializeObject(ReservedTimeSlotDBResponse);
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

