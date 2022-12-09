using System;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BookParser.Core.Web
{
    /// <summary>
    /// Предоставляет набор HTTP-инструментов.
    /// </summary>
    public class HttpTools
    {
        /// <summary>
        /// Получает HTML-код заданного ресурса.
        /// </summary>
        /// <param name="url">Адрес ресурса.</param>
        /// <returns>HTML-код ресурса.</returns>
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

        /// <summary>
        /// Формирует массив HTML-кодов ресурсов.
        /// </summary>
        /// <param name="urls">Адреса ресурсов.</param>
        /// <returns>Массив исходных HTML-кодов ресурсов.</returns>
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

        /// <summary>
        /// Формирует HTML-код ресурсов в тип HtmlDocument.
        /// </summary>
        /// <param name="htmlPages">HTML-код ресурса.</param>
        /// <returns>HTML-код ресурсов в типе HtmlDocument.</returns>
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

        /// <summary>
        /// Формирует коллекцию типа HtmlDocument по ссылкам на ресурсы.
        /// </summary>
        /// <param name="urls">Адреса ресурсов.</param>
        /// <param name="htmlDocuments">Коллекция типа HtmlDocument.</param>
        /// <returns>В случае успешного формирования - True, иначе - False.</returns>
        public static Task<Boolean> ParseHtmlDocuments(
            in ObservableCollection<String[]> urls,
            out ObservableCollection<HtmlDocument[]> htmlDocuments)
        {
            htmlDocuments = new ObservableCollection<HtmlDocument[]>();

            return ParseHtmlDocumentsInternal(urls, htmlDocuments);
        }

        /// <summary>
        /// Формирует коллекцию типа HtmlDocument по ссылкам на ресурсы.
        /// </summary>
        /// <param name="urls">Адреса ресурсов.</param>
        /// <param name="htmlDocuments">Коллекция типа HtmlDocument.</param>
        /// <returns>В случае успешного формирования - True, иначе - False.</returns>
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