using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationManagementV2
{
    public class HostViewV2Model
    {
        public List<HostListV2Model> HostListModel { get; set; } = new List<HostListV2Model>();
        public ClubBasicDetailModel ClubDetailModel { get; set; } = new ClubBasicDetailModel();
    }
    public class HostListV2Model
    {
        public string ClubId { get; set; }
        public string HostId { get; set; }
        public string HostNameEnglish { get; set; }
        public string HostNameJapanese { get; set; }
        public string HostImage { get; set; }
        public string HostPosition { get; set; }
    }
}