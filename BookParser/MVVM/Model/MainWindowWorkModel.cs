using System;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using BookParser.Core.Parser;

namespace BookParser.MVVM.Model
{
    public class MainWindowWorkModel : HTMLBookParser
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
                                         "https://litmore.ru/17833-neksus.html",
                                         "https://litmore.ru/tegi/%D1%81%D0%BE%D1%86%D0%B8%D0%B0%D0%BB%D1%8C%D0%BD%D0%B0%D1%8F+%D0%BF%D1%80%D0%BE%D0%B7%D0%B0/");
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

            var tempAnswer = await BooksParse(urls);
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

