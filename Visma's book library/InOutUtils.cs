using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Vismas_book_library
{
    /// <summary>
    /// class for reading/printing/saving changes to or creating .json files
    /// </summary>
    public static class InOutUtils
    {
        // user is allowed to take 3 books maximum
        private const int MAXBOOKS = 3;

        // user is allowed to lend the book for 2 months maximum
        private const int MAXMONTHS = 2;

        /// <summary>
        /// writing all available books to .json file
        /// </summary>
        /// <param name="books">list of available books</param>
        public static void BooksToJson(List<Book> books)
        {
            string path = @"./books.json";
            var jsonData = JsonConvert.SerializeObject(books);
            File.WriteAllText(path, jsonData);
        }

        /// <summary>
        /// writing all taken books to .json file
        /// </summary>
        /// <param name="takenBooks">list of taken books</param>
        public static void BooksToJson(List<TakenBook> takenBooks)
        {
            string path = @"./takenBooks.json";
            var jsonData = JsonConvert.SerializeObject(takenBooks);
            File.WriteAllText(path, jsonData);
        }

        /// <summary>
        /// method for printing available books
        /// </summary>
        /// <param name="books">available books</param>
        public static void PrintBooks(List<Book> books)
        {
            // checking if there are any books
            if (books.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("There are no books yet or no books fitting this criteria.\nPlease create or return one.");
                Console.WriteLine();
                return;
            }

            // prints header of a table
            PrintHeader();

            foreach (Book book in books)
            {
                Console.WriteLine(book.ToString());
            }
            Console.WriteLine(new string('-', 98));
            Console.WriteLine();
        }

        /// <summary>
        /// method for printing all taken books
        /// </summary>
        /// <param name="takenBooks">taken books list</param>
        public static void PrintBooks(List<TakenBook> takenBooks)
        {
            // checking if there are any taken books
            if (takenBooks.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("There are no books yet or no books fitting this criteria.\nPlease create or return one.");
                Console.WriteLine();
                return;
            }

            // prints header of a table
            PrintHeader();

            foreach (TakenBook takenBook in takenBooks)
            {
                Console.WriteLine(takenBook.ToString());
            }
            Console.WriteLine(new string('-', 98));
            Console.WriteLine();
        }

        /// <summary>
        /// method for printing individual user's taken books
        /// </summary>
        /// <param name="takenBooks">taken books list</param>
        /// <param name="user">user whose taken books we are printing</param>
        public static void PrintTakenBooks(List<TakenBook> takenBooks, User user)
        {
            // checking if they have taken any books
            if (takenBooks.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("There are no books yet or no books fitting this criteria.\nPlease create or return one.");
                Console.WriteLine();
                return;
            }

            List<TakenBook> userBooks = takenBooks.FindAll(n => n.User.Name == user.Name);

            // prints header of a table
            PrintHeader();

            foreach (TakenBook takenBook in userBooks)
            {
                Console.WriteLine(takenBook.ToString());
            }
            Console.WriteLine(new string('-', 98));
            Console.WriteLine();
        }

        /// <summary>
        /// used for retrieving available book list from a .json file
        /// </summary>
        /// <returns>a list of available books</returns>
        public static List<Book> LoadBooks()
        {
            // if .json file exists, we return books, if it does not, we create it
            string path = @"./books.json";
            if (!File.Exists(path))
            {
                return new List<Book>();
            }

            List<Book> books;

            using (StreamReader r = new(path))
            {
                string json = r.ReadToEnd();
                books = JsonConvert.DeserializeObject<List<Book>>(json);
            }
            return books;
        }

        /// <summary>
        /// used for retrieving taken books list from a .json file
        /// </summary>
        /// <returns>a list of taken books</returns>
        public static List<TakenBook> LoadTakenBooks()
        {
            // if .json file exists, we return books, if it does not, we create it
            string path = @"./takenBooks.json";
            if (!File.Exists(path))
            {
                return new List<TakenBook>();
            }

            List<TakenBook> takenBooks;

            using (StreamReader r = new(path))
            {
                string json = r.ReadToEnd();
                takenBooks = JsonConvert.DeserializeObject<List<TakenBook>>(json);
            }
            return takenBooks;
        }

        /// <summary>
        /// method for printing table header
        /// </summary>
        private static void PrintHeader()
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 98));
            Console.WriteLine(String.Format("| {0, -12} | {1, -12} | {2, -12} | {3, -12} | {4, -16} | {5, -15} |",
                "Name", "Author", "Category", "Language", "Publication Date", "ISBN"));
            Console.WriteLine(new string('-', 98));
        }

        /// <summary>
        /// Method for creating a book using user's input
        /// </summary>
        /// <returns>a Book object with user's entered information in it</returns>
        public static Book CreateBook()
        {
            Console.Write("Enter the book's name: ");
            string name = Console.ReadLine();

            Console.Write("Enter the author: ");
            string author = Console.ReadLine();

            Console.Write("Enter the category: ");
            string category = Console.ReadLine();

            Console.Write("Enter the language: ");
            string language = Console.ReadLine();

            // used for tracking if user has entered a valid date
            bool successfulConversion;

            // default datetime
            DateTime publicationDate = DateTime.Now;

            do
            {
                try
                {
                    Console.Write("Enter the publication date or -1 to go back: ");

                    // we want them to enter a valid date
                    var input = Console.ReadLine();

                    if (input == "-1")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Exiting");
                        Console.WriteLine();
                        return null;
                    }
                    publicationDate = DateTime.Parse(input);
                    successfulConversion = true;
                }
                catch
                {
                    successfulConversion = false;
                    Console.WriteLine();
                    Console.WriteLine("Enter a valid datetime, like this: 2000-02-15");
                    Console.WriteLine();
                }
            } while (!successfulConversion);

            Console.Write("Enter the ISBN: ");
            string ISBN = Console.ReadLine();

            return new Book(name, author, category, language, publicationDate, ISBN);
        }

        /// <summary>
        /// method for taking a book by ISBN
        /// </summary>
        /// <param name="books">available book list</param>
        /// <param name="takenBooks">user's taken book list</param>
        /// <param name="user">user that wants to take a book</param>
        /// <returns>a TakenBook object, that has a Book object and user's information</returns>
        public static TakenBook TakeBook(List<Book> books, List<TakenBook> takenBooks, User user)
        {
            // checking whether it is possible to retrieve the book
            if (books.Count == 0)
            {
                Console.WriteLine("There are no books.");
                return null;
            }

            // checking if they already have a maximum allowed amount of books
            if (!Tasks.CanTakeBook(takenBooks, user))
            {
                Console.WriteLine("You cannot have more than {0} books at once.\nReturn the book to receive a new one.", MAXBOOKS);
                return null;
            }

            Console.WriteLine("What book do you want to take? (ISBN code).\nEnter -1 to go back.");
            InOutUtils.PrintBooks(books);
            Book book;

            do
            {
                Console.Write("ISBN: ");

                string givenISBN = Console.ReadLine();

                if (givenISBN == "-1")
                    return null;

                // checking whether the book exists, if it does, assigning to the book object
                book = Tasks.GetBookByISBN(books, givenISBN);

                if (book == null) // error handling
                {
                    Console.WriteLine("Please enter a valid ISBN from a list.\nEnter -1 to go back.");
                    Console.WriteLine();
                }
                else
                    break;

                // cycle repeats until user enters a valid ISBN or enters "-1"
            } while (book == null);

            // used for checking whether user agrees to keep books for agreed amount of time
            int promisedToReturnInMonths;

            do
            {
                Console.WriteLine("How many months are you taking this book for? (Maximum {0} months)", MAXMONTHS);
                Console.WriteLine("Enter -1 to go back.");
                Console.WriteLine();
                promisedToReturnInMonths = Convert.ToInt32(Console.ReadLine());

                if (promisedToReturnInMonths == -1)
                    return null;

                Console.WriteLine();

                // user has to enter 1 or 2 months; -1 to exit
            } while (promisedToReturnInMonths < 1 || promisedToReturnInMonths > MAXMONTHS);

            return new TakenBook(book, DateTime.Now.AddMonths(promisedToReturnInMonths), user);
        }

        /// <summary>
        /// method for returning user's book
        /// </summary>
        /// <param name="takenBooks">list of user's taken books</param>
        /// <param name="user">user that wants to return the book</param>
        public static TakenBook ReturnBook(List<TakenBook> takenBooks, User user)
        {
            // checking if the user is eligible to return the book
            if (!Tasks.HasAnyBooksTaken(takenBooks, user))
            {
                Console.WriteLine("You do not have any books, so you cannot return anything.");
                Console.WriteLine();
                return null;
            }

            TakenBook takenBook;
            Console.WriteLine("Enter an ISBN of the book you want to return.\nEnter -1 to go back");
            InOutUtils.PrintTakenBooks(takenBooks, user);

            do
            {
                Console.Write("ISBN: ");
                string givenISBN = Console.ReadLine();
                if (givenISBN == "-1")
                    return null;

                // finding book by provided ISBN, they cannot return the book if they do not own it
                takenBook = Tasks.GetBookByISBNByUser(takenBooks, user, givenISBN);

                if (takenBook == null)
                {
                    Console.WriteLine("Please enter a valid ISBN from a list.\nEnter -1 to go back.");
                    Console.WriteLine();
                }
                else
                {
                    // letting the user know about successful removal
                    Console.WriteLine("Book was successfully returned.");
                    Console.WriteLine();
                    break;
                }

                // repeating until the user enters a valid ISBN or enters -1
            } while (takenBook == null);

            // checking if they returned the book late
            if (Tasks.IsReturnedLate(takenBook.PromisedReturnDate))
            {
                Console.WriteLine("Not cool man, please keep your promises (not epic).");
                Console.WriteLine();
            }

            return takenBook;
        }

        public static List<Book> FilterBooks(List<TakenBook> takenBooks, List<Book> books)
        {
            while (true)
            {
                Console.WriteLine("Choose filtering method by entering a digit or -1 to exit:");
                Console.WriteLine("1. Author\n2. Category\n3. Language\n4. ISBN\n5. Name\n6. Taken or available books");
                Console.WriteLine();

                switch (Console.ReadLine())
                {
                    case ("1"): // filtering by author
                        {
                            if (books.Count == 0)
                            {
                                Console.WriteLine("No books in the library fitting this criteria.");
                                return null;
                            }
                            InOutUtils.PrintBooks(books);
                            Console.Write("Enter an author: ");
                            string author = Console.ReadLine();
                            return Tasks.FilterBooks(takenBooks, books, "1", author);
                        }
                    case ("2"): // filtering by category
                        {
                            if (books.Count == 0)
                            {
                                Console.WriteLine("No books in the library fitting this criteria.");
                                return null;
                            }
                            InOutUtils.PrintBooks(books);
                            Console.Write("Enter a category: ");
                            string category = Console.ReadLine();
                            return Tasks.FilterBooks(takenBooks, books, "2", category);
                        }
                    case ("3"): // filtering by language
                        {
                            if (books.Count == 0)
                            {
                                Console.WriteLine("No books in the library fitting this criteria.");
                                return null;
                            }
                            InOutUtils.PrintBooks(books);
                            Console.Write("Enter a language: ");
                            string language = Console.ReadLine();
                            return Tasks.FilterBooks(takenBooks, books, "3", language);
                        }
                    case ("4"): // filtering by ISBN
                        {
                            if (books.Count == 0)
                            {
                                Console.WriteLine("No books in the library fitting this criteria.");
                                return null;
                            }
                            InOutUtils.PrintBooks(books);
                            Console.Write("Enter an ISBN: ");
                            string ISBN = Console.ReadLine();
                            return Tasks.FilterBooks(takenBooks, books, "4", ISBN);
                        }
                    case ("5"): // filtering by name
                        {
                            if (books.Count == 0)
                            {
                                Console.WriteLine("No books in the library fitting this criteria.");
                                return null;
                            }
                            InOutUtils.PrintBooks(books);
                            Console.Write("Enter a name: ");
                            string name = Console.ReadLine();
                            return Tasks.FilterBooks(takenBooks, books, "5", name);
                        }
                    case ("6"): // showing all available or taken books
                        {
                            while (true)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Do you want taken or available books? Write -1 to exit.");
                                Console.WriteLine("1. Taken books\n2. Available books");
                                Console.WriteLine();
                                string choice = Console.ReadLine();

                                if (choice == "1") // printing all taken books
                                {
                                    if (takenBooks.Count == 0)
                                    {
                                        Console.WriteLine("No books in the library fitting this criteria.");
                                        return null;
                                    }
                                    return Tasks.FilterBooks(takenBooks, books, "6");
                                }
                                else if (choice == "2") // printing all available books
                                {
                                    if (books.Count == 0)
                                    {
                                        Console.WriteLine("No books in the library fitting this criteria.");
                                        return null;
                                    }
                                    return Tasks.FilterBooks(takenBooks, books, "7");
                                }
                                else if (choice == "-1") // exiting the loop
                                    return null;
                            }
                        }
                    case ("-1"): // exiting to main menu
                        {
                            return null;
                        }
                    default: // error handling
                        {
                            Console.WriteLine("No such option");
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// method for deleting the book
        /// </summary>
        /// <param name="takenBooks">list of user's taken books</param>
        /// <param name="books">list of available books</param>
        public static Book DeleteBook(List<TakenBook> takenBooks, List<Book> books)
        {
            Console.WriteLine("Available books:");
            InOutUtils.PrintBooks(books);

            Console.WriteLine("Taken books");
            InOutUtils.PrintBooks(takenBooks);

            Console.Write("Which book you want to delete?\nEnter ISBN: ");
            string ISBN = Console.ReadLine();
            Console.WriteLine();

            Book book;

            // checking if book with given ISBN exists in available books
            if (!books.Exists(n => n.ISBN == ISBN))
                // checking if book with given ISBN exists in taken books
                if (!takenBooks.Exists(n => n.Book.ISBN == ISBN))
                {
                    // given ISBN is invalid
                    Console.WriteLine("No book found with ISBN: {0} in database", ISBN);
                    Console.WriteLine();
                    book = null;
                }
                else
                {
                    book = takenBooks.Find(n => n.Book.ISBN == ISBN).Book;
                }
            else
            {
                book = books.Find(n => n.ISBN == ISBN);
            }

            return book;
        }
    }
}