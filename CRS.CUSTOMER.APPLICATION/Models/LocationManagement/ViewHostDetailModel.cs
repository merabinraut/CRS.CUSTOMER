using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.LocationManagement
{
    public class ViewHostDetailModel
    {
        public string ClubId { get; set; }
        public string LocationId { get; set; }
        public string HostId { get; set; }
        public string HostName { get; set; }
        public string ClubName { get; set; }
        public string ClubNameJapanese { get; set; }
        public string CLubLogo { get; set; }
        public string ConstellationGroup { get; set; }
        public string Address { get; set; }
        public string Height { get; set; }
        public string BloodType { get; set; }
        public string PreviousOccupation { get; set; }
        public string LiquorStrength { get; set; }
        public string DOB { get; set; }
        public string IsBookmarked { get; set; }
        public string LoveCount { get; set; }
        public string HostRank { get; set; }
        public string HostWebsiteLink { get; set; }
        public string HostTiktokLink { get; set; }
        public string HostTwitterLink { get; set; }
        public string HostInstagramLink { get; set; }
        public string HostFacebookLink { get; set; }
        public string HostLine { get; set; }
        public List<string> HostGalleryImageList { get; set; }
    }
}