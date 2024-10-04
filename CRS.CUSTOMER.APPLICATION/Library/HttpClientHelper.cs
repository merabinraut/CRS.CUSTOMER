using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CRS.CUSTOMER.APPLICATION.Library
{
    public static class HttpClientHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<T> HttpGetRequestAsync<T>(string url, string username, string password, Dictionary<string, string> headers = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            AddBasicAuth(requestMessage, username, password);
            AddHeaders(requestMessage, headers);

            return await SendRequestAsync<T>(requestMessage);
        }

        public static async Task<T> HttpGetRequestWithBearerTokenAsync<T>(string url, string token, Dictionary<string, string> headers = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            AddHeaders(requestMessage, headers);

            return await SendRequestAsync<T>(requestMessage);
        }

        public static async Task<T> HttpPostRequestAsync<T>(string url, object model, Dictionary<string, string> headers = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
            };
            AddHeaders(requestMessage, headers);

            return await SendRequestAsync<T>(requestMessage);
        }

        public static async Task<T> HttpPostRequestWithTokenAsync<T>(string url, object model, string token, Dictionary<string, string> headers = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            AddHeaders(requestMessage, headers);

            return await SendRequestAsync<T>(requestMessage);
        }

        public static async Task<T> HttpPutRequestWithTokenAsync<T>(string url, object model, string token, Dictionary<string, string> headers = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            AddHeaders(requestMessage, headers);

            return await SendRequestAsync<T>(requestMessage);
        }

        private static async Task<T> SendRequestAsync<T>(HttpRequestMessage requestMessage)
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.SendAsync(requestMessage))
                {
                    //response.EnsureSuccessStatusCode();
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request failed: {ex.Message}");
                throw;
            }
        }

        private static void AddHeaders(HttpRequestMessage requestMessage, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }
        }

        private static void AddBasicAuth(HttpRequestMessage requestMessage, string username, string password)
        {
            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
    }
}