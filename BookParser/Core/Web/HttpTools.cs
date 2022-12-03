using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookParser.Core.Web
{
    public class HttpTools
    {
        public static Task<String> GetHtmlFromUrl(String url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return null;
            }

            var httpClient = new HttpClient();
            Task<String> answerFromGet;

            try
            {
                answerFromGet = httpClient.GetStringAsync(url);
            }
            catch (HttpRequestException)
            {
                return null;
            }

            return answerFromGet;
        }
    }
}