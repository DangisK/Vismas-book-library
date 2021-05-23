using System;
using System.Collections.Generic;

namespace Vismas_book_library
{
    internal class Program
    {
        private static void Main()
        {
            List<TakenBook> takenBooks = InOutUtils.LoadTakenBooks();

            List<Book> books = InOutUtils.LoadBooks();

            Console.Write("What's your name? ");
            string name = Console.ReadLine();
            User user;

            // creating a user if name does not appear in taken book list
            if (takenBooks.Find(n => n.User.Name == name) == null)
                user = new User(name);
            else
                // assigning user to the one found in taken book list
                user = takenBooks.Find(n => n.User.Name == name).User;

            Console.WriteLine();

            // greeting our user
            Console.WriteLine("Hello, {0}", user.Name);
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Enter a number of an action you would like to perform or -1 to exit: ");
                Console.WriteLine();
                Console.WriteLine("1. Add a new book\n2. Take a book from the library\n3. Return a book\n4. List all the filtered books\n5. Delete a book");
                Console.WriteLine();

                switch (Console.ReadLine())
                {
                    case ("1"): // adding a new book
                        {
                            Console.WriteLine();
                            Book bookToCreate = InOutUtils.CreateBook();
                            if (bookToCreate != null)
                                Tasks.AddNewBook(books, bookToCreate);
                            break;
                        }
                    case ("2"): // taking the book from the library
                        {
                            Console.WriteLine();
                            TakenBook bookToTake = InOutUtils.TakeBook(books, takenBooks, user);
                            if (bookToTake != null)
                                Tasks.TakeBook(takenBooks, books, bookToTake, user);
                            break;
                        }
                    case ("3"): // returning the book
                        {
                            Console.WriteLine();
                            TakenBook bookToReturn = InOutUtils.ReturnBook(takenBooks, user);
                            if (bookToReturn != null)
                                Tasks.ReturnBook(takenBooks, books, bookToReturn, user);
                            break;
                        }
                    case ("4"): // filtering books by given criteria
                        {
                            Console.WriteLine();
                            List<Book> filteredBooks = InOutUtils.FilterBooks(takenBooks, books);
                            if (filteredBooks != null)
                                InOutUtils.PrintBooks(filteredBooks);
                            break;
                        }
                    case ("5"): // deleting the book
                        {
                            Console.WriteLine();
                            Book bookToDelete = InOutUtils.DeleteBook(takenBooks, books);
                            if (bookToDelete != null)
                                Tasks.DeleteBook(takenBooks, books, bookToDelete);
                            break;
                        }
                    case ("-1"): // exiting the program
                        {
                            Console.WriteLine();
                            Console.WriteLine("Returning...");
                            return;
                        }
                    default: // error handling
                        {
                            Console.WriteLine();
                            Console.WriteLine("No such option");
                            break;
                        }
                }
            }
        }
    }
}