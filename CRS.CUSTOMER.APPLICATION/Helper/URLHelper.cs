using CRS.CUSTOMER.APPLICATION.Library;
using System;

namespace CRS.CUSTOMER.APPLICATION.Helper
{
    public static class URLHelper
    {
        public static string EncryptQueryParams(string url)
        {
            Uri uri = new Uri(url);
            string queryString = uri.Query.TrimStart('?');

            string[] queryParams = queryString.Split('&');

            string encryptedQueryString = "";
            foreach (string param in queryParams)
            {
                if (string.IsNullOrEmpty(param))
                    continue;
                string[] parts = param.Split('=');
                string paramName = parts[0];
                string paramValue = parts[1];

                string encryptedValue = ApplicationUtilities.EncryptParameter(paramValue);
                encryptedQueryString += $"{paramName}={encryptedValue}&";
            }
            string encryptedUrl = url.Replace(queryString, encryptedQueryString.TrimEnd('&'));
            return encryptedUrl;
        }
    }
}