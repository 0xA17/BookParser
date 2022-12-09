using System;
using System.Windows;
using BookParser.MVVM.Model;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BookParser.MVVM.ViewModel
{
    /// <summary>
    /// View-модель главного окна.
    /// </summary>
    class MainWindowViewModel : ObservableObject
    {
        #region Переменные

        /// <summary>
        /// Статус парсинга.
        /// </summary>
        private Boolean _parseIsRunning;
        public Boolean ParseIsRunning
        {
            get
            {
                return _parseIsRunning;
            }
            set 
            {
                SetProperty(ref _parseIsRunning, value);
                MainWindow.GetInstance().IsParseLoad.Visibility = _parseIsRunning 
                                                                ? Visibility.Visible 
                                                                : Visibility.Hidden;
            }
        }

        /// <summary>
        /// Команда выполнения парсинга.
        /// </summary>
        public IAsyncRelayCommand ExecuteParsing { get; }

        /// <summary>
        /// Команда заверщения парсинга.
        /// </summary>
        public IRelayCommand CancelParsingCommand { get; }

        /// <summary>
        /// Команда завершения работы приложения.
        /// </summary>
        public Core.Command.RelayCommand CloseWindow { get; set; }

        /// <summary>
        /// Команда сворачивания окна.
        /// </summary>
        public Core.Command.RelayCommand MinimizeWindow { get; set; }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор View-модели.
        /// </summary>
        public MainWindowViewModel()
        {
            CloseWindow = new Core.Command.RelayCommand(o =>
            {
                MainWindowWorkModel.InitCloseWindow();
            });

            MinimizeWindow = new Core.Command.RelayCommand(o =>
            {
                MainWindowWorkModel.InitMinimizeWindow();
            });

            ParseIsRunning = false;

            ExecuteParsing = new AsyncRelayCommand(InitParseExecuteAsync, ChangeParseExecute);

            CancelParsingCommand = new RelayCommand(
                () =>
                {
                    MainWindowWorkModel.CancelParse();
                    UpdateCommandStatus();
                },
                () => ParseIsRunning);
        }

        #endregion

        #region Методы

        /// <summary>
        /// Инициализирует процесс парсинга книг.
        /// </summary>
        /// <returns></returns>
        private async Task InitParseExecuteAsync()
        {
            ParseIsRunning = true;
            UpdateCommandStatus();

            await MainWindowWorkModel.PerformParsingAsync();

            ParseIsRunning = false;
            UpdateCommandStatus();
        }

        /// <summary>
        /// Изменяет статус парсинга.
        /// </summary>
        /// <returns></returns>
        private Boolean ChangeParseExecute()
        {
            return !ParseIsRunning;
        }

        /// <summary>
        /// Обновляет статус комманды.
        /// </summary>
        private void UpdateCommandStatus()
        {
            ExecuteParsing.NotifyCanExecuteChanged();
            CancelParsingCommand.NotifyCanExecuteChanged();
        }

        #endregion
    }
}