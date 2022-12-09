using System;
using System.Windows;
using System.Windows.Input;
using BookParser.MVVM.ViewModel;

namespace BookParser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Ссылка на класс главного окна.
        /// </summary>
        public static MainWindow Instance;

        /// <summary>
        /// Конструктор главного окна.
        /// </summary>
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        /// <summary>
        /// Возвращает ссылку на класс главного окна.
        /// </summary>
        /// <returns>Ссылка на класс главного окна.</returns>
        public static MainWindow GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// Отслеживает позицию зажатой мыши.
        /// </summary>
        /// <param name="sender">Вызываемый объект.</param>
        /// <param name="e">Аргументы вызываемого объекта.</param>
        private void Window_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}