using Microsoft.VisualBasic;
using System;
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
        static List<(int uid, int bid, DateTime date)> Borrowings = new List<(int uid, int bid, DateTime date)>();
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

                    case "2":
                        Console.Clear();
                        // string user2 = "Admin";
                        // LoginAccess(user2);
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

        //........................Admin Functions.....................................//
        static void adminMenu(string name)
        {
            //{
            //     accountsManagement();
            //  }
            //  else
            //  {
            if (name != "registrar") { 
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
                Console.WriteLine("\n 6 .singOut");

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
        }
          
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
                Console.WriteLine("\n 4 .singOut");

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
        }
        static void BorrowBook(int userId) {
            try
            {
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
                            Borrowings.Add((userId, Books[index].ID, DateTime.Now));
                            Console.WriteLine("You have borrowed the " + Books[index].BName + "!");


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
                SearchForBook();
                if (index != -1)
                {
                    int quantity = Books[index].quantity;
                    int originalQuantity = Books[index].originalQuantity;

                    // Check if the user has borrowed this book
                    var borrowingRecord = Borrowings.FirstOrDefault(b => b.uid == userId && b.bid == Books[index].ID);

                    if (borrowingRecord == default)
                    {
                        Console.WriteLine("Error: You have not borrowed this book.");
                        return;
                    }

                    Console.WriteLine("Do you want to return the Book?");
                    Console.WriteLine("\n press char ' y ' to borrow :");
                    string selected = Console.ReadLine();

                    if (selected == "y")
                    {
                        // Check if the current quantity equals the original quantity
                        if (quantity >= originalQuantity)
                        {
                            Console.WriteLine("Error: Cannot return the book. All copies have already been returned.");
                        }
                        else
                        {
                            ++quantity;
                            Books[index] = (Books[index].BName, Books[index].BAuthor, Books[index].ID, Books[index].originalQuantity, quantity);
                            returnings.Add((userId, Books[index].ID, DateTime.Now));
                            Borrowings.Remove(borrowingRecord);

                            Console.WriteLine("'" + Books[index].BName + "' Book has been returned successfully!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sorry! Cannot return this " + Books[index].BName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while returning the book: " + ex.Message);
            }
        }

        //........................Necessary Functions.....................................//
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
                Console.WriteLine("Books saved to file successfully.");
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

    }
}
