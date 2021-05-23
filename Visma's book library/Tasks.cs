using System;
using System.Collections.Generic;
using System.Linq;

namespace Vismas_book_library
{
    /// <summary>
    /// Class for performing given tasks
    /// </summary>
    public static class Tasks
    {
        // user is allowed to take 3 books maximum
        private const int MAXBOOKS = 3;

        /// <summary>
        /// method for adding a new book, that would have a name,
        /// author, category, language, publication date and ISBN
        /// </summary>
        /// <param name="books">book list</param>
        /// <param name="book">book that's getting added</param>
        public static void AddNewBook(List<Book> books, Book book)
        {
            books.Add(book); // adding successfully created book to the list

            InOutUtils.BooksToJson(books); // adding book to the .json file

            Console.WriteLine();
            Console.WriteLine("Book successfully added.");
            Console.WriteLine();
        }

        /// <summary>
        /// method for taking the book
        /// </summary>
        /// <param name="takenBooks">books that taken books by a user</param>
        /// <param name="books">list of available books</param>
        /// <param name="takenBook">book that is taken by a user</param>
        /// <param name="user">user, that wants to take the book</param>
        public static void TakeBook(List<TakenBook> takenBooks, List<Book> books, TakenBook takenBook, User user)
        {
            if (!CanTakeBook(takenBooks, user) || takenBook.User.Name != user.Name)
            {
                return;
            }
            takenBooks.Add(takenBook);
            books.Remove(takenBook.Book);

            // saving changes to .json files
            InOutUtils.BooksToJson(takenBooks);
            InOutUtils.BooksToJson(books);

            Console.WriteLine("Book taken successfully. Enjoy!"); // letting a user know that he retrieves the book
            Console.WriteLine();
        }

        /// <summary>
        /// checking whether user can take any more books
        /// </summary>
        /// <param name="takenBooks">their taken book list</param>
        /// <param name="user">user, that wants to take the book</param>
        /// <returns>true if they can take the book, false otherwise</returns>
        public static bool CanTakeBook(List<TakenBook> takenBooks, User user)
        {
            return takenBooks.Count(n => n.User.Name == user.Name) < MAXBOOKS;
        }

        /// <summary>
        /// finding the book by given ISBN
        /// </summary>
        /// <param name="books">list of available books</param>
        /// <param name="ISBN">provided ISBN</param>
        /// <returns>the book object if it was found, null otherwise</returns>
        public static Book GetBookByISBN(List<Book> books, string ISBN)
        {
            return books.Find(n => n.ISBN == ISBN);
        }

        /// <summary>
        /// method for returning user's book
        /// </summary>
        /// <param name="takenBooks">list of user's taken books</param>
        /// <param name="books">list of available books</param>
        /// <param name="takenBook">book that's getting returned</param>
        /// <param name="user">user that wants to return the book</param>
        public static void ReturnBook(List<TakenBook> takenBooks, List<Book> books, TakenBook takenBook, User user)
        {
            // checking if the user is eligible to return the book
            if (!HasAnyBooksTaken(takenBooks, user) || takenBook.User.Name != user.Name)
            {
                return;
            }

            // adding the book to the list
            books.Add(takenBook.Book);

            // removing book from user's taken books list
            takenBooks.Remove(takenBook);

            // saving changes to .json files
            InOutUtils.BooksToJson(takenBooks);
            InOutUtils.BooksToJson(books);
        }

        /// <summary>
        /// checking whether user has lent any books
        /// </summary>
        /// <param name="takenBooks">list of taken books</param>
        /// <param name="user">user that's being checked</param>
        /// <returns>true if they have taken any books, false otherwise</returns>
        public static bool HasAnyBooksTaken(List<TakenBook> takenBooks, User user)
        {
            return takenBooks.Any(n => n.User.Name == user.Name);
        }

        /// <summary>
        /// finding a taken book by given ISBN, but
        /// they cannot return the book if they do not own it
        /// </summary>
        /// <param name="takenBooks">user's taken book list</param>
        /// <param name="user">user that wants to return the book</param>
        /// <param name="ISBN">provided ISBN</param>
        /// <returns>a TakenBook object with book information if found, false otherwise</returns>
        public static TakenBook GetBookByISBNByUser(List<TakenBook> takenBooks, User user, string ISBN)
        {
            return takenBooks.Find(n => n.Book.ISBN == ISBN && n.User.Name == user.Name);
        }

        /// <summary>
        /// checking if they returned it late
        /// </summary>
        /// <param name="promisedReturnDate">an indicator when the book had to be returned</param>
        /// <returns>true if it was returned later than agreed on, false otherwise</returns>
        public static bool IsReturnedLate(DateTime promisedReturnDate)
        {
            return DateTime.Now <= promisedReturnDate;
        }

        /// <summary>
        /// used for filtering the books by a parameter
        /// </summary>
        /// <param name="takenBooks">user's taken books</param>
        /// <param name="books">available books</param>
        /// <param name="choice">user's filtering choice</param>
        /// <param name="query">sorting according to user's provided query</param>
        /// <returns></returns>
        public static List<Book> FilterBooks(List<TakenBook> takenBooks, List<Book> books, string choice, string query = "")
        {
            switch (choice)
            {
                case ("1"): // filtering by author
                    {
                        return books.FindAll(n => n.Author == query);
                    }
                case ("2"): // filtering by category
                    {
                        return books.FindAll(n => n.Category == query);
                    }
                case ("3"): // filtering by language
                    {
                        return books.FindAll(n => n.Language == query);
                    }
                case ("4"): // filtering by ISBN
                    {
                        return books.FindAll(n => n.ISBN == query);
                    }
                case ("5"): // filtering by name
                    {
                        return books.FindAll(n => n.Name == query);
                    }
                case ("6"): // showing all taken books
                    {
                        return takenBooks.Select(n => n.Book).ToList();
                    }
                default: // showing all available books
                    {
                        return books;
                    }
            }
        }

        /// <summary>
        /// method for deleting the book
        /// </summary>
        /// <param name="takenBooks">list of books taken from library</param>
        /// <param name="books">list of available books</param>
        /// <param name="book">book to delete</param>
        public static void DeleteBook(List<TakenBook> takenBooks, List<Book> books, Book book)
        {
            // the book to remove
            if (!books.Remove(book))
            {
                // if no books was found in the library, searching in taken books
                takenBooks.Remove(takenBooks.Find(n => n.Book == book));

                InOutUtils.BooksToJson(takenBooks);
                Console.WriteLine();
                Console.WriteLine("Deleted successfully.");
                Console.WriteLine();
            }
            else
            {
                // book was found in available book list and successfully deleted
                InOutUtils.BooksToJson(books);

                Console.WriteLine();
                Console.WriteLine("Deleted successfully.");
                Console.WriteLine();
            }
        }
    }
}