using GutenbergApp.Helpers;
using GutenbergApp.Json;
using GutenbergApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GutenbergApp.Services
{
    public class CommunicationService
    {
        private static CommunicationService communicationService;
        private static object lockObj = new object();

        private CommunicationService()
        {
        }

        public static CommunicationService Instance
        {
            get
            {
                if(communicationService == null)
                {
                    lock(lockObj)
                    {
                        if(communicationService == null)
                        {
                            communicationService = new CommunicationService();
                        }
                    }
                }

                return communicationService;
            }
        }


        public async Task<Books> GetBooksAsync(string apiURI)
        {
            try
            {
                Task<BooksJson> booksJsonTask = RequestService.Instance.GetAsync<BooksJson>(apiURI);

                await booksJsonTask;

                if (booksJsonTask.Result != null)
                {
                    return JsonCSConversion.PraseJson(booksJsonTask.Result);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message} Stacktrace: {ex.StackTrace}");
            }
            return null;
        }

        public async Task<string> GetBookCoverImage(string uri)
        {
            try
            {
                Task<string> response = RequestService.Instance.GetBookCoverImage(uri);

                await response;

                System.Diagnostics.Debug.WriteLine($"Uri: {uri} Is Result empty: {response.Result} ");

                if (!string.IsNullOrWhiteSpace(response.Result))
                {
                    return response.Result;
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Message: {ex.Message} Stacktrace: {ex.StackTrace}");
            }

            return null;
        }
    }
}
