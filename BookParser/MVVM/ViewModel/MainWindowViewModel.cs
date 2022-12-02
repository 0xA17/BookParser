using System;
using BookParser.MVVM.Model;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace BookParser.MVVM.ViewModel
{
    class MainWindowViewModel : ObservableObject
    {
        #region Переменные

        private Boolean _parseIsRunning;
        public Boolean ParseIsRunning
        {
            get => _parseIsRunning;
            set => SetProperty(ref _parseIsRunning, value);
        }

        private Int16 _parseProgressValue;
        public Int16 ParseProgressValue
        {
            get => _parseProgressValue;
            set => SetProperty(ref _parseProgressValue, value);
        }

        public IAsyncRelayCommand ExecuteParsing { get; }
        public IRelayCommand CancelParsingCommand { get; }

        public Core.Command.RelayCommand CloseWindow { get; set; }
        public Core.Command.RelayCommand MinimizeWindow { get; set; }

        #endregion

        #region Конструктор

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

        private async Task InitParseExecuteAsync()
        {
            ParseIsRunning = true;
            UpdateCommandStatus();

            var progress = new Progress<ProgressInfo>();

            progress.ProgressChanged += (sender, e) =>
            {
                ParseProgressValue = e.ParseProgressValue;
            };

            await MainWindowWorkModel.PerformParsingAsync(progress);

            ParseIsRunning = false;
            UpdateCommandStatus();
        }

        private Boolean ChangeParseExecute()
        {
            return !ParseIsRunning;
        }

        private void UpdateCommandStatus()
        {
            ExecuteParsing.NotifyCanExecuteChanged();
            CancelParsingCommand.NotifyCanExecuteChanged();
        }

        #endregion
    }
}