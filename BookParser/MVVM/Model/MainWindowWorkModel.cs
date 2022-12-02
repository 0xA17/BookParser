using System;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;

namespace BookParser.MVVM.Model
{
    public class MainWindowWorkModel
    {
        #region Методы

        public static void InitCloseWindow() => Application.Current.Shutdown();

        public static void InitMinimizeWindow() => Application.Current.MainWindow.WindowState = WindowState.Minimized;

        private static CancellationTokenSource _parseCancelTokenSrc;

        public static async Task PerformParsingAsync(IProgress<ProgressInfo> progress)
        {
            using (_parseCancelTokenSrc = new CancellationTokenSource())
            {
                try
                {
                    for (Int16 i = 0; i < 100; i++)
                    {
                        _parseCancelTokenSrc.Token.ThrowIfCancellationRequested();

                        await Task.Delay(100);

                        progress.Report(new ProgressInfo(i));
                    }

                    progress.Report(new ProgressInfo(0));
                }
                catch (OperationCanceledException)
                {
                    progress.Report(new ProgressInfo(0));
                    return;
                }

            }
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