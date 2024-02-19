using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.BUSINESS.CommonManagement;
using CRS.CUSTOMER.SHARED;
using System.Collections.Generic;
using System.Web;

namespace CRS.CUSTOMER.APPLICATION.Helper
{
    public static class DDLHelper
    {
        public static object LoadDropdownList(string forMethod, string search1 = "", string search2 = "")
        {
            var response = new Dictionary<string, string>();
            var context = HttpContext.Current;
            if (context == null)
                return response;//throw new InvalidOperationException("HttpContext not available.");

            var culture = context.Request.Cookies["culture"]?.Value ?? "ja";
            var _CommonBuss = new CommonManagementBusiness();
            var dbResponse = new List<StaticDataCommon>();
            switch (forMethod.ToUpper())
            {
                case "PAYMENTMETHODLIST":
                    dbResponse = _CommonBuss.GetDropDown_V2("1", search1, search2);
                    dbResponse.ForEach(item => { response.Add(item.StaticValue.EncryptParameter(), GetLocalizedLabel(item, culture)); });
                    return response;

                default:
                    return response;
            }
        }

        private static string GetLocalizedLabel(StaticDataCommon item, string culture)
        {
            switch (culture.ToLower())
            {
                case "ja":
                    return item.StaticLabelJapanese;
                case "en":
                    return item.StaticLabelEnglish;

                default:
                    return item.StaticLabelJapanese;
            }
        }
    }
}