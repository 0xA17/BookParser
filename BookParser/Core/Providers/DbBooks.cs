using LinqToDB;
using BookParser.MVVM.Model;

namespace BookParser.Core.Providers
{
    public class DbBooks : LinqToDB.Data.DataConnection
    {
        public DbBooks() : base("Books") { }

        public ITable<BookModel> BookModel => this.GetTable<BookModel>();
    }

}
