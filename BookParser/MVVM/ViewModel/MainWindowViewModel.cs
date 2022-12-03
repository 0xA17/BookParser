using System;
using BookParser.MVVM.Model;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows;

namespace BookParser.MVVM.ViewModel
{
    class MainWindowViewModel : ObservableObject
    {
        #region Переменные

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

            await MainWindowWorkModel.PerformParsingAsync();

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