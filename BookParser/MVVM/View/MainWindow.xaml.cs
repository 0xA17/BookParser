using System;
using System.Windows;
using System.Windows.Input;
using BookParser.MVVM.ViewModel;

namespace BookParser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        public static MainWindow GetInstance()
        {
            return Instance;
        }

        private void Window_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}