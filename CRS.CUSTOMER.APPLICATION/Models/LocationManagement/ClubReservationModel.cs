using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.LocationManagement
{
    public class ClubReservationModel
    {
        public string ClubId { get; set; }
        public string ClubName { get; set; }
        public string NoOfPeople { get; set; }
        public string ReservedDate { get; set; }
        public string ClubOpeningTime { get; set; }
        public string ClubClosingTime { get; set; }
        public string SelectedDate { get; set; }
        public string SelectedHost { get; set; }
        public List<ClubWeeklyScheduleModel> ClubWeeklyScheduleModel { get; set; } = new List<ClubWeeklyScheduleModel>();
    }
}