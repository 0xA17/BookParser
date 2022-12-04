using HtmlAgilityPack;
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

        protected static async Task<String[]> GetHtmlPagesFromUrls(params String[] urls)
        {
            if (urls is null ||
                urls.Length == 0)
            {
                return null;
            }

            String[] htmlPages = new String[urls.Length];

            for (Int32 i = 0; i < urls.Length; i++)
            {
                htmlPages[i] = await GetHtmlFromUrl(urls[i]);
                await Task.Delay(1000);
            }

            return htmlPages;
        }

        public static HtmlDocument[] GetLoadedHtmlDoc(string[] htmlPages)
        {
            var htmlDocument = new HtmlDocument[htmlPages.Length];

            for (Int32 i = 0; i < htmlPages.Length; i++)
            {
                htmlDocument[i] = new HtmlDocument();

                try
                {
                    htmlDocument[i].LoadHtml(htmlPages[i]);
                }
                catch
                {
                    continue;
                }
            }

            return htmlDocument;
        }
    }
}