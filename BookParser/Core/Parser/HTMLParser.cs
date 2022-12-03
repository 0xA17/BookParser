using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookParser.Core.Web;
using System.Linq;
using BookParser.MVVM.Model;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;

namespace BookParser.Core.Parser
{
    public class HTMLParser : HttpTools
    {
        private static BookModel ParseBookModel(HtmlDocument html)
        {
            if (html is null)
            {
                return null;
            }

            HtmlNode[] statinfoNodes = html.DocumentNode?.
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
                Genres = GetGenres(htmlNode: GetTargetNode(statinfoNodes, "Жанр"), attributeName: "a") ?? null,
                Series = GetTargetNode(statinfoNodes, "Серия")?.ChildNodes?.Where(x => x.Name == "a")?.First()?.InnerText ?? String.Empty,
                Description = SelectNodeInnerText(html, "//*[@itemprop='description']") ?? String.Empty,
            };
        }

        private static String SelectNodeInnerText(HtmlDocument htmlDocument, String xPath)
        {
            if (htmlDocument is null ||
                String.IsNullOrEmpty(xPath))
            {
                return null;
            }

            return htmlDocument.DocumentNode?.SelectSingleNode(xPath)?.InnerText ?? null;
        }

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

            String[] genres = new String[tmpNodes.Length];

            for (Byte i = 0; i < genres.Length; i++)
            {
                genres[i] = tmpNodes[i].InnerText;
            }

            return genres;
        }

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

        public static ObservableCollection<BookModel> BuildBookModels(params String[] htmlPages)
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
                    htmlDocument[i].LoadHtml(htmlPages[i]);
                }
                catch
                {
                    continue;
                }
            }

            var bookModels = new ObservableCollection<BookModel>();

            foreach (var html in htmlDocument)
            {
                var tempBookModel = ParseBookModel(html);

                if (tempBookModel is null)
                {
                    continue;
                }

                bookModels.Add(tempBookModel);
            }

            if (bookModels.Count == 0)
            {
                return null;
            }

            return bookModels;
        }
    }
}