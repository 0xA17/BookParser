using System;

namespace BookParser.MVVM.Model
{
    /// <summary>
    /// Книга.
    /// </summary>
    public class BookModel
    {
        /// <summary>
        /// Название.
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Автор.
        /// </summary>
        public String Author { get; set; }

        /// <summary>
        /// Год издания.
        /// </summary>
        public String PublishYear { get; set; }

        /// <summary>
        /// Год написания.
        /// </summary>
        public String WriteYear { get; set; }

        /// <summary>
        /// Издательство.
        /// </summary>
        public String Publisher { get; set; }

        /// <summary>
        /// Isbn.
        /// </summary>
        public String Isbn { get; set; }

        /// <summary>
        /// Жанр.
        /// </summary>
        public String[] Genres { get; set; }

        /// <summary>
        /// Серия.
        /// </summary>
        public String Series { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        public String Description { get; set; }
    }
}