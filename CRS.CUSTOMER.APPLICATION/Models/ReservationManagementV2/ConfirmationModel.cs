using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationManagementV2
{
    public class ConfirmationViewModel
    {
        public List<HostListV2Model> HostListModel { get; set; } = new List<HostListV2Model>();
        public ClubBasicDetailModel ClubDetailModel { get; set; } = new ClubBasicDetailModel();
    }
}