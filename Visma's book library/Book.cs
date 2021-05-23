using System;

namespace Vismas_book_library
{
    /// <summary>
    /// Class for books that are available
    /// </summary>
    public class Book
    {
        public string Name { get; }
        public string Author { get; }
        public string Category { get; }
        public string Language { get; }
        public DateTime PublicationDate { get; }
        public string ISBN { get; }

        public Book(string name, string author, string category, string language, DateTime publicationDate, string iSBN)
        {
            Name = name;
            Author = author;
            Category = category;
            Language = language;
            PublicationDate = publicationDate;
            ISBN = iSBN;
        }

        /// <summary>
        /// used for filling table slots
        /// </summary>
        /// <returns>overriding ToString() method, so we can form tables</returns>
        public override string ToString()
        {
            return String.Format("| {0, -12} | {1, -12} | {2, -12} | {3, -12} | {4, -16} | {5, -15} |",
                Name, Author, Category, Language, PublicationDate.ToString("yyyy-MM-dd"), ISBN);
        }
    }
}