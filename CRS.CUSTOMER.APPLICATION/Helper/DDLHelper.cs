using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.CommonModel;
using CRS.CUSTOMER.BUSINESS.CommonManagement;
using CRS.CUSTOMER.SHARED;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRS.CUSTOMER.APPLICATION.Helper
{
    public static class DDLHelper
    {
        public static Dictionary<string, string> LoadDropdownList(string forMethod, string search1 = "", string search2 = "")
        {
            var response = new Dictionary<string, string>();
            var context = HttpContext.Current;
            var culture = context?.Request.Cookies["culture"]?.Value ?? "ja";
            var _CommonBuss = new CommonManagementBusiness();
            var methodMapping = new Dictionary<string, string>
            {
                { "PAYMENTMETHODLIST", "1" }, //get customer payment type
                { "1", "5" }, //get location list with total no of club count
                { "2", "6" }, //get Club Category
                { "3", "7" }, //get plan price list
                { "4", "8" }, //get club availability
                { "5", "9" }, //Blood Group
                { "6", "10" }, //Height list
                { "7", "11" }, //Age Range
                { "8", "12" }, //Zodiac signs
                { "9", "13" } //Occupation list
            };

            if (!methodMapping.TryGetValue(forMethod.Trim(), out var methodCode))
                return response;

            var dbResponse = _CommonBuss.GetDropDown_V2(methodCode, search1, search2);
            dbResponse.ForEach(item => response.Add(item.StaticValue.EncryptParameter(), GetLocalizedLabel(item, culture)));

            return response;
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

        public static List<StaticDataModel> ConvertDictionaryToList(Dictionary<string, string> dictionary, bool HasSelect = false)
        {
            if (dictionary == null)
                return new List<StaticDataModel>();

            if (HasSelect)
            {
                dictionary.Add("0", "選択");
            }
            return dictionary.Select(kvp => new StaticDataModel { StaticValue = kvp.Key, StaticLabel = kvp.Value }).ToList();
        }
        public static List<StaticDataModel> ConvertDictionaryToListJapanese(Dictionary<string, string> dictionary, bool HasSelect = false)
        {
            if (dictionary == null)
                return new List<StaticDataModel>();

            if (HasSelect)
            {
                dictionary.Add("0", "選択");
            }
            return dictionary.Select(kvp => new StaticDataModel { StaticValue = kvp.Key, StaticLabel = kvp.Value }).ToList();
        }

        public static string GetValueForKey(Dictionary<string, string> dictionary, string key)
        {
            if (dictionary.TryGetValue(key, out string value))
                return value;
            else
                return null;
        }
    }
}