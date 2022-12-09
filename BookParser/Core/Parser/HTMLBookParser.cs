using System;
using System.Linq;
using HtmlAgilityPack;
using BookParser.Core.Web;
using BookParser.MVVM.Model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BookParser.Core.Parser
{
    /// <summary>
    /// Парсер книг по HTML.
    /// </summary>
    public class HTMLBookParser : HttpTools
    {
        /// <summary>
        /// Парсинг данных о книге с целевого ресурса.
        /// </summary>
        /// <param name="html">HTML-код ресурса.</param>
        /// <returns>Модель книги.</returns>
        private static BookModel ParseBookModel(HtmlDocument html)
        {
            if (html is null)
            {
                return null;
            }

            var statinfoNodes = html.DocumentNode?.
                                   SelectSingleNode("//div[@class='statinfo']")?.
                                   ChildNodes?.
                                   Where(x => x.Name == "ul")?.
                                   First()?.Elements("li")?.
                                   ToArray() ?? null;

            if (statinfoNodes.Length == 0 ||
                statinfoNodes is null)
            {
                return null;
            }

            return new BookModel()
            {
                Title = SelectNodeInnerText(html, "//*[@itemprop='name']") ?? String.Empty,
                Author = SelectNodeInnerText(html, "//*[@itemprop='author']") ?? String.Empty,
                PublishYear = GetTargetNode(statinfoNodes, "Год издания")?.ChildNodes?.ToArray()?.Last()?.InnerText?.Replace(" ", String.Empty) ?? String.Empty,
                WriteYear = GetTargetNode(statinfoNodes, "Год написания")?.ChildNodes?.ToArray()?.Last()?.InnerText?.Replace(" ", String.Empty) ?? String.Empty,
                Publisher = SelectNodeInnerText(html, "//*[@itemprop='publisher']") ?? String.Empty,
                Isbn = SelectNodeInnerText(html, "//*[@itemprop='isbn']") ?? String.Empty,
                Genres = GetSequenceGenres(GetGenres(htmlNode: GetTargetNode(statinfoNodes, "Жанр"), attributeName: "a")) ?? null,
                Series = GetTargetNode(statinfoNodes, "Серия")?.ChildNodes?.Where(x => x.Name == "a")?.First()?.InnerText ?? String.Empty,
                Description = SelectNodeInnerText(html, "//*[@itemprop='description']") ?? String.Empty,
            };
        }

        /// <summary>
        /// Формирует массив жанров в строку.
        /// </summary>
        /// <param name="genres">Массив жанров.</param>
        /// <returns>Строка жанров.</returns>
        private static String GetSequenceGenres(String[] genres)
        {
            if (genres is null ||
                genres.Length == 0)
            {
                return null;
            }

            var sequenceGenres = String.Empty;
            var lastGenre = genres.Last();

            foreach (var genre in genres)
            {
                if (genre == lastGenre)
                {
                    sequenceGenres += $"{genre}.";
                    continue;
                }

                sequenceGenres += $"{genre}, ";
            }

            return sequenceGenres;
        }

        /// <summary>
        /// Получает внутренний текст заданного объекта с HTML-страницы.
        /// </summary>
        /// <param name="htmlDocument">HTML-страница</param>
        /// <param name="xPath">Целевой объект.</param>
        /// <returns>Внутренний тест объекта.</returns>
        private static String SelectNodeInnerText(HtmlDocument htmlDocument, String xPath)
        {
            if (htmlDocument is null ||
                String.IsNullOrEmpty(xPath))
            {
                return null;
            }

            return htmlDocument.DocumentNode?.SelectSingleNode(xPath)?.InnerText ?? null;
        }

        /// <summary>
        /// Получает массив жанров по имени атрибута.
        /// </summary>
        /// <param name="htmlNode">HTML-ресурс.</param>
        /// <param name="attributeName">Имя атрибута.</param>
        /// <returns>Массив жанров.</returns>
        private static String[] GetGenres(HtmlNode htmlNode, String attributeName)
        {
            if (String.IsNullOrEmpty(attributeName) ||
                htmlNode is null)
            {
                return null;
            }

            var tmpNodes = htmlNode.ChildNodes?.Where(x => x.Name == attributeName)?.ToArray() ?? null;

            if (tmpNodes is null ||
                tmpNodes.Length == 0)
            {
                return null;
            }

            var genres = new String[tmpNodes.Length];

            for (Byte i = 0; i < genres.Length; i++)
            {
                genres[i] = tmpNodes[i].InnerText;
            }

            return genres;
        }

        /// <summary>
        /// Возвращает целевой HTML-объект по заданному имени.
        /// </summary>
        /// <param name="htmlNodes">HTML-объекты.</param>
        /// <param name="targetName">Целевое имя объекта.</param>
        /// <returns>HTML-объект.</returns>
        private static HtmlNode GetTargetNode(HtmlNode[] htmlNodes, String targetName)
        {
            if (String.IsNullOrEmpty(targetName) ||
                htmlNodes is null)
            {
                return null;
            }

            targetName = targetName.ToLower();

            foreach (var node in htmlNodes)
            {
                if (node.InnerText.ToLower().Contains(targetName))
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Проверяет массив ресурсов на наличие подборок книг. 
        /// Если определяет страницу подборок, то добавляет все книги с данного ресурса.
        /// </summary>
        /// <param name="htmlDocumentPages">HTML-страницы.</param>
        /// <param name="targetXPath">Критерий проверки на подборку.</param>
        /// <returns>Сформированная коллекция книг.</returns>
        private static async Task<ObservableCollection<HtmlDocument>> SelectionBooksCheck(
            HtmlDocument[] htmlDocumentPages,
            String targetXPath)
        {
            if (htmlDocumentPages is null ||
                String.IsNullOrEmpty(targetXPath))
            {
                return null;
            }

            var bookPages = new ObservableCollection<HtmlDocument>();
            var selectionPages = new ObservableCollection<HtmlDocument>();

            foreach (var page in htmlDocumentPages)
            {
                if (SelectNodeInnerText(page, targetXPath) is not null)
                {
                    bookPages.Add(page);
                    continue;
                }

                selectionPages.Add(page);
            }

            if (selectionPages.Count == 0)
            {
                return bookPages;
            }

            if (!ParseBooksUrls(in selectionPages, out ObservableCollection<String[]> bookUrls))
            {
                /*FAIL*/
            }

            if (!await ParseHtmlDocuments(
                 in bookUrls,
                 out ObservableCollection<HtmlDocument[]> bookHtmlDocuments))
            {
                /*FAIL*/
            }

            if (!ParseBookPages(
                 ref bookPages,
                 in bookHtmlDocuments))
            {
                /*FAIL*/
            }

            return bookPages;
        }

        /// <summary>
        /// Формирует страницы кинг с коллекции HTML-ресурсов.
        /// </summary>
        /// <param name="bookPages">Коллеция книг.</param>
        /// <param name="bookHtmlDocuments">Коллекция HTML-ресурсов.</param>
        /// <returns>В случае успешного добавления - True, иначе - False.</returns>
        private static Boolean ParseBookPages(
            ref ObservableCollection<HtmlDocument> bookPages,
            in ObservableCollection<HtmlDocument[]> bookHtmlDocuments)
        {
            if (bookPages is null ||
                bookHtmlDocuments is null)
            {
                return false;
            }

            foreach (var bookHtmlDocument in bookHtmlDocuments)
            {
                if (bookHtmlDocument is null)
                {
                    continue;
                }

                foreach (var bookPage in bookHtmlDocument)
                {
                    if (bookPage is not null)
                    {
                        bookPages.Add(bookPage);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Формирует ссылки на книги с заданной страницы.
        /// </summary>
        /// <param name="selectionPages">Заданная страница.</param>
        /// <param name="bookUrls">Ссылки на книги.</param>
        /// <returns>True, если параметр ссылки успешно добавлены; в противном случае — false.</returns>
        private static Boolean ParseBooksUrls(
            in ObservableCollection<HtmlDocument> selectionPages,
            out ObservableCollection<String[]> bookUrls)
        {
            if (selectionPages is null ||
               selectionPages.Count == 0)
            {
                bookUrls = null;
                return false;
            }

            bookUrls = new ObservableCollection<String[]>();

            foreach (var page in selectionPages)
            {
                var tempBookUrls = GetBooksUrls(page);

                if (tempBookUrls is not null)
                {
                    bookUrls.Add(tempBookUrls);
                }
            }

            if (bookUrls.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Получает массив ссылок на книги с заданного ресурса.
        /// </summary>
        /// <param name="htmlDocument">Целевой ресурс.</param>
        /// <returns>Массив ссылок.</returns>
        private static String[] GetBooksUrls(HtmlDocument htmlDocument)
        {
            if (htmlDocument is null)
            {
                return null;
            }

            return htmlDocument.DocumentNode?.
                   SelectNodes("//a[@href]")?.
                   Where(x => x.Attributes.Last().Name == "title" && x.ParentNode.ChildNodes.Count == 1)?.
                   Select(x => x.Attributes.First().Value)?.ToArray() ?? null;
        }

        /// <summary>
        /// Парсит книги с ресурсов, формируя модели книг.
        /// </summary>
        /// <param name="urls">Ссылки на ресурсы.</param>
        /// <returns>Коллеция моделей книг.</returns>
        protected static async Task<ObservableCollection<BookModel>> BooksParse(params String[] urls)
        {
            if (urls is null ||
                urls.Length == 0)
            {
                return null;
            }

            return await BuildBookModels(htmlPages: await GetHtmlPagesFromUrls(urls));
        }

        /// <summary>
        /// Формирует модели книг.
        /// </summary>
        /// <param name="htmlPages">Исходные HTML-страницы книг.</param>
        /// <returns></returns>
        private static async Task<ObservableCollection<BookModel>> BuildBookModels(params String[] htmlPages)
        {
            if (htmlPages is null ||
                htmlPages.Length == 0)
            {
                return null;
            }

            var bookModels = new ObservableCollection<BookModel>();

            foreach (var html in await SelectionBooksCheck(
                                       htmlDocumentPages: GetLoadedHtmlDoc(htmlPages),
                                       targetXPath: "//div[@class='tegtitle']"))
            {
                var tempBookModel = ParseBookModel(html);

                if (tempBookModel is not null)
                {
                    bookModels.Add(tempBookModel);
                }
            }

            if (bookModels.Count == 0)
            {
                return null;
            }

            return bookModels;
        }
    }
}