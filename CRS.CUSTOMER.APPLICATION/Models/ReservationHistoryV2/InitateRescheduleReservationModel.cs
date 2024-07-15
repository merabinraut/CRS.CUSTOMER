using CRS.CUSTOMER.APPLICATION.Models.ReservationManagementV2;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationHistoryV2
{
    public class InitateRescheduleReservationModel
    {
        public string ClubId { get; set; }
        public string Time { get; set; }
        public string ReservationId { get; set; }
        public string SelectedDate { get; set; }
        public string NoOfPeople { get; set; }
        public List<ClubReservableTimeModel> ClubReservableTimeModel { get; set; } = new List<ClubReservableTimeModel>();
        public List<ReservedTimeSlotModel> ReservedTimeSlotModel { get; set; } = new List<ReservedTimeSlotModel>();
    }
}