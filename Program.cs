using System;
using System.ComponentModel.Design;
using System.Text;
using System.Xml.Linq;

namespace BasicLibrary
{
    internal class Program
    {// testing checkout ....

        static List<(string BName, string BAuthor, int ID,int originalQuantity ,int quantity)> Books = new List<(string BName, string BAuthor, int ID, int originalQuantity, int quantity)>();
        static List<(int id,string email,string pw,string name)> Users =new List<(int id,string email,string pw, string name)>();
        static List<(int email, int pw, string name)> Admins = new List<(int email, int pw, string name)>();
        static List<(int uid, int bid, DateTime date)> Borrowings = new List<(int uid, int bid, DateTime date)>();
        static List<(int uid, int bid, DateTime date)> returnings = new List<(int uid, int bid, DateTime date)>();
        static string filePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\lib.txt";
        static string userFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\user.txt";
        static string adminFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\admin.txt";
        static string borrowingFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\borrowing.txt";
        static string returningFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\returning.txt";
        static int index;
        //-----------------------------------------------------------------------------//

        static void Main(string[] args)
        {
            bool ExitFlag = false;
            LoadBooksFromFile();
            do
            {
                Console.WriteLine("Welcome to Library");
                Console.WriteLine("\n Enter the No of operation you need :");
                Console.WriteLine("\n 1 .For Admin");
                Console.WriteLine("\n 2 .For User");
                Console.WriteLine("\n 3 .Save and Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        adminMenu();
                        break;

                    case "2":
                        Console.Clear();
                        userMenu();
                        break;

                    case "3":
                        SaveBooksToFile();
                        Console.WriteLine("\npress any key to exit out system");
                        string outsystem = Console.ReadLine();
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Sorry your choice was wrong!!");
                        break;
                }
                Console.WriteLine("press any key to continue in Home Page");
                string cont = Console.ReadLine();
                Console.Clear();
            } while (ExitFlag != true);
        }
        static void adminMenu()
        {
            bool ExitFlag = false;
            do
            {
                Console.WriteLine("Welcome Admin in Library");
                Console.WriteLine("\n Enter the No. of operation you need :");
                Console.WriteLine("\n 1 .Add New Book");
                Console.WriteLine("\n 2 .Display All Books");
                Console.WriteLine("\n 3 .Search for Book by Name");
                Console.WriteLine("\n 4 .Edit a Book");
                Console.WriteLine("\n 5 .Remove a Book");
                Console.WriteLine("\n 6 .Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewBook();
                        break;

                    case "2":
                        ViewAllBooks();
                        break;

                    case "3":
                        SearchForBook();
                        break;
                    case "4":
                        //editBook();
                        break;
                    case "5":
                        //removeBook();
                        break;
                    case "6":
                        ExitFlag = true;
                        break;
                    default:
                        Console.WriteLine("Sorry your choice was wrong !!");
                        break;

                }

                Console.WriteLine("press any key to continue");
                string cont = Console.ReadLine();

                Console.Clear();

            } while (ExitFlag != true);
        }

        static void userMenu()
        {
            bool ExitFlag = false;
            do
            {
                Console.WriteLine("Welcome User in Library");
                Console.WriteLine("\n Enter the No of operation you need :");
                Console.WriteLine("\n 1 .Search For Book");
                Console.WriteLine("\n 2 .Borrow Book");
                Console.WriteLine("\n 3 .Return Book");
                Console.WriteLine("\n 4 .Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SearchForBook();
                        break;

                    case "2":
                        BorrowBook();
                        break;

                    case "3":
                        ReturnBook();
                        break;

                    case "4":
                        ExitFlag = true;
                        break;

                    default:
                        Console.WriteLine("Sorry your choice was wrong !!");
                        break;

                }

                Console.WriteLine("press any key to continue");
                string cont = Console.ReadLine();

                Console.Clear();

            } while (ExitFlag != true);
        }
        static void AddNewBook()
        {
            string name;
            string author;
            int quantity = 0;

            
            Console.WriteLine("Enter Book Name");
             name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))// to handle the book name
            {
                Console.WriteLine("Error: Book name cannot be empty.");
                return;
            }

            Console.WriteLine("Enter Book Author");
             author = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(author))//to handle the book author
            {
                Console.WriteLine("Error: Author name cannot be empty.");
                return;
            }

            // manually generate the next book ID by checking the IDs in the Books list
            int newID = 1; // Start with 1 if there are no books
            if (Books.Count > 0)
            {
                // Loop through the books to find the highest existing ID
                foreach (var book in Books)
                {
                    if (book.ID >= newID)
                    {
                        newID = book.ID + 1;
                    }
                }
            }

            // to handle the book quantity
            Console.WriteLine("Enter the Book Quantity");
             quantity = int.Parse(Console.ReadLine());
            try
            {
                quantity = int.Parse(Console.ReadLine());
                if (quantity < 0)
                {
                    Console.WriteLine("Error: Quantity cannot be negative.");
                    return;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Please enter a valid number for the Book Quantity.");
                return;
            }
            catch (OverflowException)
            {
                Console.WriteLine("Error: The number for the Book Quantity is too large.");
                return;
            }

            // Add the book if everything is valid
            Books.Add((name, author, newID, quantity, quantity));
            Console.WriteLine($"Book added successfully with ID: {newID} !");

        }

        static void ViewAllBooks()
        {
            StringBuilder sb = new StringBuilder();

            int BookNumber = 0;

            for (int i = 0; i < Books.Count; i++)
            {
                BookNumber = i + 1;
                sb.Append("Book ").Append(BookNumber).Append(" name : ").Append(Books[i].BName);
                sb.AppendLine();
                sb.Append("Book ").Append(BookNumber).Append(" Author : ").Append(Books[i].BAuthor);
                sb.AppendLine();
                sb.Append("Book ").Append(BookNumber).Append(" ID : ").Append(Books[i].ID);
                sb.AppendLine();
                sb.Append("Book ").Append(BookNumber).Append(" Quantity : ").Append(Books[i].quantity);
                sb.AppendLine().AppendLine();
                Console.WriteLine(sb.ToString());
                sb.Clear();

            }
        }

        static void SearchForBook()
        {
            Console.WriteLine("Enter the book name you want");
            string name = Console.ReadLine();
            bool flag = false;

            for (int i = 0; i < Books.Count; i++)
            {
                if (Books[i].BName == name)
                {
                    Console.WriteLine("Book Author is : " + Books[i].BAuthor);
                    flag = true;
                    index= i;
                    break;

                }
            }

            if (flag != true)
            { Console.WriteLine("book not found");
                index = -1;
                    }
        }


        static void BorrowBook() {
            SearchForBook();
            if (index != -1)
            {
                int quantity = Books[index].quantity;
                if (quantity > 0)
                {
                    Console.WriteLine("Do you want to borrow the Book?");
                    Console.WriteLine("\n press char ' y ' to borrow :");
                    string selected = Console.ReadLine();

                    if (selected != "y")
                    {
                        Console.WriteLine("Sorry! Can not borrow this " + Books[index].BName);
                    }

                    else
                    {
                        --quantity;
                        Books[index] = (Books[index].BName, Books[index].BAuthor, Books[index].ID, Books[index].originalQuantity, quantity);

                        Console.WriteLine("you are Borrowed this " + Books[index].BName + "  !");
                        return;

                    }


                }

                else { Console.WriteLine("Sorry! All books are borrowed..."); }
            }
        }
        static void ReturnBook() {
            SearchForBook();
            if (index != -1)
            {
                int quantity = Books[index].quantity;

                Console.WriteLine("Do you want to return the Book?");
                Console.WriteLine("\n press char ' y ' to borrow :");
                string selected = Console.ReadLine();

                if (selected == "y")
                {
                    ++quantity;
                    Books[index] = (Books[index].BName, Books[index].BAuthor, Books[index].ID, Books[index].originalQuantity, quantity);

                    Console.WriteLine("'" + Books[index].BName + "' Book has been returned successfully!");
                    return;
                }



                else { Console.WriteLine("Sorry! Can not return this " + Books[index].BName); }
            }
        }
        static void LoadBooksFromFile()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 5)
                            {
                                Books.Add((parts[0], parts[1], int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4])));
                            }
                        }
                    }
                    Console.WriteLine("Books loaded from file successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }

        static void SaveBooksToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var book in Books)
                    {
                        writer.WriteLine($"{book.BName}|{book.BAuthor}|{book.ID}|{book.quantity}");
                    }
                }
                Console.WriteLine("Books saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

    }
}
