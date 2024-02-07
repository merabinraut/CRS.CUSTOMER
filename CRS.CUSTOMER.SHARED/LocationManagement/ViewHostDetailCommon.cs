using System.Collections.Generic;

namespace CRS.CUSTOMER.SHARED.LocationManagement
{
    public class ViewHostDetailCommon
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

    public class ViewHostDetailCommonV2
    {
        public string HostId { get; set; }
        public string ClubId { get; set; }
        public string ClubNameEnglish { get; set; }
        public string ClubNameJapanese { get; set; }
        public string ClubLocationName { get; set; }
        public string ClubLogo { get; set; }
        public string HostNameJapanese { get; set; }
        public string HostNameEnglish { get; set; }
        public string HostDOB { get; set; }
        public string HostAge { get; set; }
        public string HostBirthPlace { get; set; }
        public string HostRank { get; set; }
        public string HostIntroduction { get; set; }
        public string HostHeight { get; set; }
        public string HostWebsiteLink { get; set; }
        public string HostTiktokLink { get; set; }
        public string HostTwitterLink { get; set; }
        public string HostInstagramLink { get; set; }
        public string HostFacebookLink { get; set; }
        public string HostLine { get; set; }
        public string HostConstellationGroup { get; set; }
        public string HostPreviousOccupation { get; set; }
        public string HostBloodType { get; set; }
        public string HostLiquorStrength { get; set; }
        public string IsBookmarked { get; set; }
        public string HostLoveCount { get; set; }
        public string CustomerHostCompatibility { get; set; }
        public List<string> HostGalleryImageList { get; set; }
        public List<HostIdentityDetailsCommon> HostIdentityDetailsModel = new List<HostIdentityDetailsCommon>();
    }
    public class HostIdentityDetailsCommon
    {
        public string LabelType { get; set; }
        public string LabelEnglish { get; set; }
        public string LabelJapanese { get; set; }
        public string LabelValue { get; set; }
    }
}
