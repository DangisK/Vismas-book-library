using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Vismas_book_library;

namespace Vismas_unit_tests
{
    public class Tests
    {
        private readonly List<Book> books = new();
        private readonly List<TakenBook> takenBooks = new();
        private readonly User userMichael = new("Michael");
        private readonly User userNoBooks = new("No books");

        [OneTimeSetUp]
        public void GlobatSetup()
        {
            InOutUtils.BooksToJson(books);
            InOutUtils.BooksToJson(takenBooks);
        }

        [Test, Order(1)]
        [TestCase("name", "author", "category", "language", "iSBN")]
        [TestCase("name1", "author1", "category1", "language1", "iSBN1")]
        [TestCase("name2", "author2", "category2", "language2", "iSBN2")]
        [TestCase("name3", "author3", "category3", "language3", "iSBN3")]
        [TestCase("name4", "author4", "category4", "language4", "iSBN4")]
        [TestCase("name4", "author4", "category4", "language4", "iSBN4")]
        public void AddANewBook(string name, string author, string category, string language, string iSBN)
        {
            Book book = new(name, author, category, language, DateTime.Now, iSBN);
            Tasks.AddNewBook(books, book);
            Assert.AreEqual(book, books[^1]);
        }

        [Test, Order(2)]
        public void TakeBooks()
        {
            int booksCount = books.Count;
            while (takenBooks.Count(n => n.User == userMichael) < 3)
                Tasks.TakeBook(takenBooks, books, new(books[0], DateTime.Now.AddMonths(2), userMichael), userMichael);
            Assert.AreEqual(3, takenBooks.Count);
            Assert.AreEqual(booksCount - 3, books.Count);
        }

        [Test, Order(3)]
        public void TakeBooksAboveLimit()
        {
            int booksCount = books.Count;
            int takenBooksCount = takenBooks.Count;
            for (int i = 0; i < 2; i++)
            {
                Tasks.TakeBook(takenBooks, books, new TakenBook(books[0], DateTime.Now.AddMonths(2), userMichael), userMichael);
            }
            Assert.AreEqual(takenBooksCount, takenBooks.Count);
            Assert.AreEqual(booksCount, books.Count);
        }

        [Test, Order(4)]
        [TestCase("1", 2, "author4")]
        [TestCase("2", 2, "category4")]
        [TestCase("3", 2, "language4")]
        [TestCase("4", 2, "iSBN4")]
        [TestCase("5", 2, "name4")]
        [TestCase("6", 3, "")]
        [TestCase("7", 3, "")]
        public void FilterBooks(string choice, int answer, string query = "")
        {
            List<Book> filteredBooks = Tasks.FilterBooks(takenBooks, books, choice, query);
            Assert.AreEqual(answer, filteredBooks.Count);
        }

        [Test, Order(5)]
        public void ReturnBooksByInvalidUser()
        {
            TakenBook takenBook = new(books[0], DateTime.Now, userNoBooks);
            int booksCount = books.Count;
            int takenBooksCount = takenBooks.Count;
            Tasks.ReturnBook(takenBooks, books, takenBook, userNoBooks);
            Assert.AreEqual(takenBooksCount, takenBooks.Count);
            Assert.AreEqual(booksCount, books.Count);
        }

        [Test, Order(6)]
        public void ReturnBooks()
        {
            int booksCount = books.Count;
            int takenBooksCount = takenBooks.Count;
            while (takenBooks.Any(n => n.User == userMichael))
                Tasks.ReturnBook(takenBooks, books, takenBooks.Find(n => n.User == userMichael), userMichael);
            Assert.AreEqual(0, takenBooks.Count);
            Assert.AreEqual(booksCount + 3, books.Count);
        }

        [Test, Order(7)]
        public void DeleteBooks()
        {
            int booksCount = books.Count;
            for (int i = 0; i < booksCount; i++)
                Tasks.DeleteBook(takenBooks, books, books[0]);
            Assert.AreEqual(0, books.Count);
        }
    }
}