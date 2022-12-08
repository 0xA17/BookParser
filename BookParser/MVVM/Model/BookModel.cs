using System;
using LinqToDB.Mapping;

namespace BookParser.MVVM.Model
{
    /// <summary>
    /// Книга.
    /// </summary>
    [Table(Name = "public.books")]
    public class BookModel
    {
        /// <summary>
        /// Название.
        /// </summary>
        [Column(Name = "title")]
        public String Title { get; set; }

        /// <summary>
        /// Автор.
        /// </summary>
        [Column(Name = "author")]
        public String Author { get; set; }

        /// <summary>
        /// Год издания.
        /// </summary>
        [Column(Name = "publishyear")]
        public String PublishYear { get; set; }

        /// <summary>
        /// Год написания.
        /// </summary>
        [Column(Name = "writeyear")]
        public String WriteYear { get; set; }

        /// <summary>
        /// Издательство.
        /// </summary>
        [Column(Name = "publisher")]
        public String Publisher { get; set; }

        /// <summary>
        /// Isbn.
        /// </summary>
        [Column(Name = "isbn")]
        public String Isbn { get; set; }

        /// <summary>
        /// Жанр.
        /// </summary>
        [Column(Name = "genres")]
        public String Genres { get; set; }

        /// <summary>
        /// Серия.
        /// </summary>
        [Column(Name = "series")]
        public String Series { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        [Column(Name = "description")]
        public String Description { get; set; }
    }
}