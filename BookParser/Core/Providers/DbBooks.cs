using LinqToDB;
using BookParser.MVVM.Model;

namespace BookParser.Core.Providers
{
    /// <summary>
    /// Подключение к целевой БД.
    /// </summary>
    public class DbBooks : LinqToDB.Data.DataConnection
    {
        /// <summary>
        /// Базовый конструктор подключения.
        /// </summary>
        public DbBooks() : base("Books") { }

        /// <summary>
        /// Структура таблицы.
        /// </summary>
        public ITable<BookModel> BookModel => this.GetTable<BookModel>();
    }

}