using System;
using LinqToDB;
using BookParser.MVVM.Model;

namespace BookParser.Core.Providers
{
    public class PostgreSQLProvider
    {
        #region Методы

        public static Boolean InsertBookModel(params BookModel[] bookModels)
        {
            if (bookModels is null ||
                bookModels.Length == 0)
            {
                return false;
            }

            using (var db = new DbBooks())
            {
                try
                {
                    foreach (var bookModel in bookModels)
                    {
                        db.BookModel
                          .Value(p => p.Title, bookModel.Title)
                          .Value(p => p.Author, bookModel.Author)
                          .Value(p => p.PublishYear, bookModel.PublishYear)
                          .Value(p => p.WriteYear, bookModel.WriteYear)
                          .Value(p => p.Publisher, bookModel.Publisher)
                          .Value(p => p.Isbn, bookModel.Isbn)
                          .Value(p => p.Genres, bookModel.Genres)
                          .Value(p => p.Series, bookModel.Series)
                          .Value(p => p.Description, bookModel.Description)
                          .Insert();
                    }
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
