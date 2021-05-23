using System;

namespace Vismas_book_library
{
    /// <summary>
    /// Class for taken books
    /// </summary>
    public class TakenBook
    {
        public Book Book { get; }

        // for checking if user is late to return the book
        public DateTime PromisedReturnDate { get; }

        public User User { get; }

        public TakenBook(Book book, DateTime promisedReturnDate, User user)
        {
            Book = book;
            PromisedReturnDate = promisedReturnDate;
            User = user;
        }

        /// <summary>
        /// used for filling table slots
        /// </summary>
        /// <returns>overriding ToString() method, so we can form tables</returns>
        public override string ToString()
        {
            return String.Format("| {0, -12} | {1, -12} | {2, -12} | {3, -12} | {4, -16} | {5, -15} |",
                Book.Name, Book.Author, Book.Category, Book.Language, Book.PublicationDate.ToString("yyyy-MM-dd"), Book.ISBN);
        }
    }
}