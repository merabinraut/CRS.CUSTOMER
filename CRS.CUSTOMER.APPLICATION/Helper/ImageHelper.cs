using System.Configuration;

namespace CRS.CUSTOMER.APPLICATION.Helper
{
    public static class ImageHelper
    {
        public static string ProcessedImage(string requestImage)
        {
            var response = "/Content/assets/images/customer/demo-image.jpeg";
            var imageVirtualPath = ConfigurationManager.AppSettings["Phase"]?.ToString().ToUpper() != "DEVELOPMENT" ? ConfigurationManager.AppSettings["ImageVirtualPath"]?.ToString() : "";
            return string.IsNullOrEmpty(requestImage) ? response : $"{imageVirtualPath}{requestImage}";
        }

    }
}