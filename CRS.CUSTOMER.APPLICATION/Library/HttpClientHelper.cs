using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CRS.CUSTOMER.APPLICATION.Library
{
    public static class HttpClientHelper
    {
        public static T HttpGetRequest<T>(string url, string username, string password, Dictionary<string, string> headers = null)
        {
            T result = default(T);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json;charset=UTF-8";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            httpWebRequest.Credentials = new NetworkCredential(username, password);
            string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            httpWebRequest.Headers.Add("Authorization", "Basic " + auth);

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    httpWebRequest.Headers.Add(header.Key, header.Value);
                }
            }

            try
            {
                string responseContent;
                using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                if (!string.IsNullOrEmpty(responseContent))
                {
                    result = responseContent.MapResponseString<T>();
                }
            }
            catch (WebException ex)
            {
                HandleWebException(ex);  // Helper method to handle exceptions (optional)
            }

            return result;
        }

        public static async Task<T> HttpGetRequestWithBearerToken<T>(string url, string token, Dictionary<string, string> headers = null)
        {
            T result = default(T);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json;charset=UTF-8";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    httpWebRequest.Headers.Add(header.Key, header.Value);
                }
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)await httpWebRequest.GetResponseAsync())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseContent = await reader.ReadToEndAsync();
                        //result = JsonConvert.DeserializeObject<T>(responseContent);
                        result = responseContent.MapResponseString<T>();
                    }
                }
            }
            catch (WebException ex)
            {
                HandleWebException(ex);  // This handles exceptions like a 401 Unauthorized
            }

            return result;
        }


        public static T HttpPostRequestWithBasicAuth<T>(string url, object model, string username, string password, Dictionary<string, string> headers = null)
        {
            T result = default(T);

            // Create the web request object
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            // Serialize the model to JSON
            byte[] bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(model));

            // Set the method to POST and specify the content type and length
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json;charset=UTF-8";
            httpWebRequest.ContentLength = bytes.Length;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";

            // Add Basic Authentication header
            string authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            httpWebRequest.Headers.Add("Authorization", "Basic " + authValue);

            // Add custom headers if provided
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    httpWebRequest.Headers.Add(header.Key, header.Value);
                }
            }

            try
            {
                // Write the request body (the serialized model)
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                // Handle the response from the server
                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseContent = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            result = responseContent.MapResponseString<T>();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle the WebException to extract more details (optional)
                HandleWebException(ex);
            }

            return result;
        }


        public static T HttpPostRequestWithToken<T>(string url, object model, string token, Dictionary<string, string> headers = null)
        {
            T result = default(T);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            byte[] bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(model));
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json;charset=UTF-8";
            httpWebRequest.ContentLength = bytes.Length;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    httpWebRequest.Headers.Add(header.Key, header.Value);
                }
            }

            try
            {
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                string responseContent;
                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                if (!string.IsNullOrEmpty(responseContent))
                {
                    result = responseContent.MapResponseString<T>();
                }
            }
            catch (WebException ex)
            {
                HandleWebException(ex);  // Helper method to handle exceptions (optional)
            }

            return result;
        }

        public static T HttpPatchRequestWithToken<T>(string url, object model, string token, Dictionary<string, string> headers = null)
        {
            T result = default(T);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            byte[] bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(model));
            httpWebRequest.Method = "PATCH";
            httpWebRequest.ContentType = "application/json;charset=UTF-8";
            httpWebRequest.ContentLength = bytes.Length;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    httpWebRequest.Headers.Add(header.Key, header.Value);
                }
            }

            try
            {
                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

                string responseContent;
                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                if (!string.IsNullOrEmpty(responseContent))
                {
                    result = responseContent.MapResponseString<T>();
                }
            }
            catch (WebException ex)
            {
                HandleWebException(ex);  // Helper method to handle exceptions (optional)
            }

            return result;
        }

        private static void HandleWebException(WebException ex)
        {
            if (ex.Response != null)
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)ex.Response)
                {
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        string errorResponse = streamReader.ReadToEnd();
                        // Log or handle the error
                        Console.WriteLine($"Error: {errorResponse}");
                    }
                }
            }
            else
            {
                // Handle no response case
                Console.WriteLine("No response from server.");
            }
        }

        private static T MapResponseString<T>(this string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                throw;
            }
        }

    }
}