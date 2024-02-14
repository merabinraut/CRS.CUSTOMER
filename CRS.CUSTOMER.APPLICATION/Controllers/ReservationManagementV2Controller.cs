using CRS.CUSTOMER.APPLICATION.Helper;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.ReservationManagementV2;
using CRS.CUSTOMER.BUSINESS.ReservationManagementV2;
using CRS.CUSTOMER.SHARED;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class ReservationManagementV2Controller : CustomController
    {
        private readonly IReservationManagementV2Business _buss;
        public ReservationManagementV2Controller(IReservationManagementV2Business buss)
        {
            _buss = buss;
        }
        [HttpGet]
        public JsonResult InitiateClubReservationProcess(string ClubId, string SelectedDate = "")
        {
            var ResponseModel = new InitiateClubReservationCommonModel();
            var culture = Request.Cookies["culture"]?.Value ?? "ja";
            var cId = !string.IsNullOrEmpty(ClubId) ? ClubId.DecryptParameter() : null;
            var responseData = new Dictionary<string, object> { { "Code", 1 }, { "Message", "Invalid Details" }, { "PartialView", "" }, { "UnreservableDates", "" } };
            var dbResponse = _buss.InitiateClubReservationProcess(cId, SelectedDate);
            if (dbResponse.Code == ResponseCode.Success)
            {
                ResponseModel = dbResponse.MapObject<InitiateClubReservationCommonModel>();
                ResponseModel.ClubId = ClubId;
                var partialViewString = RenderHelper.RenderPartialViewToString(this, "_ReservationPopup", ResponseModel);
                responseData["Code"] = 0;
                responseData["Message"] = "Success";
                responseData["PartialView"] = partialViewString;
                var unReservableDateList = ResponseModel.ClubReservationScheduleModel
                                             .Where(item => !string.IsNullOrEmpty(item.Schedule) && item.Schedule.Trim().ToUpper() != "RESERVABLE")
                                             .Select(item => item.Date)
                                             .ToList();

                responseData["UnreservableDates"] = Newtonsoft.Json.JsonConvert.SerializeObject(unReservableDateList);

            }
            else responseData["Message"] = dbResponse.Message ?? responseData["Message"];
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Plan(string ClubId, string Date, string Time, string NoOfPeople)
        {

        }
    }
}