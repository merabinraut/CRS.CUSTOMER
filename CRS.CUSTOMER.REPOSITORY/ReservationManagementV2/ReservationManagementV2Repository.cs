using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.LocationManagement;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagement;
using CRS.CUSTOMER.SHARED.ReservationManagementV2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.ReservationManagementV2
{
    public class ReservationManagementV2Repository : IReservationManagementV2Repository
    {
        private readonly RepositoryDao _dao;
        public ReservationManagementV2Repository()
        {
            _dao = new RepositoryDao();
        }

        #region InitiateClubReservationProcess
        public InitiateClubReservationCommon InitiateClubReservationProcess(string ClubId, string SelectedDate = "")
        {
            var Response = new InitiateClubReservationCommon();
            string SQL = "EXEC sproc_cp_reservation_management @Flag = 'ccrs'";
            SQL += ", @ClubId=" + _dao.FilterString(ClubId);
            SQL += !string.IsNullOrEmpty(SelectedDate) ? ", @SelectedDate=" + _dao.FilterString(SelectedDate) : "";
            var dbResponse = _dao.ExecuteDataRow(SQL);
            if (dbResponse != null && _dao.ParseColumnValue(dbResponse, "Code").ToString().Trim() == "0")
            {
                Response.Code = ResponseCode.Success;
                Response.Message = _dao.ParseColumnValue(dbResponse, "Message").ToString() ?? "Success";
                Response.MaxNoOfPeopleAllowed = !string.IsNullOrEmpty(_dao.ParseColumnValue(dbResponse, "MaxNoOfPeopleAllowed").ToString()) ? Convert.ToInt32(_dao.ParseColumnValue(dbResponse, "MaxNoOfPeopleAllowed").ToString()) : 0;
                Response.TotalNoOfPeople = !string.IsNullOrEmpty(_dao.ParseColumnValue(dbResponse, "TotalNoOfPeople").ToString()) ? Convert.ToInt32(_dao.ParseColumnValue(dbResponse, "TotalNoOfPeople").ToString()) : 0;
                Response.ClubReservationScheduleModel = GetClubSchedule(ClubId);
                Response.ClubReservableTimeModel = GetClubReservationTime(ClubId);
            }
            else
            {
                Response.Code = ResponseCode.Failed;
                Response.Message = _dao.ParseColumnValue(dbResponse, "Message").ToString() ?? "Invalid request";
            }
            return Response;
        }

        private List<ClubReservationScheduleCommon> GetClubSchedule(string ClubId)
        {
            string SQL = "EXEC sproc_cp_reservation_management @Flag = 'gcds'";
            SQL += ", @ClubId=" + _dao.FilterString(ClubId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            return (dbResponse != null && dbResponse.Rows.Count > 0) ? _dao.DataTableToListObject<ClubReservationScheduleCommon>(dbResponse).ToList() : new List<ClubReservationScheduleCommon>();
        }

        private List<ClubReservableTimeCommon> GetClubReservationTime(string ClubId)
        {
            string SQL = "EXEC sproc_cp_reservation_management @Flag = 'gcrtd'";
            SQL += ", @ClubId=" + _dao.FilterString(ClubId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            return (dbResponse != null && dbResponse.Rows.Count > 0) ? _dao.DataTableToListObject<ClubReservableTimeCommon>(dbResponse).ToList() : new List<ClubReservableTimeCommon>();
        }
        #endregion

        #region  Verify club and get club details
        public Tuple<ResponseCode, string, ClubBasicDetailCommon> VerifyAndGetClubBasicDetail(string ClubId)
        {
            string SQL = "EXEC sproc_cp_reservation_management @Flag = 'vngcd', @ClubId=" + _dao.FilterString(ClubId);
            var dbResponse = _dao.ExecuteDataRow(SQL);
            if (dbResponse != null && _dao.ParseColumnValue(dbResponse, "Code").ToString().Trim() == "0")
            {
                return new Tuple<ResponseCode, string, ClubBasicDetailCommon>(ResponseCode.Success, "Success", new ClubBasicDetailCommon
                {
                    ClubId = _dao.ParseColumnValue(dbResponse, "ClubId").ToString(),
                    ClubNameEnglish = _dao.ParseColumnValue(dbResponse, "ClubNameEnglish").ToString(),
                    ClubNameJapanese = _dao.ParseColumnValue(dbResponse, "ClubNameJapanese").ToString(),
                    ClubLocationName = _dao.ParseColumnValue(dbResponse, "ClubLocationName").ToString()
                });
            }

            return new Tuple<ResponseCode, string, ClubBasicDetailCommon>(ResponseCode.Failed, _dao.ParseColumnValue(dbResponse, "Code").ToString() ?? "Invalid request", new ClubBasicDetailCommon());
        }
        #endregion

        #region check if the customer can proceed with the reservation process
        public Tuple<ResponseCode, string> IsReservationProcessValid(string ClubId, string CustomerId, string SelectedDate, string SelectedTime, string NoOfPeople)
        {
            string SQL = $"EXEC sproc_cp_reservation_management @Flag = 'cirpiv', @ClubId={_dao.FilterString(ClubId)}, @CustomerId={_dao.FilterString(CustomerId)}, " +
                $"@SelectedDate={_dao.FilterString(SelectedDate)}, @SelectedTime={_dao.FilterString(SelectedTime)}, @NoOfPeople={_dao.FilterString(NoOfPeople)}";
            var dbResponse = _dao.ExecuteDataRow(SQL);
            if (dbResponse != null && _dao.ParseColumnValue(dbResponse, "Code").ToString().Trim() == "0")
                return new Tuple<ResponseCode, string>(ResponseCode.Success, _dao.ParseColumnValue(dbResponse, "Message").ToString() ?? "Success");

            return new Tuple<ResponseCode, string>(ResponseCode.Failed, _dao.ParseColumnValue(dbResponse, "Message").ToString() ?? "Invalid request");
        }
        #endregion

        #region Plan 
        public Tuple<ResponseCode, string, List<PlanV2Common>> GetPlans(string ClubId, string CustomerId)
        {
            string SQL = $"EXEC sproc_cp_reservation_management @Flag = 'gpl', @ClubId={_dao.FilterString(ClubId)}, @CustomerId={_dao.FilterString(CustomerId)}";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null)
            {
                var Code = _dao.ParseColumnValue(dbResponse.Rows[0], "Code").ToString();
                var Message = _dao.ParseColumnValue(dbResponse.Rows[0], "Message").ToString();
                if (!string.IsNullOrEmpty(Code))
                {
                    if (Code.Trim() == "0")
                        return new Tuple<ResponseCode, string, List<PlanV2Common>>(ResponseCode.Success, "Success", _dao.DataTableToListObject<PlanV2Common>(dbResponse).ToList());
                    if (Code.Trim() == "9")
                        return new Tuple<ResponseCode, string, List<PlanV2Common>>(ResponseCode.Exception, Message ?? "Invalid request", new List<PlanV2Common>());
                }
            }
            return new Tuple<ResponseCode, string, List<PlanV2Common>>(ResponseCode.Failed, _dao.ParseColumnValue(dbResponse.Rows[0], "Message").ToString() ?? "Invalid request", new List<PlanV2Common>());
        }
        #endregion

        #region Host
        public List<HostListV2Common> GetHostList(string ClubId)
        {
            string SQL = "EXEC sproc_cp_reservation_management @Flag = 'hlvc', @ClubId=" + _dao.FilterString(ClubId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null) return _dao.DataTableToListObject<HostListV2Common>(dbResponse).ToList();
            return new List<HostListV2Common>();
        }
        #endregion

        #region Get host details by club and host id
        public List<HostListV2Common> GetSelectedHostDetail(string ClubId, string HostListId)
        {
            string SQL = $"EXEC sproc_cp_reservation_management @Flag = 'ghlfr', @ClubId={_dao.FilterString(ClubId)}, @HostListId={_dao.FilterString(HostListId)}";
            SQL += ",@HostIdList=" + _dao.FilterString(HostListId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<HostListV2Common>(dbResponse).ToList();
            return new List<HostListV2Common>();
        }
        #endregion
    }
}
