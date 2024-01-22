namespace CRS.CUSTOMER.APPLICATION.Models.Dashboard
{
    /// <summary>
    /// Banners model for customer dashboard (Display images added by superadmin)
    /// </summary>
    public class BannersModel
    {
        public string BannerId { get; set; }
        public string BannerImage { get; set; }
        public string BannerName { get; set; }
    }
}