using System;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using BookParser.Core.Parser;

namespace BookParser.MVVM.Model
{
    public class MainWindowWorkModel : HTMLParser
    {
        #region Методы

        public static void InitCloseWindow() => Application.Current.Shutdown();

        public static void InitMinimizeWindow() => Application.Current.MainWindow.WindowState = WindowState.Minimized;

        private static CancellationTokenSource _parseCancelTokenSrc;

        public static async Task PerformParsingAsync()
        {
            using (_parseCancelTokenSrc = new CancellationTokenSource())
            {
                try
                {
                    _parseCancelTokenSrc.Token.ThrowIfCancellationRequested();
                    await InitBooksParse("https://litmore.ru/17611-antropolog-na-marse.html", 
                                         "https://litmore.ru/17833-neksus.html");
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        private static async Task InitBooksParse(params String[] urls)
        {
            if (urls is null ||
                urls.Length == 0)
            {
                return;
            }

            String[] htmlPages = new String[urls.Length];

            for (Int32 i = 0; i < urls.Length; i++)
            {
                htmlPages[i] = await GetHtmlFromUrl(urls[i]);
                await Task.Delay(1000);
            }

            BuildBookModels(htmlPages);
        }

        public static void CancelParse()
        {
            if (_parseCancelTokenSrc?.IsCancellationRequested == false)
            {
                _parseCancelTokenSrc.Cancel();
            }
        }

        #endregion
    }
}