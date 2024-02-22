using System.Collections.Generic;

namespace CRS.CUSTOMER.SHARED.ReservationManagementV2
{
    public class InitiateClubReservationCommon
    {
        public ResponseCode Code { get; set; } = ResponseCode.Exception;
        public string Message { get; set; }
        public int TotalNoOfPeople { get; set; } = 0;
        public int MaxNoOfPeopleAllowed { get; set; } = 0;
        public List<ClubReservationScheduleCommon> ClubReservationScheduleModel { get; set; } = new List<ClubReservationScheduleCommon>();
        public List<ClubReservableTimeCommon> ClubReservableTimeModel { get; set; } = new List<ClubReservableTimeCommon>();
    }

    public class ClubReservationScheduleCommon
    {
        public string Date { get; set; }
        public string Schedule { get; set; }
    }

    public class ClubReservableTimeCommon
    {
        public string Time { get; set; }
        public string TimeStatus { get; set; }
    }
}
