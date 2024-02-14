//using CRS.CUSTOMER.APPLICATION.Helper;
//using CRS.CUSTOMER.APPLICATION.Library;
//using CRS.CUSTOMER.APPLICATION.Models.ReservationManagement;
//using CRS.CUSTOMER.SHARED;
//using System.Collections.Generic;
//using System.Web.Mvc;

//namespace CRS.CUSTOMER.APPLICATION.Controllers
//{
//    public class ReservationManagementController : CustomController
//    {
//        [HttpPost, ValidateAntiForgeryToken]
//        public JsonResult InitiateClubReservationProcess(string ClubId, string SelectedDate)
//        {
//            var ResponseModel = new InitiateClubReservationCommonModel();
//            var culture = Request.Cookies["culture"]?.Value;
//            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
//            var cId = !string.IsNullOrEmpty(ClubId) ? ClubId.DecryptParameter() : null;
//            var responseData = new Dictionary<string, object>
//            {
//                { "Code", 1 },
//                { "Message", "Invalid Details" },
//                { "PartialView", "" }
//            };
//            var dbResponse = new CommonDbResponse()
//            {
//                Code = 0,
//                Message = "Success"
//            };
//            if (dbResponse.Code == ResponseCode.Success)
//            {
//                var partialViewString = RenderHelper.RenderPartialViewToString(this, "_ReservationPopup", ResponseModel);
//                responseData["Code"] = 0;
//                responseData["Message"] = "Success";
//                responseData["PartialView"] = partialViewString;
//            }
//            else responseData["Message"] = dbResponse.Message ?? responseData["Message"];
//            return Json(responseData, JsonRequestBehavior.AllowGet);
//        }
//        [HttpPost, ValidateAntiForgeryToken]
//        public JsonResult CheckClubReservationStatus(string ClubId, string SelectedDate)
//        {
//            var culture = Request.Cookies["culture"]?.Value;
//            culture = string.IsNullOrEmpty(culture) ? "ja" : culture;
//            var cId = !string.IsNullOrEmpty(ClubId) ? ClubId.DecryptParameter() : null;
//            if (string.IsNullOrEmpty(cId)) return Json(new { Code = "1", Message = "Invalid Details" });
//            ViewBag.AllowedNoOfPeopleList = ApplicationUtilities.LoadDropdownList("ALLOWEDNOOFPEOPLELIST") as Dictionary<string, string>;
//            ViewBag.ReservableTimeList = ApplicationUtilities.LoadDropdownList("RESERVABLETIMELIST", cId) as Dictionary<string, string>;
//            // Reservation validation section
//            var dbResponse = new CommonDbResponse()
//            {
//                Code = 0,
//                Message = "Success"
//            };
//            //Reservation validation section
//            return Json(new { Code = dbResponse.Code, Message = dbResponse.Message ?? "Invalid Details" });
//        }
//    }
//}