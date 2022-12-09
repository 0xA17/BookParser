using System;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using BookParser.Core.Parser;
using BookParser.Core.Providers;

namespace BookParser.MVVM.Model
{
    /// <summary>
    /// Модель работы главного окна.
    /// </summary>
    public class MainWindowWorkModel : HTMLBookParser
    {
        #region Методы

        /// <summary>
        /// Завершает работу программы.
        /// </summary>
        public static void InitCloseWindow() => Application.Current.Shutdown();

        /// <summary>
        /// Сворачивает окно программы.
        /// </summary>
        public static void InitMinimizeWindow() => Application.Current.MainWindow.WindowState = WindowState.Minimized;

        /// <summary>
        /// Токен отмены выполнения парсинга.
        /// </summary>
        private static CancellationTokenSource _parseCancelTokenSrc;

        /// <summary>
        /// Массив ссылок(Для примера).
        /// </summary>
        private static readonly String[] BooksUrlArray = 
            new String[] { "https://litmore.ru/17611-antropolog-na-marse.html",
                           "https://litmore.ru/17833-neksus.html",
                           "https://litmore.ru/tegi/%D1%82%D0%B0%D0%B9%D0%BD%D1%8B+%D0%B8+%D0%B7%D0%B0%D0%B3%D0%B0%D0%B4%D0%BA%D0%B8+%D0%B1%D1%8B%D1%82%D0%B8%D1%8F/"};

        /// <summary>
        /// Выполняет запуск процесса парсинга книг.
        /// </summary>
        public static async Task PerformParsingAsync()
        {
            using (_parseCancelTokenSrc = new CancellationTokenSource())
            {
                try
                {
                    _parseCancelTokenSrc.Token.ThrowIfCancellationRequested();
                    await InitBooksParse(BooksUrlArray);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Инициализирует парсинг книг.
        /// </summary>
        /// <param name="urls">Ссылки на ресурсы книг.</param>
        private static async Task InitBooksParse(params String[] urls)
        {
            if (urls is null ||
                urls.Length == 0)
            {
                return;
            }

            PostgreSQLProvider.InsertBookModel((await BooksParse(urls)).ToArray());
        }

        /// <summary>
        /// Отмена парснга книг.
        /// </summary>
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