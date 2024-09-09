using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Security.Principal;
using System.Text;
using System.Xml.Linq;

namespace BasicLibrary
{
    internal class Program
    {// TEST 
        //........................Necessary variables and path files.....................................//

        static List<(string BName, string BAuthor, int ID,int originalQuantity ,int quantity)> Books = new List<(string BName, string BAuthor, int ID, int originalQuantity, int quantity)>();
        static List<(int id,string email,string pw,string name)> Users =new List<(int id,string email,string pw, string name)>();
        static List<(string email, string pw, string name)> Admins = new List<(string email, string pw, string name)>();
        static List<(int uid, int bid, DateTime date,bool returnBook)> Borrowings = new List<(int uid, int bid, DateTime date, bool returnBook)>();
        static List<(int uid, int bid, DateTime date)> returnings = new List<(int uid, int bid, DateTime date)>();
        static string filePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\lib.txt";
        static string userFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\user.txt";
        static string adminFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\admin.txt";
        static string borrowingFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\borrowing.txt";
        static string returningFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\returning.txt";
        static int index = -1;
        //-----------------------------------------------------------------------------//

        static void Main(string[] args)
        {
            
            LoadAllFiles();
            loginPage();

        }

        //........................Functions Part.......................................//

        //........................Login Functions.....................................//
        static void loginPage( ) 
        {
            bool ExitFlag = false;
            do
            {
                Console.WriteLine("------------Welcome to Library------------");
                Console.WriteLine("\n----------------Login Page----------------");
                Console.WriteLine("\n Enter the No what you are :");
                Console.WriteLine("\n 0 . Users Register");
                Console.WriteLine("\n 1 . Admin Access");
                Console.WriteLine("\n 2 . User Access");
                Console.WriteLine("\n 3 .Exit");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "0":
                        Console.Write("Enter Your Email: ");

                        string rEmail = Console.ReadLine();
                        if (rEmail == "registrar")
                        {
                            var registrar = Admins.FirstOrDefault(r => r.email == rEmail);
                            if (registrar != default)
                            {
                                Console.Write("\nEnter Password: ");
                                string enterPW = Console.ReadLine();
                                if (enterPW == registrar.pw)
                                {
                                    accountsManagement(registrar.name);
                                }
                            }
                        }
                        else { Console.WriteLine("Sorry! you are not allowed to access here..."); }
                        break;
                    case "1":
                        Console.Clear();
                        // string user2 = "Admin";
                        // LoginAccess(user2);
                        Console.Write("Enter Your Email: ");
                        
                        string aEmail = Console.ReadLine();
                      if (aEmail != "registrar"){
                            var admin = Admins.FirstOrDefault(a => a.email == aEmail);
                            if (admin != default)
                            {
                                Console.Write("\nEnter Password: ");
                                string enterPW = Console.ReadLine();
                                if (enterPW == admin.pw)
                                {
                                    adminMenu(admin.name);
                                }
                            }
                        else { Console.WriteLine("Sorry! you are not allowed to access here..."); }

                        }
                        else { Console.WriteLine("Sorry! you are not allowed to access here..."); }
                        break;
                    case "2":
                        Console.Clear();
                        Console.Write("Enter Your Email: ");

                        string uEmail = Console.ReadLine();
                        var user = Users.FirstOrDefault(u => u.email == uEmail);
                        if (user != default)
                        {
                            Console.Write("\nEnter Password: ");
                            string enterPW = Console.ReadLine();
                            if (enterPW == user.pw)
                            {
                                userMenu(user.id, user.name);
                            }
                        }
                        else { Console.WriteLine("Sorry! you are not allowed to access here..."); }
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
        static void LoginAccess(string userAccess)
        { if (userAccess == "coustomer")
            {
                Console.Write("Enter Your Email: ");

                string uEmail = Console.ReadLine();
                var user = Users.FirstOrDefault(u => u.email == uEmail);
                if (user != default)
                {
                    Console.Write("\nEnter Password: ");
                    string enterPW = Console.ReadLine();
                    if (enterPW == user.pw)
                    {
                        userMenu(user.id, user.name);
                    }
                }
            } 

            /////
            else if (userAccess == "admin")
            {
                Console.Write("Enter Your Email: ");

                string aEmail = Console.ReadLine();
                var admin = Admins.FirstOrDefault(a => a.email == aEmail);
                if (admin != default)
                {
                    Console.Write("\nEnter Password: ");
                    string enterPW = Console.ReadLine();
                    if (enterPW == admin.pw)
                    {
                        adminMenu(admin.name);
                    }
                }
            }
            else { Console.WriteLine("to there any user like that..."); }
        }
        //.........................registrar function.................................//

        static void accountsManagement(string name)
        {
            bool ExitFlag = false;
            do
            {
                Console.WriteLine("- Accounts Management -");
                Console.WriteLine("\n Enter the No. of operation you need :");
                Console.WriteLine("\n 1 .Add new user");
                Console.WriteLine("\n 2 .Edit user information");
                Console.WriteLine("\n 3 .Remove user account");
                Console.WriteLine("\n 4 .singOut");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewUser();
                        break;
                    case "2":
                        EditUserInformation();
                        break;
                    case "3":
                        RemoveUserAccount();
                        break;
                    case "4":
                        saveAllUsers();
                        Console.WriteLine("\npress any key to exit out system");
                        string outsystem = Console.ReadLine();
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
        static void AddNewUser()
        {
            
            Console.WriteLine("Would you like to add a new User or Admin?");
            Console.WriteLine("Enter '1' for User or '2' for Admin:");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": // Add a new User
                    AddUser();
                    break;

                case "2": // Add a new Admin
                    AddAdmin();
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please enter '1' for User or '2' for Admin.");
                    break;
            }
        }

        static void AddUser()
        {
            
                // Determine the next ID by finding the highest existing ID and incrementing it
                int nextID = 1; // Start with 1 if there are no books
                if (Users.Count > 0)
                {
                    // Loop through the books to find the highest existing ID
                    foreach (var user in Users)
                    {
                        if (user.id >= nextID)
                        {
                            nextID = user.id + 1;
                        }
                    }
                }

                Console.WriteLine("Enter User Email:");
                string email = Console.ReadLine();
            if (Users.Any(u => u.email.ToLower() == email.ToLower())) // Compare emails case-insensitively
            {
                Console.WriteLine("Error: A user with this email already exists.");
                return;
            }

            Console.WriteLine("Enter User Password:");
                string password = Console.ReadLine();

                Console.WriteLine("Enter User Name:");
                string name = Console.ReadLine();

                // Add new user to the Users list with the auto-generated ID
                Users.Add((nextID, email, password, name));
                Console.WriteLine($"New user added successfully with ID: {nextID}");
            
        }

        static void AddAdmin()
        {
            Console.WriteLine("Enter Admin Email:");
            string email = Console.ReadLine();
            if (Admins.Any(a => a.email.ToLower() == email.ToLower())) // Case-insensitive check
            {
                Console.WriteLine("Error: An admin with this email already exists.");
                return;
            }
            Console.WriteLine("Enter Admin Password:");
            string pw = Console.ReadLine();

            Console.WriteLine("Enter Admin Name:");
            string name = Console.ReadLine();

            // Add admin to the list
            Admins.Add((email, pw, name));
            Console.WriteLine("Admin added successfully.");
        }

        static void EditUserInformation()
        {
            ViewAllUsers();
            Console.WriteLine("Enter the type of account to edit (user/admin):");
            string accountType = Console.ReadLine().ToLower();

            switch (accountType)
            {
                case "user":
                    EditUser();
                    break;
                case "admin":
                    EditAdmin();
                    break;
                default:
                    Console.WriteLine("Error: Invalid account type. Please enter 'user' or 'admin'.");
                    break;
            }
        }
        static void EditUser()
        {
            Console.WriteLine("Enter the User ID of the user you want to edit:");
            if (!int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("Error: Invalid ID format.");
                return;
            }

            var user = Users.FirstOrDefault(u => u.id == userId);

            if (user.id == 0)
            {
                Console.WriteLine("Error: User not found.");
                return;
            }

            Console.WriteLine($"Current details for User ID {userId}:");
            Console.WriteLine($"Email: {user.email}");
            Console.WriteLine($"Name: {user.name}");

            Console.WriteLine("Enter new email (leave blank to keep current):");
            string newEmail = Console.ReadLine();
            Console.WriteLine("Enter new password (leave blank to keep current):");
            string newPassword = Console.ReadLine();
            Console.WriteLine("Enter new name (leave blank to keep current):");
            string newName = Console.ReadLine();

            // Confirmation
            Console.WriteLine("Confirm the changes:");
            Console.WriteLine($"New Email: {(string.IsNullOrWhiteSpace(newEmail) ? user.email : newEmail)}");
            Console.WriteLine($"New Password: {(string.IsNullOrWhiteSpace(newPassword) ? "Not changed" : newPassword)}");
            Console.WriteLine($"New Name: {(string.IsNullOrWhiteSpace(newName) ? user.name : newName)}");
            Console.WriteLine("Are you sure you want to apply these changes? (y/n)");
            string confirmation = Console.ReadLine();

            if (confirmation.ToLower() == "y")
            {
                // Update the user information if confirmed
                Users = Users.Select(u =>
                    u.id == userId ?
                    (u.id,
                     string.IsNullOrWhiteSpace(newEmail) ? u.email : newEmail,
                     string.IsNullOrWhiteSpace(newPassword) ? u.pw : newPassword,
                     string.IsNullOrWhiteSpace(newName) ? u.name : newName)
                    : u
                ).ToList();

                Console.WriteLine("User information updated successfully.");
            }
            else
            {
                Console.WriteLine("User information update canceled.");
            }
        }
        static void EditAdmin()
        {
            Console.WriteLine("Enter the email of the admin you want to edit:");
            string email = Console.ReadLine();

            var admin = Admins.FirstOrDefault(a => a.email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (admin == default)
            {
                Console.WriteLine("Error: Admin not found.");
                return;
            }

            Console.WriteLine($"Current details for Admin Email {email}:");
            Console.WriteLine($"Name: {admin.name}");

            Console.WriteLine("Enter new password (leave blank to keep current):");
            string newPassword = Console.ReadLine();
            Console.WriteLine("Enter new name (leave blank to keep current):");
            string newName = Console.ReadLine();

            // Confirmation
            Console.WriteLine("Confirm the changes:");
            Console.WriteLine($"New Password: {(string.IsNullOrWhiteSpace(newPassword) ? "Not changed" : newPassword)}");
            Console.WriteLine($"New Name: {(string.IsNullOrWhiteSpace(newName) ? admin.name : newName)}");
            Console.WriteLine("Are you sure you want to apply these changes? (y/n)");
            string confirmation = Console.ReadLine();

            if (confirmation.ToLower() == "y")
            {
                // Update the admin information if confirmed
                Admins = Admins.Select(a =>
                    a.email.Equals(email, StringComparison.OrdinalIgnoreCase) ?
                    (a.email,
                     string.IsNullOrWhiteSpace(newPassword) ? a.pw : newPassword,
                     string.IsNullOrWhiteSpace(newName) ? a.name : newName)
                    : a
                ).ToList();

                Console.WriteLine("Admin information updated successfully.");
            }
            else
            {
                Console.WriteLine("Admin information update canceled.");
            }
        }

        static void RemoveUserAccount()
        {
            ViewAllUsers();
            Console.WriteLine("Enter the type of account to remove (user/admin):");
            string accountType = Console.ReadLine().ToLower();

            switch (accountType)
            {
                case "user":
                    RemoveUser();
                    break;
                case "admin":
                    RemoveAdmin();
                    break;
                default:
                    Console.WriteLine("Error: Invalid account type. Please enter 'user' or 'admin'.");
                    break;
            }
        }

        static void RemoveUser()
        {
            try
            {
                Console.WriteLine("Enter the User ID of the user you want to remove:");
                if (!int.TryParse(Console.ReadLine(), out int userId))
                {
                    Console.WriteLine("Error: Invalid ID format.");
                    return;
                }

                var user = Users.FirstOrDefault(u => u.id == userId);

                if (user.id == 0)
                {
                    Console.WriteLine("Error: User not found.");
                    return;
                }

                Console.WriteLine($"Confirm removal of User ID {userId}:");
                Console.WriteLine($"Email: {user.email}");
                Console.WriteLine($"Name: {user.name}");
                Console.WriteLine("Are you sure you want to remove this user? (y/n)");
                string confirmation = Console.ReadLine();

                if (confirmation.ToLower() == "y")
                {
                    Users = Users.Where(u => u.id != userId).ToList();
                    Console.WriteLine("User removed successfully.");
                }
                else
                {
                    Console.WriteLine("User removal canceled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while removing the user: " + ex.Message);
            }
        }

        static void RemoveAdmin()
        {
            try
            {
                Console.WriteLine("Enter the email of the admin you want to remove:");
                string email = Console.ReadLine();
                // Prevent the removal of the "registrar" admin
                if (email.Equals("registrar", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Error: The 'registrar' admin cannot be removed.");
                    return;
                }

                var admin = Admins.FirstOrDefault(a => a.email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (admin == default)
                {
                    Console.WriteLine("Error: Admin not found.");
                    return;
                }

                Console.WriteLine($"Confirm removal of Admin with Email {email}:");
                Console.WriteLine($"Name: {admin.name}");
                Console.WriteLine("Are you sure you want to remove this admin? (y/n)");
                string confirmation = Console.ReadLine();

                if (confirmation.ToLower() == "y")
                {
                    Admins = Admins.Where(a => !a.email.Equals(email, StringComparison.OrdinalIgnoreCase)).ToList();
                    Console.WriteLine("Admin removed successfully.");
                }
                else
                {
                    Console.WriteLine("Admin removal canceled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while removing the admin: " + ex.Message);
            }
        }
        static void ViewAllUsers()
        {
            StringBuilder sb = new StringBuilder();

            int userNumber = 0;
            sb.Append("\n \t---All Users in Library System---");
            sb.Append("\n \t---Admins---");
            sb.Append("\n \tEmail\tPassword\tName");
            for (int i = 0; i < Admins.Count; i++)
            {
                userNumber = i + 1;
                sb.AppendLine($"\n({userNumber})\t {Admins[i].email}\t{Admins[i].pw}\t{Admins[i].name}");
                sb.AppendLine();
                Console.WriteLine(sb.ToString());
                sb.Clear();

            }
            userNumber = 0;
            sb.Append("\n\n \t---Users---");
            sb.Append("\n \tEmail\tPassword\tID\tName");
            for (int i = 0; i < Admins.Count; i++)
            {
                userNumber = i + 1;
                sb.AppendLine($"\n({userNumber})\t{Users[i].email}\t{Users[i].pw}\t{Users[i].id}\t{Users[i].name}");
                sb.AppendLine();
                Console.WriteLine(sb.ToString());
                sb.Clear();
            }
        }

        //........................Admin Functions.....................................//
        static void adminMenu(string name)
        {
            //{
            //     accountsManagement();
            //  }
            //  else
            //  {
           // if (name != "registrar") { 
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
                Console.WriteLine("\n 6 .Report");
                Console.WriteLine("\n 7 .singOut");

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
                        editBook();
                        break;
                    case "5":
                        removeBook();
                        break;
                    case "6":
                        reporting();
                        break;
                    case "7":
                        SaveBooksToFile();
                        Console.WriteLine("\npress any key to exit out system");
                        string outsystem = Console.ReadLine();
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
       // }
          
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
        static void editBook()
        {

            // Check if there are books to edit
            if (Books.Count == 0)
            {
                Console.WriteLine("No books available to edit.");
                return;
            }
            SearchForBook();

            var book = Books[index];

            Console.WriteLine($"Editing Book: {book.BName} by {book.BAuthor}");
            Console.WriteLine("Enter new Book Name (or press Enter to skip):");
            string newName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newName)) //is a better and safer check for ensuring that the input is valid (not null, not empty, and not just whitespace).
            {
                book.BName = newName;
            }

            Console.WriteLine("Enter new Book Author (or press Enter to skip):");
            string newAuthor = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newAuthor))
            {
                book.BAuthor = newAuthor;
            }

            Console.WriteLine("Enter new Book Quantity (or press Enter to skip):");
            string quantityInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(quantityInput))
            {
                try
                {
                    int newQuantity = int.Parse(quantityInput);
                    if (newQuantity < 0)
                    {
                        Console.WriteLine("Error: Quantity cannot be negative.");
                        return;
                    }
                    book.quantity = newQuantity;
                    book.originalQuantity = newQuantity;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Error: Please enter a valid number for the quantity.");
                    return;
                }
            }

            // Update the book in the list
            Books[index] = book;
            Console.WriteLine("Book details updated successfully.");

        }
        static void removeBook()
        {
            SearchForBook();
            // If a valid book index is found
            if (index != -1)
            {
                // Ask for confirmation
                Console.WriteLine($"Are you sure you want to delete the book '{Books[index].BName}' by {Books[index].BAuthor}? (y/n)");
                string confirmation = Console.ReadLine();

                if (confirmation.ToLower() == "y")
                {
                    // Remove the book from the list
                    Books.RemoveAt(index);
                    Console.WriteLine("Book has been successfully removed.");
                }
                else
                {
                    Console.WriteLine("Book removal canceled.");
                }
            }

        }
        static void reporting()
        {
            Console.WriteLine("\n------\tToday's Report\t-----");
            // List of tuples to track how many times each book has been borrowed: (bookId, count)
            List<(int bookId, int count)> bookBorrowCount = new List<(int bookId, int count)>();

            // List of tuples to track how many times each author has been borrowed: (authorName, count)
            List<(string authorName, int count)> authorBorrowCount = new List<(string authorName, int count)>();
            foreach (var borrowing in Borrowings)
            {
                int bookId = borrowing.bid;

                // Update book borrow count
                bool bookFound = false;
                for (int i = 0; i < bookBorrowCount.Count; i++)
                {
                    if (bookBorrowCount[i].bookId == bookId)
                    {
                        bookBorrowCount[i] = (bookBorrowCount[i].bookId, bookBorrowCount[i].count + 1);
                        bookFound = true;
                        break;
                    }
                }
                if (!bookFound)
                {
                    bookBorrowCount.Add((bookId, 1));
                }

                // Find the book to get the author
                var book = Books.FirstOrDefault(b => b.ID == bookId);
                if (book != default)
                {
                    string author = book.BAuthor;

                    // Update author borrow count
                    bool authorFound = false;
                    for (int i = 0; i < authorBorrowCount.Count; i++)
                    {
                        if (authorBorrowCount[i].authorName == author)
                        {
                            authorBorrowCount[i] = (authorBorrowCount[i].authorName, authorBorrowCount[i].count + 1);
                            authorFound = true;
                            break;
                        }
                    }
                    if (!authorFound)
                    {
                        authorBorrowCount.Add((author, 1));
                    }
                }
            }

            // Use Max() function to find the most borrowed book
            if (bookBorrowCount.Count > 0)
            {
                int maxBorrowCount = bookBorrowCount.Max(b => b.count);
                var mostBorrowedBook = bookBorrowCount.FirstOrDefault(b => b.count == maxBorrowCount);

                var book = Books.FirstOrDefault(b => b.ID == mostBorrowedBook.bookId);
                Console.WriteLine($"Most Borrowed Book: '{book.BName}' by {book.BAuthor} (Borrowed {maxBorrowCount} times)");
            }
            else
            {
                Console.WriteLine("No books have been borrowed yet.");
            }

            // Find the most requested author
            if (authorBorrowCount.Count > 0)
            {
                int maxAuthorBorrowCount = authorBorrowCount.Max(a => a.count);
                var mostRequestedAuthor = authorBorrowCount.FirstOrDefault(a => a.count == maxAuthorBorrowCount);

                Console.WriteLine($"Most Requested Author: {mostRequestedAuthor.authorName} (Borrowed {maxAuthorBorrowCount} times)");
            }
            else
            {
                Console.WriteLine("No authors have been borrowed yet.");
            }
        }

        //........................User Functions.....................................//
        static void userMenu(int id,string name)
        {
            bool ExitFlag = false;
            do
            {
                Console.WriteLine($"Welcome {name} in Library");
                Console.WriteLine("\n Enter the No of operation you need :");
                Console.WriteLine("\n 1 .Search For Book");
                Console.WriteLine("\n 2 .Borrow Book");
                Console.WriteLine("\n 3 .Return Book");
                Console.WriteLine("\n 4 .Suggestions For you");
                Console.WriteLine("\n 5 .singOut");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SearchForBook();
                        break;

                    case "2":
                        BorrowBook(id);
                        break;

                    case "3":
                        ReturnBook(id);
                        break;

                    case "4":
                        suggestionsForUser(id);
                        break;


                    case "5":
                        SaveBooksToFile();
                        saveAllActions();
                        Console.WriteLine("\npress any key to exit out system");
                        string outsystem = Console.ReadLine();
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
        static void BorrowBook(int userId) {
            try
            {
                SearchForBook();
                if (index != -1)
                {
                    var existingBorrowing = Borrowings.FirstOrDefault(b => b.uid == userId && b.bid == Books[index].ID && b.returnBook == false);
                    if (existingBorrowing != default)
                    {
                        Console.WriteLine("Error: You have already borrowed this book and haven't returned it yet.");
                        return;
                    }

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
                            Borrowings.Add((userId, Books[index].ID, DateTime.Now,false));
                            Console.WriteLine("You have borrowed the " + Books[index].BName + "!");
                            suggestionsForUser(userId, Books[index].ID);

                        }


                    }

                    else { Console.WriteLine("Sorry! All books are borrowed..."); }
                }
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors
                Console.WriteLine("An error occurred while borrowing the book: " + ex.Message);
            }
        }
        static void ReturnBook(int userId) {

            try
            {
                DisplayYourBookBorrowed(userId);
                if (index != -1)
                {
                    int quantity = Books[index].quantity;
                    int originalQuantity = Books[index].originalQuantity;

                    // Check if the user has borrowed this book
                    var borrowingRecord = Borrowings.FirstOrDefault(b => b.uid == userId && b.bid == Books[index].ID && !b.returnBook);

                    if (borrowingRecord == default)
                    {
                        Console.WriteLine("Error: You have not borrowed this book.");
                        return;
                    }
                    
                    Console.WriteLine("Do you want to return the Book?");
                    Console.WriteLine("\n press char ' y ' to borrow :");
                    string selected = Console.ReadLine();

                    if (selected != "y")
                    {
                        Console.WriteLine("Sorry! Cannot return this " + Books[index].BName);

                    }
                    else
                    {// Check if the current quantity equals the original quantity
                        if (quantity >= originalQuantity )
                        {
                            Console.WriteLine("Error: Cannot return the book. All copies have already been returned.");
                        }
                        else
                        {
                            ++quantity;
                            Books[index] = (Books[index].BName, Books[index].BAuthor, Books[index].ID, Books[index].originalQuantity, quantity);

                            // Update the Borrowings list 
                            for (int i = 0; i < Borrowings.Count; i++)
                            {
                                if (Borrowings[i].uid == userId && Borrowings[i].bid == Books[index].ID && Borrowings[i].returnBook == false)
                                {
                                    Borrowings[i] = (Borrowings[i].uid, Borrowings[i].bid, Borrowings[i].date, true);
                                }
                            }

                            returnings.Add((userId, Books[index].ID, DateTime.Now));
                            Console.WriteLine("'" + Books[index].BName + "' Book has been returned successfully!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while returning the book: " + ex.Message);
            }
        }
        static void DisplayYourBookBorrowed(int userId)
        {
            // Check if the user has borrowed any books
            var userBorrowings = Borrowings.Where(b => b.uid == userId).ToList();

            if (userBorrowings.Count == 0)
            {
                Console.WriteLine("You have not borrowed any books.");
                return;
            }

            Console.WriteLine("Books you have borrowed:");

            foreach (var borrowing in userBorrowings)
            {
                // Find the book details based on the book ID
                var book = Books.FirstOrDefault(b => b.ID == borrowing.bid);

                if (book != default && borrowing.returnBook!=true)
                {
                    Console.WriteLine($"- '{book.BName}' by {book.BAuthor} (Borrowed on: {borrowing.date.ToShortDateString()})");
                }
            }

            // Allow the user to select a book for return if desired
            Console.WriteLine("\nEnter the name of the book you want to return or press Enter to skip:");
            string bookName = Console.ReadLine();

            if (!string.IsNullOrEmpty(bookName))
            {
                // Find the book in user's borrowed books
                var selectedBook = Books.FirstOrDefault(b => b.BName.ToLower() == bookName.ToLower());

                if (selectedBook != default)
                {
                    index = Books.IndexOf(selectedBook);
                }
                else
                {
                    Console.WriteLine("Book not found in your borrowed list.");
                    index = -1;
                }
            }
            else
            {
                index = -1; // No book selected for return
            }
        }
        static void suggestionsForUser(int userID,int BookID=0)
        {
            if (BookID == 0)
            {
                try
                {
                    List<int> bookIds = new List<int>();
                    List<int> borrowCounts = new List<int>();
                    // Count the borrowings for each book
                    foreach (var borrowing in Borrowings)
                    {
                        int bookId = borrowing.bid;
                        int index = bookIds.IndexOf(bookId);

                        if (index == -1)
                        {
                            // If the book ID is not in the list, add it with an initial count
                            bookIds.Add(bookId);
                            borrowCounts.Add(1);
                        }
                        else
                        {
                            // If the book ID is already in the list, increment its count
                            borrowCounts[index]++;
                        }
                    }

                    // Find the book with the highest borrow count
                    int mostBorrowedBookId = -1;
                    int maxBorrowCount = 0;

                    for (int i = 0; i < borrowCounts.Count; i++)
                    {
                        if (borrowCounts[i] > maxBorrowCount)
                        {
                            mostBorrowedBookId = bookIds[i];
                            maxBorrowCount = borrowCounts[i];
                        }
                    }
                    string BookName = null;
                    foreach (var book in Books)
                    {
                        if (book.ID == mostBorrowedBookId)
                            BookName = book.BName;
                    }

                    // Output the ID of the most borrowed book
                    Console.WriteLine($"Most Borrowed Book ID: {mostBorrowedBookId} the name book:{BookName}");


                    // Create lists to track authors and their corresponding borrow counts
                    List<string> authors = new List<string>();
                    List<int> authorCounts = new List<int>();

                    // Count the borrowings for each author
                    foreach (var borrowing in Borrowings)
                    {
                        // Find the book corresponding to the borrowing
                        var book = Books.FirstOrDefault(b => b.ID == borrowing.bid);
                        if (book != default)
                        {
                            string author = book.BAuthor;
                            int index = authors.IndexOf(author);

                            if (index == -1)
                            {
                                // If the author is not in the list, add it with an initial count
                                authors.Add(author);
                                authorCounts.Add(1);
                            }
                            else
                            {
                                // If the author is already in the list, increment their count
                                authorCounts[index]++;
                            }
                        }
                    }

                    // Find the author with the highest borrow count
                    string bestAuthor = null;
                    int maxAuthorCount = 0;

                    for (int i = 0; i < authorCounts.Count; i++)
                    {
                        if (authorCounts[i] > maxAuthorCount)
                        {
                            bestAuthor = authors[i];
                            maxAuthorCount = authorCounts[i];
                        }
                    }

                    // Output the name of the best author
                    Console.WriteLine($"Best Author: {bestAuthor}");



                    // List to keep track of book IDs borrowed along with the most borrowed book
                    List<int> borrowedWithMostBorrowedBook = new List<int>();

                    // Find the book IDs borrowed by the same users who borrowed the most borrowed book
                    foreach (var borrowing in Borrowings)
                    {
                        if (borrowing.bid == mostBorrowedBookId && !borrowing.returnBook)
                        {
                            foreach (var otherBorrowing in Borrowings)
                            {
                                if (otherBorrowing.uid == borrowing.uid && otherBorrowing.bid != mostBorrowedBookId && !otherBorrowing.returnBook)
                                {
                                    if (!borrowedWithMostBorrowedBook.Contains(otherBorrowing.bid))
                                    {
                                        borrowedWithMostBorrowedBook.Add(otherBorrowing.bid);
                                    }
                                }
                            }
                        }
                    }

                    // Output the IDs of the books borrowed with the most borrowed book
                    Console.WriteLine("Books borrowed with the most borrowed book:");
                    foreach (var bookId in borrowedWithMostBorrowedBook)
                    {
                        string bookn = null;
                        foreach (var book in Books)
                        {
                            if (bookId == book.ID)
                                bookn = book.BName;
                        }
                        Console.WriteLine(bookId + ": " + bookn);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while suggesting books: " + ex.Message);
                }
            }
            else
            {// Check if the borrowed book exists
                var borrowedBook = Books.FirstOrDefault(b => b.ID == BookID);
                if (borrowedBook == default)
                {
                    Console.WriteLine("Book not found for suggestions.");
                    return;
                }

                Console.WriteLine("\nBook suggestions for you based on your recent borrowing:");

                // Suggest books by the same author
                Console.WriteLine("\nBooks by the same author:");
                var booksBySameAuthor = Books.Where(b => b.BAuthor == borrowedBook.BAuthor && b.ID != BookID).ToList();
                if (booksBySameAuthor.Count > 0)
                {
                    foreach (var book in booksBySameAuthor)
                    {
                        Console.WriteLine($"{book.BName} by {book.BAuthor}");
                    }
                }
                else
                {
                    Console.WriteLine("No other books by the same author.");
                }

                // Suggest books that were borrowed along with the current book by other users
                Console.WriteLine("\nBooks commonly borrowed with this book:");
                List<int> borrowedWithSameBook = new List<int>();
                foreach (var borrowing in Borrowings)
                {
                    if (borrowing.bid == BookID && borrowing.returnBook == false)
                    {
                        foreach (var otherBorrowing in Borrowings)
                        {
                            if (otherBorrowing.uid == borrowing.uid && otherBorrowing.bid != BookID && !otherBorrowing.returnBook)
                            {
                                if (!borrowedWithSameBook.Contains(otherBorrowing.bid))
                                {
                                    borrowedWithSameBook.Add(otherBorrowing.bid);
                                }
                            }
                        }
                    }
                }

                if (borrowedWithSameBook.Count > 0)
                {
                    foreach (var bookId in borrowedWithSameBook)
                    {
                        var book = Books.FirstOrDefault(b => b.ID == bookId);
                        if (book != default)
                        {
                            Console.WriteLine($"{book.BName} by {book.BAuthor}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No other books borrowed with this one.");
                }

                // Suggest the most borrowed books
                Console.WriteLine("\nPopular books among other users:");
                Dictionary<int, int> borrowCount = new Dictionary<int, int>();
                foreach (var borrowing in Borrowings)
                {
                    if (borrowCount.ContainsKey(borrowing.bid))
                    {
                        borrowCount[borrowing.bid]++;
                    }
                    else
                    {
                        borrowCount[borrowing.bid] = 1;
                    }
                }

                var popularBooks = borrowCount.OrderByDescending(b => b.Value).Take(3).ToList(); // Suggest top 3 most borrowed books
                foreach (var bookEntry in popularBooks)
                {
                    var book = Books.FirstOrDefault(b => b.ID == bookEntry.Key);
                    if (book != default)
                    {
                        Console.WriteLine($"{book.BName} by {book.BAuthor}");
                    }
                }
            }
        }
        

        //........................helper Functions.....................................//
        static void LoadAllFiles()
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
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
            //-----------
            try
            {
                if (File.Exists(userFilePath))
                {
                    using (StreamReader reader = new StreamReader(userFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        { //int id,string email,string pw,string name
                            var parts = line.Split('|');
                            if (parts.Length == 4)
                            {
                                Users.Add((int.Parse(parts[0]),parts[1], parts[2], parts[3]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
            //-------------------

            try
            {
                if (File.Exists(borrowingFilePath))
                {
                    using (StreamReader reader = new StreamReader(borrowingFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 4)
                            {
                                int uid = int.Parse(parts[0]);
                                int bid = int.Parse(parts[1]);
                                DateTime date = DateTime.Parse(parts[2]); // Parse the date string
                                bool returnBook = bool.Parse(parts[3]); // Parse the returnBook string

                                Borrowings.Add((uid, bid, date, returnBook));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
            //---------------------
            try
            {
                if (File.Exists(returningFilePath))
                {
                    using (StreamReader reader = new StreamReader(returningFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 3)
                            {
                                int uid = int.Parse(parts[0]);
                                int bid = int.Parse(parts[1]);
                                DateTime date = DateTime.Parse(parts[2]); // Parse the date

                                returnings.Add((uid, bid, date));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }

            //---------------
            try
            {
                if (File.Exists(adminFilePath))
                {
                    using (StreamReader reader = new StreamReader(adminFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        { //string email, string pw, string name
                            var parts = line.Split('|');
                            if (parts.Length == 3)
                            {
                                Admins.Add((parts[0], parts[1], parts[2]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }

            Console.WriteLine("all loaded from files successfully...!");

        }
        static void SaveBooksToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var book in Books)
                    {
                        writer.WriteLine($"{book.BName}|{book.BAuthor}|{book.ID}|{book.originalQuantity}|{book.quantity}");
                    }
                }
                Console.WriteLine("Books updated to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }
        static void saveAllUsers()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(userFilePath))
                {
                    foreach (var user in Users)
                    {
                        writer.WriteLine($"{user.id}|{user.email}|{user.pw}|{user.name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(adminFilePath))
                {
                    foreach (var admin in Admins)
                    {
                        writer.WriteLine($"{admin.email}|{admin.pw}|{admin.name}");
                    }
                }
                Console.WriteLine("All users data saved to file successfully!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }
        static void ViewAllBooks()
        {
            StringBuilder sb = new StringBuilder();

            int BookNumber = 0;
            sb.Append("\n \t---All Books in Library ---"); 
            sb.Append("\n \tname\tAuthor\tID\tQuantity");
            for (int i = 0; i < Books.Count; i++)
            {
                BookNumber = i + 1;
                sb.AppendLine($"({BookNumber})\tBook Name:{Books[i].BName}\tAuthor:{Books[i].BAuthor}\tID:{Books[i].BAuthor}\tQuantity:{Books[i].quantity}");
                sb.AppendLine();
                Console.WriteLine(sb.ToString());
                sb.Clear();

            }
        }
        static void SearchForBook()
        {
            ViewAllBooks();
            Console.WriteLine("Enter the book name you want");
            string name = Console.ReadLine();

            // Check if the name is valid
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Error: Book name cannot be empty or just spaces.");
                return;
            }

            bool found = false;
            try
            {
                string searchNameLower = name.ToLower();
                for (int i = 0; i < Books.Count; i++)
                {
                    if (Books[i].BName.ToLower() == searchNameLower)
                    {
                        Console.WriteLine("Book Author is : " + Books[i].BAuthor);
                        found = true;
                        index = i;
                        break;

                    }
                }

                if (!found)
                {
                    Console.WriteLine("book not found !");
                    index = -1;
                }
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors
                Console.WriteLine("An error occurred while searching for the book: " + ex.Message);
            }
        }
        static void saveAllActions() 
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(borrowingFilePath))
                {
                    foreach (var b in Borrowings)
                    {
                        writer.WriteLine($"{b.uid}|{b.bid}|{b.date}|{b.returnBook}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(returningFilePath))
                {
                    foreach (var r in returnings)
                    {
                        writer.WriteLine($"{r.uid}|{r.bid}|{r.date}");
                    }
                }
                Console.WriteLine($"all your actions, save in file successfully!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }
        

    }
}
