using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationManagementV2
{
    public class InitiateClubReservationCommonModel
    {
        public string ClubId { get; set; }
        public int TotalNoOfPeople { get; set; } = 0;
        public int MaxNoOfPeopleAllowed { get; set; } = 0;
        public string SelectedHost { get; set; }
        public List<ClubReservationScheduleModel> ClubReservationScheduleModel { get; set; } = new List<ClubReservationScheduleModel>();
        public List<ClubReservableTimeModel> ClubReservableTimeModel { get; set; } = new List<ClubReservableTimeModel>();
    }

    public class ClubReservationScheduleModel
    {
        public string Date { get; set; }
        public string Schedule { get; set; }
        public string ScheduleLabel { get; set; }
    }

    public class ClubReservableTimeModel
    {
        public string Time { get; set; }
        public string TimeStatus { get; set; }
    }
}