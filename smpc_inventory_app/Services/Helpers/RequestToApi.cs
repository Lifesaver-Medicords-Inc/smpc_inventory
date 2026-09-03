using Newtonsoft.Json;
using smpc_invemtory_app.Pages.Shared;
using smpc_inventory_app.Data;
using smpc_inventory_app.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace smpc_inventory_app.Services.Helpers
{
    internal class RequestToApi<T> where T : class
    {
        static string baseUrl
        {
            get
            {
                string env =
                    ConfigurationManager.AppSettings["Environment"]
                    ?? "Development";

                return ConfigurationManager.AppSettings[$"ApiBaseUrl.{env}"]
                    ?? "http://127.0.0.1:3000/api";
            }
        }
        static CookieContainer cookieContainer = new CookieContainer();

        static private async Task<T> SendRequestAsync(string url, HttpMethod method, string body = null)
        {
            // Create an HttpClientHandler and assign the CookieContainer to it
            HttpClientHandler handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            };
            using (HttpClient client = new HttpClient(handler))
            {
                try
                {
                    //LoaderIndicatorOverlay.ShowOverlay();
                    HttpContent content = null;
                    // If no content is provided, create an empty StringContent with Content-Type set to "application/json"
                    if (content == null && method != HttpMethod.Get)
                    {
                        content = new StringContent(body, Encoding.UTF8, "application/json");
                    }
                    // Create the HttpRequestMessage with the specified method (GET, POST, PUT, DELETE)
                    var requestMessage = new HttpRequestMessage(method, baseUrl + url)
                    {
                        Content = content
                    };
                    if (CacheData.SessionToken != "")
                    {
                        client.DefaultRequestHeaders.Add("Authorization", CacheData.SessionToken);
                    }
                    // Perform the HTTP request asynchronously
                    HttpResponseMessage response = await client.SendAsync(requestMessage);

                    string allHeaders = string.Join("\n", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"));

                    //testing if the token is being sent back in the response headers
                    //MessageBox.Show("Status: " + response.StatusCode + "\n\nHeaders:\n" + allHeaders, "Response Debug");


                    // Check if the response is successful
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();



                        // Only the login endpoint issues a Set-Cookie; every other call
                        // succeeds without one. HttpResponseHeaders.GetValues THROWS
                        // ("The given header was not found") rather than returning null when
                        // the header is absent, so any successful non-login response arriving
                        // while the token was still empty - anything the login screen itself
                        // fetches before sign-in - surfaced as a bare "Exception: The given
                        // header was not found" dialog over the login window. TryGetValues is
                        // the non-throwing form: take the token when it is actually there,
                        // otherwise carry on. (Dispatching and Admin never hit this because
                        // they already guard with Headers.Contains first; Sales, Accounting
                        // and Engineering use this same TryGetValues form.)
                        if (string.IsNullOrEmpty(CacheData.SessionToken)
                            && response.Headers.TryGetValues("Set-Cookie", out var setCookieValues))
                        {
                            List<String> tokenResponseArr = setCookieValues.ToList();
                            if (tokenResponseArr.Count > 0)
                            {
                                string token = ExtractToken(tokenResponseArr[0]);
                                if (!string.IsNullOrEmpty(token))
                                {
                                    CacheData.SessionToken = token;
                                }
                            }
                        }
                        // Optionally, you can parse the responseContent into an object of type T
                        T result = JsonConvert.DeserializeObject<T>(responseContent);
                        // Display the response content (for debugging purposes)
                        //MessageBox.Show(responseContent, "API Response");
                        return result; // Return the parsed result
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        // Optionally, you can parse the responseContent into an object of type T
                        T result = JsonConvert.DeserializeObject<T>(responseContent);
                        // Display the response content (for debugging purposes)
                        //MessageBox.Show(responseContent, "API Response");

                        return result; // Return the
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception: " + ex.Message, "Error ");
                    return default(T);  // Return default value of T in case of exception
                }
                finally
                {
                    //LoaderIndicatorOverlay.HideOverlay();
                }
            }
        }
        //// POST Method
        static async Task<T> Post(string url, HttpContent data)
        {
            string jsonContent = JsonConvert.SerializeObject(data);
            return await SendRequestAsync(url, HttpMethod.Post, jsonContent);
        }
        static internal async Task<T> Post(string url, Dictionary<string, dynamic> data)
        {
            string jsonContent = JsonConvert.SerializeObject(data);
            return await SendRequestAsync(url, HttpMethod.Post, jsonContent);
        }
        // PUT Method
        static internal async Task<T> Put(string url, HttpContent data)
        {
            string jsonContent = JsonConvert.SerializeObject(data);
            return await SendRequestAsync(url, HttpMethod.Put, jsonContent);
        }
        static internal async Task<T> Put(string url, Dictionary<string, object> data)
        {
            string jsonContent = JsonConvert.SerializeObject(data);
            return await SendRequestAsync(url, HttpMethod.Put, jsonContent);
        }
        static internal async Task<T> Put(string url, List<Dictionary<string, object>> data)
        {
            string jsonContent = JsonConvert.SerializeObject(data);
            return await SendRequestAsync(url, HttpMethod.Put, jsonContent);
        }
        // GET Method
        public static async Task<T> Get(string url)
        {
            return await SendRequestAsync(url, HttpMethod.Get);
        }
        //DELETE Method
        static internal async Task<T> Delete(string url, HttpContent data)
        {
            string jsonContent = JsonConvert.SerializeObject(data);
            return await SendRequestAsync(url, HttpMethod.Delete, jsonContent);
        }
        static internal async Task<T> Delete(string url, Dictionary<string, object> data)
        {
            string jsonContent = JsonConvert.SerializeObject(data);
            return await SendRequestAsync(url, HttpMethod.Delete, jsonContent);
        }
        // Returns null when the cookie carries no Authorization value, rather than throwing.
        // The original computed Substring BEFORE testing tokenEndIndex for -1, so a
        // Set-Cookie without a trailing semicolon threw ArgumentOutOfRangeException - and a
        // cookie with no "Authorization=" at all made IndexOf return -1, putting the start
        // index at 13 and slicing from the middle of whatever was there. Both surfaced as
        // an unexplained exception dialog on the login screen, same as the missing header.
        private static string ExtractToken(string cookieString)
        {
            if (string.IsNullOrEmpty(cookieString)) return null;

            const string marker = "Authorization=";
            int markerIndex = cookieString.IndexOf(marker);
            if (markerIndex < 0) return null;

            int tokenStartIndex = markerIndex + marker.Length;
            int tokenEndIndex = cookieString.IndexOf(";", tokenStartIndex);

            // No semicolon means there is no expiry info after it - take the rest.
            return tokenEndIndex < 0
                ? cookieString.Substring(tokenStartIndex)
                : cookieString.Substring(tokenStartIndex, tokenEndIndex - tokenStartIndex);
        }
    }












}
