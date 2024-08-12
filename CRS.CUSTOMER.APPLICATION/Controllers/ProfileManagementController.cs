using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models;
using CRS.CUSTOMER.APPLICATION.Models.ProfileManagement;
using CRS.CUSTOMER.APPLICATION.Models.ReservationManagement;
using CRS.CUSTOMER.APPLICATION.Models.UserProfileManagement;
using CRS.CUSTOMER.BUSINESS.ProfileManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ProfileManagement;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Util;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class ProfileManagementController : CustomController
    {
        private readonly IProfileManagementBusiness _business;
        public ProfileManagementController(IProfileManagementBusiness business) => this._business = business;

        [HttpGet, Route("user/account/profile")]
        public ActionResult Index()
        {
            ViewBag.LocationDDL = ApplicationUtilities.LoadDropdownList("LOCATIONDDL", "", "") as Dictionary<string, string>;
            ViewBag.PrefectureDDL = ApplicationUtilities.LoadDropdownList("PREFECTUREDDL", "", "") as Dictionary<string, string>;
            ViewBag.ActionPageName = "NavMenu";
            ViewBag.PageTitle = "プロフィール編集"/*Resources.Resource.ProfileInfo*/;
            if (TempData["UserProfileModel"] != null)
            {
                var userProfileJson = TempData["UserProfileModel"].ToString();
                var userProfileModel = JsonConvert.DeserializeObject<UserProfileModel>(userProfileJson);
                ViewBag.PrefectureKey = userProfileModel.Prefecture;
                ViewBag.PreferredLocationKey = userProfileModel.PreferredLocation;
                userProfileModel.ProfileImage = ImageHelper.ProcessedImage(userProfileModel.ProfileImage);
                return View(userProfileModel);
            }
            var common = new UserProfileCommon()
            {
                ActionUserId = ApplicationUtilities.GetSessionValue("UserId").ToString().DecryptParameter(),
                AgentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter(),
                Session = Session.SessionID,
            };
            var data = _business.GetUserProfileDetail(common);
            var viewModel = data.MapObject<UserProfileModel>();
            if (viewModel.ProfileImage != null)
                viewModel.ProfileImage = ImageHelper.ProcessedImage(viewModel.ProfileImage);
            ViewBag.PrefectureKey = viewModel.Prefecture?.EncryptParameter();
            ViewBag.PreferredLocationKey = viewModel.PreferredLocation?.EncryptParameter();
            var dob = !string.IsNullOrEmpty(viewModel.DateOfBirth) ? Convert.ToDateTime(viewModel.DateOfBirth) : DateTime.Now;
            if (!string.IsNullOrEmpty(viewModel.DateOfBirth))
            {
                viewModel.DOBYear = dob.Year.ToString("D4");
                viewModel.DOBMonth = dob.Month.ToString("D2");
                viewModel.DOBDay = dob.Day.ToString("D2");
            }
            viewModel.PreferredLocation = viewModel.PreferredLocation?.EncryptParameter();
            viewModel.Prefecture = viewModel.Prefecture?.EncryptParameter();
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteCustomereAccount()
        {
            string customerId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();
            if (string.IsNullOrEmpty(customerId))
            {
                AddNotificationMessage(new NotificationModel()
                {
                    Message = "Invalid Profile details",
                    NotificationType = NotificationMessage.WARNING,
                    Title = NotificationMessage.WARNING.ToString(),
                });
                return Json(1);
            }
            string actionUser = Session["username"].ToString();
            string actionIp = ApplicationUtilities.GetIP();
            string actionPlatform = "CUSTOMER";

            var dbResponseInfo = _business.DeleteCustomerAccount(customerId, actionUser, actionIp, actionPlatform);
            dbResponseInfo.Extra1 = "true";
            if (dbResponseInfo != null)
            {
                if (dbResponseInfo.Code == ResponseCode.Success)
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        Message = dbResponseInfo.Message ?? "Your account has been deleted",
                        NotificationType = NotificationMessage.SUCCESS,
                        Title = NotificationMessage.SUCCESS.ToString(),
                    });
                    return Json(new { redirect = Url.Action("LogOff", "Home") });
                }
                AddNotificationMessage(new NotificationModel()
                {
                    Message = dbResponseInfo.Message ?? "Failed",
                    NotificationType = NotificationMessage.INFORMATION,
                    Title = NotificationMessage.INFORMATION.ToString(),
                });
                return Json(1);
            }
            AddNotificationMessage(new NotificationModel()
            {
                Message = "Something went wrong. Please try again later.",
                NotificationType = NotificationMessage.INFORMATION,
                Title = NotificationMessage.INFORMATION.ToString(),
            });
            return Json(1);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateUserProfileDetail(UserProfileModel userProfileModel, string PreferredLocationDDL, string PrefectureDDL)
        {
            userProfileModel.Prefecture = PrefectureDDL;
            userProfileModel.PreferredLocation = PreferredLocationDDL;
            TempData["UserProfileModel"] = JsonConvert.SerializeObject(userProfileModel);
            if (ModelState.IsValid)
            {
                var common = userProfileModel.MapObject<UserProfileCommon>();
                common.ActionUserId = ApplicationUtilities.GetSessionValue("UserId").ToString().DecryptParameter();
                common.AgentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();
                common.IpAddress = ApplicationUtilities.GetIP();
                common.PreferredLocation = PreferredLocationDDL?.DecryptParameter();
                common.Prefecture = PrefectureDDL?.DecryptParameter();
                if (!string.IsNullOrEmpty(userProfileModel.DOBYear) && !string.IsNullOrEmpty(userProfileModel.DOBMonth) && !string.IsNullOrEmpty(userProfileModel.DOBDay))
                {
                    if (userProfileModel.DOBYear.Contains("年"))
                    {
                        userProfileModel.DOBYear = userProfileModel.DOBYear.Replace("年", "");
                    }
                    if (userProfileModel.DOBMonth.Contains("月"))
                    {
                        userProfileModel.DOBMonth = userProfileModel.DOBMonth.Replace("月", "");
                    }
                    if (userProfileModel.DOBDay.Contains("日"))
                    {
                        userProfileModel.DOBDay = userProfileModel.DOBDay.Replace("日", "");
                    }
                    int countYear = userProfileModel.DOBYear.Count(char.IsDigit);
                    if (countYear < 4 || countYear > 4)
                    {
                        AddNotificationMessage(new NotificationModel()
                        {
                            NotificationType = NotificationMessage.SUCCESS,
                            Message = "長さは 4 文字である必要があります",
                            Title = NotificationMessage.SUCCESS.ToString()
                        });
                        return Redirect("/user/account/profile");
                    }
                    int countMonth = userProfileModel.DOBMonth.Count(char.IsDigit);
                    if (countMonth > 2)
                    {
                        AddNotificationMessage(new NotificationModel()
                        {
                            NotificationType = NotificationMessage.SUCCESS,
                            Message = "長さは 2 文字である必要があります",
                            Title = NotificationMessage.SUCCESS.ToString()
                        });
                        return Redirect("/user/account/profile");
                    }

                    int countDay = userProfileModel.DOBDay.Count(char.IsDigit);
                    if (countDay < 2)
                    {
                        AddNotificationMessage(new NotificationModel()
                        {
                            NotificationType = NotificationMessage.SUCCESS,
                            Message = "長さは 2 文字である必要があります",
                            Title = NotificationMessage.SUCCESS.ToString()
                        });
                        return Redirect("/user/account/profile");
                    }
                    common.DateOfBirth = string.Concat(userProfileModel.DOBYear.Trim(), "-", userProfileModel.DOBMonth.Trim(), "-", userProfileModel.DOBDay.Trim());
                }
                CommonDbResponse dbresp = _business.UpdateUserProfileDetail(common);
                if (dbresp.Code == ResponseCode.Success)
                {
                    Session["EmailAddress"] = common.EmailAddress;
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.SUCCESS,
                        Message = dbresp.Message,
                        Title = NotificationMessage.SUCCESS.ToString()
                    });
                    return Redirect("/user/account/profile");
                }
                else
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.ERROR,
                        Message = dbresp.Message,
                        Title = NotificationMessage.ERROR.ToString()
                    });
                    return Redirect("/user/account/profile");
                }

            }
            var errorMessages = ModelState.Where(x => x.Value.Errors.Count > 0)
                                    .SelectMany(x => x.Value.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                                    .ToList();

            AddNotificationMessage(new NotificationModel()
            {
                NotificationType = NotificationMessage.ERROR,
                Message = Resources.Resource.All_fields_are_required,
                Title = NotificationMessage.ERROR.ToString()
            });
            return Redirect("/user/account/profile");
        }

        [HttpGet, Route("user/account/password/edit")]
        public ActionResult ChangePasswordV2()
        {
            ViewBag.ActionPageName = "NavMenu";
            ViewBag.PageTitle = Resources.Resource.ChangePassword;
            return View(new ChangePasswordModel());
        }

        [Route("user/account/password/edit")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePasswordV2(ChangePasswordModel changePasswordModel)
        {
            ViewBag.ActionPageName = "NavMenu";
            ViewBag.PageTitle = Resources.Resource.ChangePassword;
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Where(x => x.Value.Errors.Count > 0)
                                    .SelectMany(x => x.Value.Errors.Select(e => $"{e.ErrorMessage}"))
                                    .ToList();

                var notificationModels = errorMessages.Select(errorMessage => new NotificationModel
                {
                    NotificationType = NotificationMessage.ERROR,
                    Message = errorMessage,
                    Title = NotificationMessage.ERROR.ToString(),
                }).ToArray();
                TempData["ChangePWErrorMessage"] = notificationModels[0].Message;
                return RedirectToAction("ChangePasswordV2", changePasswordModel);
            }
            else
            {
                var passwordCommon = changePasswordModel.MapObject<PasswordCommon>();

                passwordCommon.UserId = ApplicationUtilities.GetSessionValue("UserId").ToString().DecryptParameter();
                passwordCommon.AgentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();
                passwordCommon.IPAddress = ApplicationUtilities.GetIP().ToString();
                passwordCommon.BrowserInfo = ApplicationUtilities.GetBrowserInfo().ToString();
                passwordCommon.Session = Session.SessionID;
                var dbResp = _business.ChangePassword(passwordCommon);
                if (dbResp.Code == ResponseCode.Failed)
                {
                    TempData["ChangePWErrorMessage"] = dbResp.Message;
                    return RedirectToAction("ChangePasswordV2", changePasswordModel);
                }
                if (dbResp.Code == ResponseCode.Success)
                {
                    //AddNotificationMessage(new NotificationModel()
                    //{
                    //    NotificationType = NotificationMessage.SUCCESS,
                    //    Message = dbResp.Message,
                    //    Title = NotificationMessage.SUCCESS.ToString()
                    //});
                    return Redirect("/user/remind/complete?nickname=" + dbResp.Extra1.EncryptParameter());
                    //return Redirect("user/remind/complete", "Home",new { nickname = dbResp.Extra1.EncryptParameter() });

                }
                return RedirectToAction("ChangePasswordV2", changePasswordModel);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ChangeProfileImage(HttpPostedFileBase file)
        {
            var common = new UserProfileCommon();
            string fileName = string.Empty;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];
            }
            if (file != null)
            {
                var contentType = file.ContentType;
                var allowedContentType = AllowedImageContentType();
                var ext = Path.GetExtension(file.FileName);
                if (allowedContentType.Contains(contentType.ToLower()))
                {
                    fileName = $"{AWSBucketFolderNameModel.CUSTOMER}/ProfileImage_{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{ext.ToLower()}";
                    common.ProfileImage = $"/{fileName}";
                }
                else
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.INFORMATION,
                        Message = "Image must be in jpeg, jpg or png format",
                        Title = NotificationMessage.INFORMATION.ToString()
                    });

                    return Json(new { Code = "0", Message = "File Must be .jpg,.png,.jpeg,.heif" });
                }
                common.ActionUserId = ApplicationUtilities.GetSessionValue("UserId").ToString().DecryptParameter();
                common.AgentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();
                var dbresp = _business.ChangeProfileImage(common);
                if (dbresp.Code == ResponseCode.Success)
                {
                    if (file != null) await ImageHelper.ImageUpload(fileName, file);
                    Session["ProfileImage"] = ImageHelper.ProcessedImage(common.ProfileImage);// ImageVirtualPath + common.ProfileImage;
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.SUCCESS,
                        Message = dbresp.Message,
                        Title = NotificationMessage.SUCCESS.ToString()
                    });

                    return Json(new { Code = "0", Message = "Uploaded successfully" });
                }
            }
            AddNotificationMessage(new NotificationModel()
            {
                NotificationType = NotificationMessage.ERROR,
                Message = "Something went wrong please try again",
                Title = NotificationMessage.ERROR.ToString(),
            });
            return Json(new { Code = "1", Message = "Something went wrong please try again" });
        }


        public ActionResult points()
        {
            PointReportModel model = new PointReportModel();
            var AgentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();

            //------ Start debit,credit points of customer (when admin transfer and retrive, after reservation otp confirmation bonus point )---------------//

            var alldbresp = _business.GetCustomerPointsReport(AgentId, "");
            if (alldbresp.Count > 0)
            {

                var allgroupedTransactions = alldbresp
                   .GroupBy(t => t.DayType)  // Group by the Date part of TransactionDate
                   .Select(Group => new
                   {
                       DayType = Group.Select(detail => detail.DayType).FirstOrDefault(),
                       PointReportList = Group.Select(detail => new PointReportDetailModel
                       {
                           TransactionDate = detail.TransactionDate,
                           TransactionMode = detail.TransactionMode,
                           Point = detail.Point,
                           Remark = detail.Remark,
                           TotalPoints = detail.TotalPoints,
                       }).ToList()
                   })
                   .ToList();
                ViewBag.TotalPoint = !string.IsNullOrEmpty(alldbresp[0].TotalPoints) ? alldbresp[0].TotalPoints : "0";
                model.AllPointReportList = allgroupedTransactions.MapObjects<PointDayTypeModel>();
            }


            //------ End debit,credit points of customer (when admin transfer and retrive, after reservation otp confirmation bonus point )---------------//


            //------ Start credit points of customer (when admin transfer, after reservation otp confirmation bonus point )---------------//

            var creditdbresp = _business.GetCustomerPointsReport(AgentId, "CR");
            if (creditdbresp.Count > 0)
            {
                var creditgroupedTransactions = creditdbresp
                   .GroupBy(t => t.DayType)  // Group by the Date part of TransactionDate
                   .Select(Group => new
                   {
                       DayType = Group.Select(detail => detail.DayType).FirstOrDefault(),
                       PointReportList = Group.Select(detail => new PointReportDetailModel
                       {
                           TransactionDate = detail.TransactionDate,
                           TransactionMode = detail.TransactionMode,
                           Point = detail.Point,
                           Remark = detail.Remark,
                           TotalPoints = detail.TotalPoints,
                       }).ToList()
                   })
                   .ToList();

                model.CreditPointReportList = creditgroupedTransactions.MapObjects<PointDayTypeModel>();
            }

            //------ End credit points of customer (when admin transfer, after reservation otp confirmation bonus point )---------------//


            //------ Start deduct points of customer (when admin retrive)---------------//

            var debitdbresp = _business.GetCustomerPointsReport(AgentId, "DR");
            if (debitdbresp.Count > 0)
            {
                var groupedTransactions = debitdbresp
                   .GroupBy(t => t.DayType)  // Group by the Date part of TransactionDate
                   .Select(Group => new
                   {
                       DayType = Group.Select(detail => detail.DayType).FirstOrDefault(),
                       PointReportList = Group.Select(detail => new PointReportDetailModel
                       {
                           TransactionDate = detail.TransactionDate,
                           TransactionMode = detail.TransactionMode,
                           Point = detail.Point,
                           Remark = detail.Remark,
                           TotalPoints = detail.TotalPoints,
                       }).ToList()
                   })
                   .ToList();


                model.DebitPointDayTypeList = groupedTransactions.MapObjects<PointDayTypeModel>();
            }

            //------ End deduct points of customer (when admin retrive)---------------//
            return View(model);
        }
    }
}