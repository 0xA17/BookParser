using System;
using System.Windows.Input;

namespace BookParser.Core.Command
{
    /// <summary>
    /// Команда ретрансляции.
    /// </summary>
    class RelayCommand : ICommand
    {
        /// <summary>
        /// Выполнение.
        /// </summary>
        private Action<Object> execute;

        /// <summary>
        /// Может выполнить.
        /// </summary>
        private Func<Object, Boolean> canExecute;

        /// <summary>
        /// Событие выполнения изменения.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Конструктор команды ретрансляции.
        /// </summary>
        /// <param name="execute">Выполнение.</param>
        /// <param name="canExecute">Может выполнить</param>
        public RelayCommand(Action<Object> execute, Func<Object, Boolean> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Определяет выполнение.
        /// </summary>
        /// <param name="parameter">Целевой параметр.</param>
        /// <returns>Результат работы параметра.</returns>
        public Boolean CanExecute(Object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        /// <summary>
        /// Присваивает выполнение.
        /// </summary>
        /// <param name="parameter">Целевой параметр.</param>
        public void Execute(Object parameter)
        {
            this.execute(parameter);
        }
    }
}
