using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GutenbergApp.Services
{
    public class RequestService
    {
        private static RequestService requestService;
        private static object lockObj = new object();
        private readonly HttpClient httpClient; 

        private RequestService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
        public static RequestService Instance
        {
            get
            {
                if(requestService == null)
                {
                    lock(lockObj)
                    {
                        if(requestService == null)
                        {
                            requestService = new RequestService();
                        }
                    }
                }

                return requestService;
            }
        }

        public async Task<TRes> GetAsync<TRes>(string uri)
        {
            HttpResponseMessage responseMessage = await httpClient.GetAsync(uri).ConfigureAwait(false);

            if(responseMessage.IsSuccessStatusCode)
            {
                string response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                TRes responseObj = JsonSerializer.Deserialize<TRes>(response);

                return responseObj;
            }

            return default(TRes);
        }

        public async Task<string> GetBookCoverImage(string uri)
        {
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync(uri);

                if (responseMessage.IsSuccessStatusCode)
                {
                    Task<byte[]> taskImage = responseMessage.Content.ReadAsByteArrayAsync();

                    await taskImage;

                    System.Diagnostics.Debug.WriteLine($"Uri: {uri} Is Result null: {taskImage.Result != null} ");

                    if (taskImage.Result != null)
                    {
                        return Convert.ToBase64String(taskImage.Result);
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return null;
        }

    }
}
