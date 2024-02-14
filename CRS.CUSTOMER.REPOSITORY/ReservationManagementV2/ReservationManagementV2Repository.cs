using CRS.CUSTOMER.SHARED;
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
                Response.Message = _dao.ParseColumnValue(dbResponse, "Message").ToString() ?? "Invalid details";
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
    }
}
