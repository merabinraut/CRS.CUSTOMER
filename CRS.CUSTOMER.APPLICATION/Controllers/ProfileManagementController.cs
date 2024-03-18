using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.ProfileManagement;
using CRS.CUSTOMER.APPLICATION.Models.UserProfileManagement;
using CRS.CUSTOMER.BUSINESS.ProfileManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ProfileManagement;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class ProfileManagementController : CustomController
    {
        private readonly IProfileManagementBusiness _business;
        public ProfileManagementController(IProfileManagementBusiness business) => this._business = business;

        [HttpGet]
        public ActionResult Index()
        {
            var common = new UserProfileCommon()
            {
                ActionUserId = ApplicationUtilities.GetSessionValue("UserId").ToString().DecryptParameter(),
                AgentId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter(),
                Session = Session.SessionID,
            };
            var data = _business.GetUserProfileDetail(common);
            var viewModel = data.MapObject<UserProfileModel>();
            ViewBag.LocationDDL = ApplicationUtilities.LoadDropdownList("LOCATIONDDL", "", "") as Dictionary<string, string>;
            ViewBag.PrefectureDDL = ApplicationUtilities.LoadDropdownList("PREFECTUREDDL", "", "") as Dictionary<string, string>;
            if (ConfigurationManager.AppSettings["Phase"] != null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() == "DEVELOPMENT") ViewBag.FileLocationPath = "";
            else ViewBag.FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
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
            ViewBag.ActionPageName = "NavMenu";
            ViewBag.PageTitle = Resources.Resource.ProfileInfo;
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
                        return RedirectToAction("Index");
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
                        return RedirectToAction("Index");
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
                        return RedirectToAction("Index");
                    }
                    common.DateOfBirth = string.Concat(userProfileModel.DOBYear.Trim(), "-", userProfileModel.DOBMonth.Trim(), "-", userProfileModel.DOBDay.Trim());
                }
                CommonDbResponse dbresp = _business.UpdateUserProfileDetail(common);
                if (dbresp.Code == ResponseCode.Success)
                {
                    Session["EmailAddress"] = common.EmailAddress;
                    Session["EmailAddress"] = common.EmailAddress;
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.SUCCESS,
                        Message = dbresp.Message,
                        Title = NotificationMessage.SUCCESS.ToString()
                    });
                }
                else
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.ERROR,
                        Message = dbresp.Message,
                        Title = NotificationMessage.ERROR.ToString()
                    });

                return RedirectToAction("Index");

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
            return RedirectToAction("Index", userProfileModel);
        }

        [HttpGet]
        public ActionResult ChangePasswordV2()
        {
            ViewBag.ActionPageName = "NavMenu";
            ViewBag.PageTitle = Resources.Resource.ChangePassword;
            return View(new ChangePasswordModel());
        }

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
                //AddNotificationMessage(notificationModels);
                //TempData["ChangePWErrorMessage"] = errorMessage;
                return RedirectToAction("ChangePasswordV2", changePasswordModel);
                //return View(changePasswordModel);
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
                    //AddNotificationMessage(new NotificationModel() { NotificationType = NotificationMessage.ERROR, Message = dbResp.Message });
                    TempData["ChangePWErrorMessage"] = dbResp.Message;
                    return RedirectToAction("ChangePasswordV2", changePasswordModel);
                }
                if (dbResp.Code == ResponseCode.Success)
                {
                    AddNotificationMessage(new NotificationModel()
                    {
                        NotificationType = NotificationMessage.SUCCESS,
                        Message = dbResp.Message,
                        Title = NotificationMessage.SUCCESS.ToString()
                    });
                    return RedirectToAction("LogOff", "Home");
                }
                return RedirectToAction("ChangePasswordV2", changePasswordModel);
            }
        }

        [HttpPost, OverrideActionFilters]
        public JsonResult ChangeProfileImage(HttpPostedFileBase file)
        {
            var common = new UserProfileCommon();

            string FileLocation = "/Content/userupload/customer/";
            string FileLocationPath = "/Content/userupload/customer/";
            string ImageVirtualPath = "";
            if (ConfigurationManager.AppSettings["Phase"] != null
                && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper() != "DEVELOPMENT")
            {
                FileLocation = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString() + FileLocationPath;
                ImageVirtualPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
            }
            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];
            }
            if (file != null)
            {
                var contentType = file.ContentType;
                var allowedContentType = AllowedImageContentType();
                var ext = Path.GetExtension(file.FileName);
                string filepath;
                if (allowedContentType.Contains(contentType.ToLower()))
                {
                    string date = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    string myfilename = date + ext.ToLower();
                    filepath = Path.Combine(Server.MapPath(FileLocation), myfilename);
                    common.ProfileImage = FileLocationPath + myfilename;
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
                    ApplicationUtilities.ResizeImage(file, filepath);
                    Session["ProfileImage"] = ImageVirtualPath + common.ProfileImage;
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
    }
}