using System;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

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

            var htmlPages = new String[urls.Length];

            for (Int32 i = 0; i < urls.Length; i++)
            {
                htmlPages[i] = await GetHtmlFromUrl(urls[i]);
                await Task.Delay(4096);
            }

            return htmlPages;
        }

        public static HtmlDocument[] GetLoadedHtmlDoc(String[] htmlPages)
        {
            if (htmlPages is null ||
                htmlPages.Length == 0)
            {
                return null;
            }

            var htmlDocument = new HtmlDocument[htmlPages.Length];

            for (Int32 i = 0; i < htmlPages.Length; i++)
            {
                htmlDocument[i] = new HtmlDocument();

                try
                {
                    if (htmlPages[i] is not null)
                    {
                        htmlDocument[i].LoadHtml(htmlPages[i]);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return htmlDocument;
        }

        public static Task<Boolean> ParseHtmlDocuments(
            in ObservableCollection<String[]> urls,
            out ObservableCollection<HtmlDocument[]> htmlDocuments)
        {
            htmlDocuments = new ObservableCollection<HtmlDocument[]>();

            return ParseHtmlDocumentsInternal(urls, htmlDocuments);
        }

        private static async Task<Boolean> ParseHtmlDocumentsInternal(
            ObservableCollection<String[]> urls,
            ObservableCollection<HtmlDocument[]> htmlDocuments)
        {
            if (urls is null ||
                urls.Count == 0)
            {
                return false;
            }

            foreach (var page in urls)
            {
                htmlDocuments.Add(GetLoadedHtmlDoc(htmlPages: await GetHtmlPagesFromUrls(page)));
            }

            if (htmlDocuments.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}