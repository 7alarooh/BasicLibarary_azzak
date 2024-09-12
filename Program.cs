using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;


namespace BasicLibrary
{
    internal class Program
    {// TEST 
     //........................Necessary variables and path files.....................................//
        static List<(int AID, string AName, string Email, string Password)> Admins = new List<(int AID, string AName, string Email, string Password)>();
        
        static List<(int UID, string Uname, string Email, string Password)> Users =new List<(int UID, string Uname, string Email, string Password)>();
       
        static List<(int BID,string BName, string BAuthor, int copies, int borrowedCopies, double Price, string catagory, int BorrowPeriod)> Books = new List<(int BID,string BName, string BAuthor, int copies, int borrowedCopies, double Price, string catagory, int BorrowPeriod)>();

        static List<(int uid, int bid, DateTime date,DateTime ReturnDate, DateTime? ActualReturnDate, int? Rating, bool ISReturned)> Borrowings = new List<(int uid, int bid, DateTime date, DateTime ReturnDate, DateTime? ActualReturnDate, int? Rating ,bool ISReturned)>();
        static List<(int CID, string CName, int NOFBooks)> Categories = new List<(int CID, string CName, int NOFBooks)>();
        static List<(int PurchaseID, int UID, int BID, DateTime PurchaseDate, double Price)> Purchases = new List<(int PurchaseID, int UID, int BID, DateTime PurchaseDate, double Price)>();
       

        static string filePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\BooksFile.txt";
        static string userFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\UsersFile.txt";
        static string adminFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\AdminsFile.txt";
        static string borrowingFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\BorrowingFile.txt";
        static string CategoriesFile = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\CategoriesFile.txt";
        static string AlertsFile = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\AlertsFile.txt";
        static string purchasesFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\purchasesFilePath.txt";
        static string reservationFilePath = "C:\\Users\\Lenovo\\source\\repos\\azzaGitTest\\ReservationsFile.txt";


        static int index = -1;
        static int purchaseIdCounter = 1; // To track unique purchase IDs
        //-----------------------------------------------------------------------------//

        static void Main(string[] args)
        {
            
            LoadAllFiles();
            loginPage();

        }

        //........................Functions Part.......................................//

        //........................Login Functions.....................................//
        static void loginPage()
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
                    // Cases for user registration and admin login
                    case "2":
                        Console.Clear();
                        Console.Write("Enter Your Email: ");
                        string uEmail = Console.ReadLine();

                        // Validate email format
                        if (!IsValidEmail(uEmail))
                        {
                            Console.WriteLine("Error: Invalid email format.");
                            break;
                        }

                        var user = Users.FirstOrDefault(u => u.Email.Equals(uEmail, StringComparison.OrdinalIgnoreCase));
                        if (user != default)
                        {
                            bool incorrectPassword = true;
                            while (incorrectPassword)
                            {
                                Console.Write("\nEnter Password: ");
                                string enterPW = Console.ReadLine();

                                // Validate password
                                string passwordValidationResult = IsValidPassword(enterPW);

                                if (passwordValidationResult.StartsWith("Error"))
                                {
                                    Console.WriteLine(passwordValidationResult);
                                    break;
                                }

                                if (enterPW == user.Password)
                                {
                                    userMenu(user.UID, user.Uname); // Access user menu on correct password
                                    incorrectPassword = false;
                                }
                                else
                                {
                                    Console.WriteLine("Error: Incorrect password.");
                                    Console.WriteLine("1. Try again");
                                    Console.WriteLine("2. Forgot password?");
                                    string retryChoice = Console.ReadLine();

                                    if (retryChoice == "2")
                                    {
                                        LogForgotPassword(user.Uname, user.Email);
                                        Console.WriteLine("We have logged your request for a forgotten password.");
                                        incorrectPassword = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: User not found.");
                        }
                        break;

                    case "3":
                        SaveBooksToFile();
                        Console.WriteLine("\nPress Enter key to exit the system");
                        Console.ReadLine();
                        ExitFlag = true; // Exit the loop
                        break;

                    default:
                        Console.WriteLine("Sorry, your choice was wrong!!");
                        break;
                }

                if (!ExitFlag)
                {
                    Console.WriteLine("Press any key to continue to the Home Page");
                    Console.ReadLine();
                    Console.Clear();
                }

            } while (!ExitFlag);
        }

        // Log forgot password request to an external file
        static void LogForgotPassword(string username, string email)
        {
            
            string logEntry = $"{username}|{email}|{DateTime.Now}|Note: Forgot Password\n";

            try
            {
                File.AppendAllText(AlertsFile, logEntry);
                Console.WriteLine("Forgot password request logged successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error logging forgot password request: " + ex.Message);
            }
        }

        // Function to validate email format
        static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }

        // Function to validate password format
        static string IsValidPassword(string password)
        {
            if (password.Length < 8)
            {
                return "Error: Password must be at least 8 characters long.";
            }

            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));

            if (hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar)
            {
                return "Strong password.";
            }
            else if ((hasUpperCase && hasLowerCase && hasDigit) || (hasLowerCase && hasDigit && hasSpecialChar))
            {
                return "Medium password.";
            }
            else
            {
                return "Weak password.";
            }
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
                        Console.WriteLine("\npress Enter key to exit out system");
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
            int nextID = 1; // Start with 1 if there are no users
            if (Users.Count > 0)
            {
                // Loop through the users to find the highest existing ID
                foreach (var user in Users)
                {
                    if (user.UID >= nextID)
                    {
                        nextID = user.UID + 1;
                    }
                }
            }

            string email;
            bool emailExists = true;

            // Loop until a valid and unique email is entered
            do
            {
                Console.WriteLine("Enter User Email:");
                email = Console.ReadLine();

                // Check if email format is valid
                if (!IsValidEmail(email))
                {
                    Console.WriteLine("Error: Invalid email format. Please try again.");
                    continue; // Go back to the beginning of the loop if email is invalid
                }

                // Check if email already exists
                if (Users.Any(u => u.Email == email)) // Case-insensitive check
                {
                    Console.WriteLine("Error: A user with this email already exists. Please enter a different email.");
                }
                else
                {
                    emailExists = false; // Email is unique, break the loop
                }
            } while (emailExists); // Continue looping until a valid and unique email is provided

            string password, confirmPassword;
            bool passwordsMatch = false;

            // Loop until passwords match and meet the validation criteria
            do
            {
                Console.WriteLine("Enter User Password (at least 6 characters):");
                password = Console.ReadLine();

                // Check if password is valid
                // Validate password format
                string passwordValidationResult = IsValidPassword(password);

                if (passwordValidationResult.StartsWith("Error"))
                {
                    Console.WriteLine(passwordValidationResult); // Show error message for invalid password

                    continue; // Return to the menu instead of exiting.
                }
                Console.WriteLine("This Password is " + IsValidPassword(password));

                Console.WriteLine("Confirm Password:");
                confirmPassword = Console.ReadLine();

                // Check if passwords match
                if (password != confirmPassword)
                {
                    Console.WriteLine("Error: Passwords do not match. Please try again.");
                }
                else
                {
                    passwordsMatch = true; // Passwords match, break the loop
                }
            } while (!passwordsMatch); // Continue looping until passwords match and are valid

            Console.WriteLine("Enter User Name:");
            string name = Console.ReadLine();

            // Add new user to the Users list with the auto-generated ID
            Users.Add((nextID, name ,email, password));
            Console.WriteLine($"New user added successfully with ID: {nextID}");
        }
        static void AddAdmin()
        {// Auto-generate unique admin ID
            int nextID = 1; // Default ID is 1 if no admins exist
            if (Admins.Count > 0)
            {
                nextID = Admins.Max(a => a.AID) + 1; // Find the highest ID and increment it
            }

            string email;
            bool emailExists = true;

            // Loop until a valid and unique email is entered
            do
            {
                Console.WriteLine("Enter Admin Email:");
                email = Console.ReadLine();

                // Check if email format is valid
                if (!IsValidEmail(email))
                {
                    Console.WriteLine("Error: Invalid email format. Please try again.");
                    continue; // Go back to the beginning of the loop if email is invalid
                }

                // Check if email already exists
                if (Admins.Any(a => a.Email.ToLower() == email.ToLower())) // Case-insensitive check
                {
                    Console.WriteLine("Error: An admin with this email already exists. Please enter a different email.");
                }
                else
                {
                    emailExists = false; // Email is unique, break the loop
                }
            } while (emailExists); // Continue looping until a valid and unique email is provided

            string password, confirmPassword;
            bool passwordsMatch = false;

            // Loop until passwords match and meet the validation criteria
            do
            {
                Console.WriteLine("Enter Admin Password (at least 6 characters):");
                password = Console.ReadLine();

                // Check if password is valid
                // Validate password format
                string passwordValidationResult = IsValidPassword(password);

                if (passwordValidationResult.StartsWith("Error"))
                {
                    Console.WriteLine(passwordValidationResult); // Show error message for invalid password
                    continue; // Return to the menu instead of exiting.
                }
                Console.WriteLine("This Password is " + IsValidPassword(password));


                Console.WriteLine("Confirm Password:");
                confirmPassword = Console.ReadLine();

                // Check if passwords match
                if (password != confirmPassword)
                {
                    Console.WriteLine("Error: Passwords do not match. Please try again.");
                }
                else
                {
                    passwordsMatch = true; // Passwords match, break the loop
                }
            } while (!passwordsMatch); // Continue looping until passwords match and are valid

            Console.WriteLine("Enter Admin Name:");
            string name = Console.ReadLine();

            // Add admin to the list with the auto-generated unique ID
            Admins.Add((nextID, name, email, password));
            Console.WriteLine($"Admin added successfully with ID: {nextID}");
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

            var user = Users.FirstOrDefault(u => u.UID == userId);

            if (user.UID == 0)
            {
                Console.WriteLine("Error: User not found.");
                return;
            }

            Console.WriteLine($"Current details for User ID {userId}:");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Name: {user.Uname}");

            Console.WriteLine("Enter new email (leave blank to keep current):");
            string newEmail = Console.ReadLine();
            Console.WriteLine("Enter new password (leave blank to keep current):");
            string newPassword = Console.ReadLine();
            Console.WriteLine("Enter new name (leave blank to keep current):");
            string newName = Console.ReadLine();

            // Confirmation
            Console.WriteLine("Confirm the changes:");
            Console.WriteLine($"New Email: {(string.IsNullOrWhiteSpace(newEmail) ? user.Email : newEmail)}");
            Console.WriteLine($"New Password: {(string.IsNullOrWhiteSpace(newPassword) ? "Not changed" : newPassword)}");
            Console.WriteLine($"New Name: {(string.IsNullOrWhiteSpace(newName) ? user.Uname : newName)}");
            Console.WriteLine("Are you sure you want to apply these changes? (y/n)");
            string confirmation = Console.ReadLine();

            if (confirmation.ToLower() == "y")
            {
                // Update the user information if confirmed
                Users = Users.Select(u =>
                    u.UID == userId ?
                    (u.UID,
                    string.IsNullOrWhiteSpace(newName) ? u.Uname : newName,
                     string.IsNullOrWhiteSpace(newEmail) ? u.Email : newEmail,
                     string.IsNullOrWhiteSpace(newPassword) ? u.Password : newPassword
                     )
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

            var admin = Admins.FirstOrDefault(a => a.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (admin == default)
            {
                Console.WriteLine("Error: Admin not found.");
                return;
            }

            Console.WriteLine($"Current details for Admin Email {email}:");
            Console.WriteLine($"Name: {admin.AName}");

            Console.WriteLine("Enter new password (leave blank to keep current):");
            string newPassword = Console.ReadLine();
            Console.WriteLine("Enter new name (leave blank to keep current):");
            string newName = Console.ReadLine();

            // Confirmation
            Console.WriteLine("Confirm the changes:");
            Console.WriteLine($"New Password: {(string.IsNullOrWhiteSpace(newPassword) ? "Not changed" : newPassword)}");
            Console.WriteLine($"New Name: {(string.IsNullOrWhiteSpace(newName) ? admin.AName : newName)}");
            Console.WriteLine("Are you sure you want to apply these changes? (y/n)");
            string confirmation = Console.ReadLine();

            if (confirmation.ToLower() == "y")
            {
                // Update the admin information if confirmed
                Admins = Admins.Select(a =>
                a.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                ? (a.AID, string.IsNullOrWhiteSpace(newName) ? a.AName : newName,
                a.Email, string.IsNullOrWhiteSpace(newPassword) ? a.Password : newPassword)
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

                var user = Users.FirstOrDefault(u => u.UID == userId);

                if (user.UID == 0)
                {
                    Console.WriteLine("Error: User not found.");
                    return;
                }

                Console.WriteLine($"Confirm removal of User ID {userId}:");
                Console.WriteLine($"Name: {user.Uname}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine("Are you sure you want to remove this user? (y/n)");
                string confirmation = Console.ReadLine();

                if (confirmation.ToLower() == "y")
                {
                    Users = Users.Where(u => u.UID != userId).ToList();
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

                var admin = Admins.FirstOrDefault(a => a.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (admin == default)
                {
                    Console.WriteLine("Error: Admin not found.");
                    return;
                }

                Console.WriteLine($"Confirm removal of Admin with Email {email}:");
                Console.WriteLine($"ID: {admin.AID}");
                Console.WriteLine($"Name: {admin.AName}");
                Console.WriteLine("Are you sure you want to remove this admin? (y/n)");
                string confirmation = Console.ReadLine();

                if (confirmation.ToLower() == "y")
                {
                    Admins = Admins.Where(a => !a.Email.Equals(email, StringComparison.OrdinalIgnoreCase)).ToList();
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

            // Define column widths
            int idWidth = 10;
            int nameWidth = 25;
            int emailWidth = 30;
            int passwordWidth = 20;

            // Print Admins
            sb.AppendLine("\n \t--- All Users in Library System ---");
            sb.AppendLine("\n \t--- Admins ---");
            sb.AppendFormat("\t{0,-" + idWidth + "} {1,-" + nameWidth + "} {2,-" + emailWidth + "} {3,-" + passwordWidth + "}",
                            "ID", "Name", "Email", "Password");
            sb.AppendLine();
            sb.AppendLine(new string('-', idWidth + nameWidth + emailWidth + passwordWidth + 12)); // 12 for padding and " \t"

            foreach (var admin in Admins)
            {
                sb.AppendFormat("\t{0,-" + idWidth + "} {1,-" + nameWidth + "} {2,-" + emailWidth + "} {3,-" + passwordWidth + "}",
                                admin.AID,
                                admin.AName,
                                admin.Email,
                                admin.Password);
                sb.AppendLine();
            }

            // Print Users
            sb.AppendLine("\n\n \t--- Users ---");
            sb.AppendFormat("\t{0,-" + idWidth + "} {1,-" + nameWidth + "} {2,-" + emailWidth + "} {3,-" + passwordWidth + "}",
                            "ID", "Name", "Email", "Password");
            sb.AppendLine();
            sb.AppendLine(new string('-', idWidth + nameWidth + emailWidth + passwordWidth + 12)); // 12 for padding and " \t"

            foreach (var user in Users)
            {
                sb.AppendFormat("\t{0,-" + idWidth + "} {1,-" + nameWidth + "} {2,-" + emailWidth + "} {3,-" + passwordWidth + "}",
                                user.UID,
                                user.Uname,
                                user.Email,
                                user.Password);
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
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
                Console.WriteLine("\n 7 .Alerts File");
                Console.WriteLine("\n 8 .singOut");

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
                        alertsFile();
                        break;
                    case "8":
                        SaveBooksToFile();
                        Console.WriteLine("\npress Enter key to exit out system");
                        string outsystem = Console.ReadLine();
                        ExitFlag = true;
                        break;
                        
                    default:
                        Console.WriteLine("Sorry your choice was wrong !!");
                        break;

                }

                Console.WriteLine("press Enter key to continue");
                string cont = Console.ReadLine();

                Console.Clear();


            } while (ExitFlag != true);
       // }
          
        }
        static void AddNewBook()
        {
            string name;
            string author;
            int copies = 0;
            double price = 0;
            string category;
            int DaysAllowedForBorrowing = 0;

            Console.WriteLine("Enter Book Name:");
            name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) // to handle the book name
            {
                Console.WriteLine("Error: Book name cannot be empty.");
                return;
            }

            Console.WriteLine("Enter Book Author:");
            author = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(author)) // to handle the book author
            {
                Console.WriteLine("Error: Author name cannot be empty.");
                return;
            }

            // Check if a book with the same name already exists
            var existingBook = Books.FirstOrDefault(b => b.BName.ToLower() == name.ToLower());
            if (existingBook != default) // If a book with the same name exists
            {
                if (existingBook.BAuthor.ToLower() == author.ToLower()) // Check if the author is the same
                {
                    Console.WriteLine("Error: A book with the same name and author already exists.");
                    return;
                }
                else
                {
                    Console.WriteLine("A book with the same name exists, but by a different author. Proceeding to add the book.");
                }
            }

            // Manually generate the next book ID by checking the IDs in the Books list
            int newID = Books.Count > 0 ? Books.Max(b => b.BID) + 1 : 1;

            // Handle the book quantity
            Console.WriteLine("Enter the Book Quantity:");
            try
            {
                copies = int.Parse(Console.ReadLine());
                if (copies < 0)
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

            // Handle the book price
            Console.WriteLine("Enter the Book Price:");
            try
            {
                price = double.Parse(Console.ReadLine());
                if (price < 0)
                {
                    Console.WriteLine("Error: Price cannot be negative.");
                    return;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Please enter a valid number for the Book Price.");
                return;
            }
            catch (OverflowException)
            {
                Console.WriteLine("Error: The number for the Book Price is too large.");
                return;
            }

            // Show all categories and let the user select one
            if (Categories.Count == 0)
            {
                Console.WriteLine("Error: No categories available. Please add categories before adding a book.");
                return;
            }

            Console.WriteLine("Select Book Category:");
            for (int i = 0; i < Categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Categories[i].CName}");
            }
            Console.WriteLine("Enter No. Categories:");
            int categoryIndex = 0;
            try
            {
                categoryIndex = int.Parse(Console.ReadLine()) - 1;
                if (categoryIndex < 0 || categoryIndex >= Categories.Count)
                {
                    Console.WriteLine("Error: Invalid category selection.");
                    return;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Please enter a valid number for the category selection.");
                return;
            }
            catch (OverflowException)
            {
                Console.WriteLine("Error: The number for the category selection is too large.");
                return;
            }

            category = Categories[categoryIndex].CName;

            // Handle the borrowing period
            Console.WriteLine("Enter the Number of Days Allowed for Borrowing:");
            try
            {
                DaysAllowedForBorrowing = int.Parse(Console.ReadLine());
                if (DaysAllowedForBorrowing <= 0)
                {
                    Console.WriteLine("Error: Borrowing period must be positive.");
                    return;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Please enter a valid number for the borrowing period.");
                return;
            }
            catch (OverflowException)
            {
                Console.WriteLine("Error: The number for the borrowing period is too large.");
                return;
            }

            // Add the book if everything is valid
            Books.Add((newID, name, author, copies, 0, price, category, DaysAllowedForBorrowing));
            Console.WriteLine($"Book added successfully with ID: {newID}!");

            // Update the category's book count
            Categories[categoryIndex] = (Categories[categoryIndex].CID, Categories[categoryIndex].CName, Categories[categoryIndex].NOFBooks + 1);
            Console.WriteLine($"Category '{category}' now has {Categories[categoryIndex].NOFBooks} books.");
        }
        static void ViewAllBooks()
        {
            StringBuilder sb = new StringBuilder();

            // Define column widths
            int nameWidth = 30;
            int authorWidth = 30;
            int idWidth = 5;
            int copiesWidth = 16;
            int priceWidth = 10;
            int categoryWidth = 15;
            int periodWidth = 10;

            sb.AppendLine("\n \t--- All Books in Library ---");

            // Use interpolation to format headers
            sb.AppendFormat("\t{0,-" + nameWidth + "} {1,-" + authorWidth + "} {2,-" + idWidth + "} {3,-" + copiesWidth + "}  {4,-" + priceWidth + "} {5,-" + categoryWidth + "} {6,-" + periodWidth + "}",
                            "Name", "Author", "ID", "Available Copies", "Price", "Category", "Borrow Period");
            sb.AppendLine();
            sb.AppendLine(new string('-', nameWidth + authorWidth + idWidth + copiesWidth + priceWidth + categoryWidth + periodWidth + 24)); // 24 for padding

            for (int i = 0; i < Books.Count; i++)
            {
                var book = Books[i];
                sb.AppendFormat("\t{0,-" + nameWidth + "} {1,-" + authorWidth + "} {2,-" + idWidth + "} {3,-" + copiesWidth + "}  {4,-" + priceWidth + "} {5,-" + categoryWidth + "} {6,-" + periodWidth + "}",
                                book.BName,
                                book.BAuthor,
                                book.BID,
                                book.copies - book.borrowedCopies,
                                book.Price.ToString("0.00") + " OMR", // Format as OMR currency
                                book.catagory,
                                book.BorrowPeriod);
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
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

            // Assuming `index` is set from the search function
            if (index == -1)
            {
                Console.WriteLine("Error: Book not found.");
                return;
            }

            var book = Books[index];

            Console.WriteLine($"Editing Book: {book.BName} by {book.BAuthor}");

            // Edit Book Name
            Console.WriteLine("Enter new Book Name (or press Enter to skip):");
            string newName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newName))
            {
                // Check for duplicate book name
                var duplicateBook = Books.FirstOrDefault(b => b.BName.ToLower() == newName.ToLower() && b.BID != book.BID);
                if (duplicateBook != default)
                {
                    Console.WriteLine("Error: A book with the same name already exists.");
                    return;
                }
                book.BName = newName;
            }

            // Edit Book Author
            Console.WriteLine("Enter new Book Author (or press Enter to skip):");
            string newAuthor = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newAuthor))
            {
                book.BAuthor = newAuthor;
            }

            // Edit Book Copies
            Console.WriteLine("Enter new Book Quantity (or press Enter to skip):");
            string quantityInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(quantityInput))
            {
                try
                {
                    int newQuantity = int.Parse(quantityInput);
                    if (newQuantity < book.borrowedCopies)  // Ensure the new quantity is not less than borrowed copies
                    {
                        Console.WriteLine($"Error: Cannot decrease the total copies below the number of currently borrowed copies ({book.borrowedCopies}).");
                        return;
                    }
                    book.copies = newQuantity;
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
                var activeBorrowing = Borrowings.Any(b => b.bid == Books[index].BID && !b.ISReturned);//the function checks the Borrowings list to see if any active borrowing exists for the book >> the book is borrowed and the returnBook flag is still false.

                if (activeBorrowing)
                {
                    Console.WriteLine($"Error: The book '{Books[index].BName}' has been borrowed and is not yet returned. It cannot be removed.");
                    return;
                }
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
        static void SearchForBook()
        {
            ViewAllBooks(); // Display all books before search

            Console.WriteLine("\nEnter the book name or part of the book name you want to search for:");
            string searchName = Console.ReadLine();

            // Check if the name is valid
            if (string.IsNullOrWhiteSpace(searchName))
            {
                Console.WriteLine("Error: Book name cannot be empty or just spaces.");
                return;
            }

            bool found = false; // Track whether any books were found
            try
            {
                string searchNameLower = searchName.ToLower();

                // Create a header for the table with fixed-width columns
                Console.WriteLine("+-----------------------------------------------------------------------------------------------+");
                Console.WriteLine("| ID  | Book Name                             | Author        | Available Copies | Price (OMR) | Category   |");
                Console.WriteLine("+-----------------------------------------------------------------------------------------------+");

                // Iterate through the list of books and find those that contain the search word
                for (int i = 0; i < Books.Count; i++)
                {
                    if (Books[i].BName.ToLower().Contains(searchNameLower))
                    {
                        // Display full details of the matching book in table format with consistent column widths
                        Console.WriteLine($"| {Books[i].BID,-3} | {Books[i].BName,-36} | {Books[i].BAuthor,-12} | {(Books[i].copies - Books[i].borrowedCopies),-17} | {Books[i].Price,-10:F2} | {Books[i].catagory,-10} |");
                        found = true;
                    }
                }

                Console.WriteLine("+-----------------------------------------------------------------------------------------------+");

                if (!found)
                {
                    Console.WriteLine("\nNo books found with the given name or keyword.");
                }
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors
                Console.WriteLine("An error occurred while searching for the book: " + ex.Message);
            }
        }
        static void reporting()
        {
            Console.Clear();
            Console.WriteLine("\n------\tToday's Report\t-----");

            // Total number of books in the library
            int totalBooksInLibrary = Books.Count;
            Console.WriteLine($"Total Number of Books in the Library: {totalBooksInLibrary}");

            // Total number of categories and book count per category
            Console.WriteLine($"\nTotal Number of Categories: {Categories.Count}");
            Console.WriteLine("Categories:");
            foreach (var category in Categories)
            {
                Console.WriteLine($"- {category.CName}: {category.NOFBooks} books");
            }

            // Total number of copies of all books
            int totalBookCopies = Books.Sum(b => b.copies);
            Console.WriteLine($"\nTotal Number of Book Copies (All Books Combined): {totalBookCopies}");

            // Total number of borrowed books (sum of all borrowed copies)
            int totalBorrowedBooks = Books.Sum(b => b.borrowedCopies);
            Console.WriteLine($"Total Number of Borrowed Books: {totalBorrowedBooks}");

            // Total number of returned books (count of Borrowings where ISReturned is true)
            int totalReturnedBooks = Borrowings.Count(b => b.ISReturned);
            Console.WriteLine($"Total Number of Returned Books: {totalReturnedBooks}");

            // List of tuples to track how many times each book has been borrowed: (bookId, count)
            List<(int bookId, int count)> bookBorrowCount = new List<(int bookId, int count)>();

            // List of tuples to track how many times each author has been borrowed: (authorName, count)
            List<(string authorName, int count)> authorBorrowCount = new List<(string authorName, int count)>();

            // List of tuples to track how many times each user has borrowed: (userId, count)
            List<(int userId, int count)> userBorrowCount = new List<(int userId, int count)>();

            foreach (var borrowing in Borrowings)
            {
                int bookId = borrowing.bid;
                int userId = borrowing.uid;

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

                // Update user borrow count
                bool userFound = false;
                for (int i = 0; i < userBorrowCount.Count; i++)
                {
                    if (userBorrowCount[i].userId == userId)
                    {
                        userBorrowCount[i] = (userBorrowCount[i].userId, userBorrowCount[i].count + 1);
                        userFound = true;
                        break;
                    }
                }
                if (!userFound)
                {
                    userBorrowCount.Add((userId, 1));
                }

                // Find the book to get the author
                var book = Books.FirstOrDefault(b => b.BID == bookId);
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

                var book = Books.FirstOrDefault(b => b.BID == mostBorrowedBook.bookId);
                Console.WriteLine($"\nMost Borrowed Book: '{book.BName}' by {book.BAuthor} (Borrowed {maxBorrowCount} times)");
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

            // Find the top 3 users who borrowed the most books
            if (userBorrowCount.Count > 0)
            {
                Console.WriteLine("\nTop 3 Users Who Borrowed the Most Books:");
                var topUsers = userBorrowCount.OrderByDescending(u => u.count).Take(3).ToList();
                foreach (var topUser in topUsers)
                {
                    var user = Users.FirstOrDefault(u => u.UID == topUser.userId);
                    Console.WriteLine($"- {user.Uname}: Borrowed {topUser.count} books");
                }
            }
            else
            {
                Console.WriteLine("No users have borrowed any books yet.");
            }

            Console.WriteLine("\n------\tEnd of Report\t-----");
        }

        static void alertsFile()
        {
            try
            {
                if (File.Exists(AlertsFile))
                {
                    var lines = File.ReadAllLines(AlertsFile);

                    // Header for the table
                    Console.WriteLine("+-----+----------------------+----------------------------+-------------------+");
                    Console.WriteLine("| ID  | Username              | Email                      | Message           |");
                    Console.WriteLine("+-----+----------------------+----------------------------+-------------------+");

                    int id = 1; // ID counter for the records
                    foreach (var line in lines)
                    {
                        // Split the line by '|' and extract data
                        var data = line.Split('|');
                        if (data.Length >= 3)
                        {
                            string username = data[0].Trim();
                            string email = data[1].Trim();
                            string message = data[3].Trim();

                            // Display data in a structured table format
                            Console.WriteLine($"| {id,-3} | {username,-20} | {email,-26} | {message,-17} |");
                            id++;
                        }
                    }

                    // Closing the table
                    Console.WriteLine("+-----+----------------------+----------------------------+-------------------+");
                }
                else
                {
                    Console.WriteLine("Alerts file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading alerts file: " + ex.Message);
            }
        }



        //........................User Functions.....................................//
        static void userMenu(int id,string name)
        {
            CheckReservations(); // Check for available reservations before showing the menu
            CheckOverdueBooks(id); // Check for overdue books before showing the menu
            bool ExitFlag = false;
            do
            {
                Console.WriteLine($"Welcome {name} in Library");
                Console.WriteLine("\n Enter the No of operation you need :");
                Console.WriteLine("\n 1 .Search For Book");
                Console.WriteLine("\n 2 .Borrow Book");
                Console.WriteLine("\n 3 .Return Book");
                Console.WriteLine("\n 4 .Suggestions For you");
                Console.WriteLine("\n 5 .Book Store");
                Console.WriteLine("\n 6 .Show Your Profile");
                Console.WriteLine("\n 7 .singOut");

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
                        BookStore(id);
                        break;
                    case "6":
                        ShowProfile(id);
                        break;
                    case "7":
                        SaveBooksToFile();
                        saveAllActions();
                        Console.WriteLine("\npress Enter key to exit out system");
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
        static void CheckOverdueBooks(int userId)
        {
            // Find all overdue books for the user
            var overdueBooks = Borrowings.Where(b => b.uid == userId && !b.ISReturned && b.ReturnDate < DateTime.Now).ToList();

            if (overdueBooks.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("You have overdue books that need to be returned before you can proceed with other operations.");

                // Display each overdue book
                foreach (var overdue in overdueBooks)
                {
                    var book = Books.FirstOrDefault(b => b.BID == overdue.bid);
                    if (book != default)
                    {
                        Console.WriteLine($"Overdue Book: {book.BName} (Return Date: {overdue.ReturnDate.ToShortDateString()})");
                    }
                }

                // Ask if the user wants to return the books
                Console.WriteLine("Do you want to return these books now? (y/n)");
                string response = Console.ReadLine();

                if (response.ToLower() == "y")
                {
                    // Process each overdue book
                    foreach (var overdue in overdueBooks)
                    {
                        HandleOverdueBooks(userId);
                    }
                }
                else
                {
                    Console.WriteLine("You must return the overdue books to proceed.");
                    loginPage();
                }
            }
        }
        static void HandleOverdueBooks(int userId)
        {
            // Find all overdue books for the user
            var overdueBooks = Borrowings.Where(b => b.uid == userId && !b.ISReturned && b.ReturnDate < DateTime.Now).ToList();

            if (overdueBooks.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("You have overdue books that need to be returned before you can proceed with other operations.");

                // Display each overdue book
                foreach (var overdue in overdueBooks)
                {
                    var book = Books.FirstOrDefault(b => b.BID == overdue.bid);
                    if (book != default)
                    {
                        Console.WriteLine($"Overdue Book: {book.BName} (Return Date: {overdue.ReturnDate.ToShortDateString()})");
                    }
                }

                // Ask if the user wants to return the books
                Console.WriteLine("Do you want to return these books now? (y/n)");
                string response = Console.ReadLine();

                if (response.ToLower() == "y")
                {
                    // Process each overdue book for return
                    foreach (var overdue in overdueBooks)
                    {
                        var bookIndex = Books.FindIndex(b => b.BID == overdue.bid);
                        if (bookIndex != -1)
                        {
                            ReturnBook(userId, bookIndex);  // Return the book by passing the userId and the book index
                        }
                    }

                    Console.WriteLine("All overdue books have been returned.");
                    string press= Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("You must return the overdue books to proceed.");
                    loginPage();  // Redirect to the login page if they refuse to return
                }
            }
            else
            {
                Console.WriteLine("No overdue books found. You can proceed.");
            }
        }
        static void BorrowBook(int userId)
        {
            try
            {
                // Display all books available for borrowing
                ViewAllBooks();

                Console.WriteLine("\nEnter the ID of the book you want to borrow:");
                if (!int.TryParse(Console.ReadLine(), out int bookId))
                {
                    Console.WriteLine("Error: Please enter a valid book ID.");
                    return;
                }

                // Find the selected book by its ID
                var selectedBook = Books.FirstOrDefault(b => b.BID == bookId);

                if (selectedBook.Equals(default))
                {
                    Console.WriteLine("Error: Book not found with the specified ID.");
                    return;
                }

                // Check if user has already borrowed this book and hasn't returned it yet
                var existingBorrowing = Borrowings.FirstOrDefault(b => b.uid == userId && b.bid == selectedBook.BID && b.ISReturned == false);
                if (existingBorrowing != default)
                {
                    Console.WriteLine("Error: You have already borrowed this book and haven't returned it yet.");
                    return;
                }

                // Check if there are available copies to borrow
                int availableCopies = selectedBook.copies - selectedBook.borrowedCopies;
                if (availableCopies <= 0)
                {
                    Console.WriteLine("Sorry! All copies of this book are currently borrowed.");
                    ReserveBook(userId, selectedBook);
                    
                    return;
                }

                // Ask the user if they want to borrow the book
                Console.WriteLine($"Do you want to borrow \"{selectedBook.BName}\"? (Press 'y' to confirm)");
                string selected = Console.ReadLine();
                if (selected.ToLower() != "y")
                {
                    Console.WriteLine("Borrowing canceled.");
                    return;
                }

                // Increase borrowedCopies by 1
                selectedBook.borrowedCopies++;
                Books[Books.FindIndex(b => b.BID == selectedBook.BID)] = selectedBook;

                // Set borrowing details
                DateTime borrowDate = DateTime.Now;
                DateTime returnDate = borrowDate.AddDays(selectedBook.BorrowPeriod);

                // Add borrowing entry with ISReturned = false, ActualReturnDate and Rating as null
                Borrowings.Add((userId, selectedBook.BID, borrowDate, returnDate, null, null, false));

                Console.WriteLine($"You have successfully borrowed \"{selectedBook.BName}\"!");

                // Provide suggestions for the user
                suggestionsForUser(userId, selectedBook.BID);
            }
            catch (Exception ex)
            {
                // Catch any unexpected errors
                Console.WriteLine("An error occurred while borrowing the book: " + ex.Message);
            }
        }
        static void ReserveBook(int userId, (int BID, string BName, string BAuthor, int copies, int borrowedCopies, double Price, string catagory, int BorrowPeriod) book)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(reservationFilePath, true))
                {
                    // Save reservation details to the file
                    writer.WriteLine($"{userId}|{book.BID}|{DateTime.Now.ToString("yyyy-MM-dd")}");
                }
                Console.WriteLine("The book is currently out of stock. Your reservation has been placed. You will be notified when the book becomes available.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving reservation to file: {ex.Message}");
            }
        }
        static void CheckReservations()
        {
            try
            {
                if (File.Exists(reservationFilePath))
                {
                    using (StreamReader reader = new StreamReader(reservationFilePath))
                    {
                        string line;
                        List<string> notifications = new List<string>();

                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 3)
                            {
                                int userId = int.Parse(parts[0]);
                                int bookId = int.Parse(parts[1]);
                                DateTime reservationDate = DateTime.Parse(parts[2]);

                                // Check if the reserved book is now available
                                var book = Books.FirstOrDefault(b => b.BID == bookId);
                                if (book != default)
                                {
                                    int availableCopies = book.copies - book.borrowedCopies;
                                    if (availableCopies > 0)
                                    {
                                        // Notify the user
                                        Console.WriteLine($"Notification for User {userId}: The book \"{book.BName}\" is now available for borrowing.");
                                        Console.WriteLine("Press 'y' to acknowledge and remove this notification.");

                                        // Wait for user input
                                        string input = Console.ReadLine();
                                        if (input.ToLower() == "y")
                                        {
                                            // Remove reservation from the file
                                            notifications.Add(line);
                                        }
                                    }
                                }
                            }
                        }

                        // Remove acknowledged reservations from the file
                        if (notifications.Count > 0)
                        {
                            var allLines = File.ReadAllLines(reservationFilePath).ToList();
                            allLines.RemoveAll(l => notifications.Contains(l));
                            File.WriteAllLines(reservationFilePath, allLines);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking reservations: {ex.Message}");
            }
        }

        static void ReturnBook(int userId, int index = -1)
        {
            try
            {
                DisplayYourBookBorrowed(userId);

                if (index == -1 || index >= Books.Count)
                {
                    Console.WriteLine("Error: Invalid book index.");
                    return;
                }

                var book = Books[index];
                int borrowedCopies = book.borrowedCopies;
                int copies = book.copies;

                // Check if the user has borrowed this book
                var borrowingRecord = Borrowings.FirstOrDefault(b => b.uid == userId && b.bid == book.BID && !b.ISReturned);

                if (borrowingRecord == default)
                {
                    Console.WriteLine("Error: You have not borrowed this book.");
                    return;
                }

                Console.WriteLine("Do you want to return the Book? Press 'y' to confirm:");
                string selected = Console.ReadLine();

                if (selected != "y")
                {
                    Console.WriteLine("Sorry! Cannot return this " + book.BName);
                    return;
                }

                // Check if the book has already been returned
                if (borrowedCopies <= 0)
                {
                    Console.WriteLine("Error: No borrowed copies to return.");
                    return;
                }

                // Increment the number of copies available (return one book)
                borrowedCopies++;
                Books[index] = (book.BID, book.BName, book.BAuthor, book.copies, borrowedCopies, book.Price, book.catagory, book.BorrowPeriod);

                // Ask the user to rate the book before finalizing the return
                int rating = 0;
                bool validRating = false;
                while (!validRating)
                {
                    Console.WriteLine("Please provide a rating for the book (1-5): ");
                    string ratingInput = Console.ReadLine();

                    if (int.TryParse(ratingInput, out rating) && rating >= 1 && rating <= 5)
                    {
                        validRating = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid rating. Please enter a number between 1 and 5.");
                    }
                }

                // Update the borrowing record to reflect the return
                for (int i = 0; i < Borrowings.Count; i++)
                {
                    if (Borrowings[i].uid == userId && Borrowings[i].bid == book.BID && !Borrowings[i].ISReturned)
                    {
                        Borrowings[i] = (Borrowings[i].uid, Borrowings[i].bid, Borrowings[i].date, Borrowings[i].ReturnDate, DateTime.Now, rating, true);
                        break; // Exit the loop once the record is updated
                    }
                }

                Console.WriteLine($"'{book.BName}' has been returned successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while returning the book: " + ex.Message);
            }
        }
        static void ShowProfile(int userId)
        {
            var user = Users.FirstOrDefault(u => u.UID == userId);
            if (user.UID == 0)
            {
                Console.WriteLine("User not found.");
                return;
            }

            // Display user information
            Console.WriteLine("+-----------------------------------------------+");
            Console.WriteLine("|                 Profile Information           |");
            Console.WriteLine("+-----------------------------------------------+");
            Console.WriteLine($"| ID:    {user.UID,-42}");
            Console.WriteLine($"| Name:  {user.Uname,-42}");
            Console.WriteLine($"| Email: {user.Email,-42}");
            Console.WriteLine("+-----------------------------------------------+\n");

            // Currently borrowed books
            var currentBorrowings = Borrowings
                .Where(b => b.uid == userId && b.ISReturned == false)
                .ToList();

            if (currentBorrowings.Any())
            {
                Console.WriteLine("+-------------------------------------------------------------------------+");
                Console.WriteLine("|                         Currently Borrowed Books                        |");
                Console.WriteLine("+-------------------------------------------------------------------------+");
                Console.WriteLine("| Book Name                           | Author       | Due Date           |");
                Console.WriteLine("+-------------------------------------------------------------------------+");

                foreach (var borrow in currentBorrowings)
                {
                    var book = Books.FirstOrDefault(b => b.BID == borrow.bid);
                    Console.WriteLine($"| {book.BName,-35} | {book.BAuthor,-12} | {borrow.ReturnDate:dd/MM/yyyy,-18} ");
                }
                Console.WriteLine("+-------------------------------------------------------------------------+\n");
            }
            else
            {
                Console.WriteLine("No currently borrowed books.\n");
            }

            // Previously returned books
            var returnedBooks = Borrowings
                .Where(b => b.uid == userId && b.ISReturned == true)
                .ToList();

            if (returnedBooks.Any())
            {
                Console.WriteLine("+-----------------------------------------------------------------------------------------------------+");
                Console.WriteLine("|                                Previously Borrowed & Returned Books                                  |");
                Console.WriteLine("+-----------------------------------------------------------------------------------------------------+");
                Console.WriteLine("| Book Name                           | Author       | Borrowed On  | Returned On  | Status            |");
                Console.WriteLine("+-----------------------------------------------------------------------------------------------------+");

                foreach (var borrow in returnedBooks)
                {
                    var book = Books.FirstOrDefault(b => b.BID == borrow.bid);
                    string onTime = borrow.ActualReturnDate <= borrow.ReturnDate ? "On time" : "Overdue";

                    Console.WriteLine($"| {book.BName,-35} | {book.BAuthor,-12} | {borrow.date:dd/MM/yyyy,-12} | {borrow.ActualReturnDate?.ToString("dd/MM/yyyy"),-12} | {onTime,-16} ");
                }
                Console.WriteLine("+-----------------------------------------------------------------------------------------------------+\n");
            }
            else
            {
                Console.WriteLine("No previously returned books.\n");
            }
        }
        static void BookStore(int userId)
        {
            // Step 1: View all books
            ViewAllBooks();

            // Step 2: Ask for the book ID
            Console.Write("\nEnter the Book ID you want to purchase: ");
            if (!int.TryParse(Console.ReadLine(), out int bookId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Book ID.");
                return;
            }

            // Step 3: Find the book by ID
            var book = Books.FirstOrDefault(b => b.BID == bookId);

            if (book.Equals(default))
            {
                Console.WriteLine("Error: Book not found.");
                return;
            }

            // Show book details
            Console.WriteLine($"\nBook Details:");
            Console.WriteLine($"Title: {book.BName}");
            Console.WriteLine($"Author: {book.BAuthor}");
            Console.WriteLine($"Price: {book.Price.ToString("0.00")} OMR");
            Console.WriteLine($"Available Copies: {book.copies - book.borrowedCopies}");

            // Step 4: Ask if the user wants to buy the book
            Console.Write("\nDo you want to buy this book? (yes/no): ");
            string response = Console.ReadLine().Trim().ToLower();

            if (response == "yes")
            {
                // Check if copies are available
                if (book.copies - book.borrowedCopies <= 0)
                {
                    Console.WriteLine("Sorry, this book is out of stock.");
                    return;
                }

                // Deduct a copy from available books
                for (int i = 0; i < Books.Count; i++)
                {
                    if (Books[i].BID == book.BID)
                    {
                        Books[i] = (Books[i].BID, Books[i].BName, Books[i].BAuthor, Books[i].copies - 1, Books[i].borrowedCopies, Books[i].Price, Books[i].catagory, Books[i].BorrowPeriod);
                        break;
                    }
                }

                // Record the purchase in the Purchases list
                Purchases.Add((purchaseIdCounter++, userId, book.BID, DateTime.Now, book.Price));
                Console.WriteLine("Purchase successful! Thank you for buying the book.");

                // Step 5: Call suggestions for additional purchases
                SuggestionsForBuy(userId, book);
            }
            else
            {
                Console.WriteLine("Purchase cancelled.");
            }
        }

        static void SuggestionsForBuy(int userId, (int BID, string BName, string BAuthor, int copies, int borrowedCopies, double Price, string catagory, int BorrowPeriod) purchasedBook)
        {
            var purchasedBookCategory = purchasedBook.catagory;

            // Fetch all books in the same category
            var booksInCategory = Books.Where(b => b.catagory == purchasedBookCategory).ToList();

            if (!booksInCategory.Any())
            {
                Console.WriteLine("No other books available in this category.");
                return;
            }

            // Find the most and least borrowed books in this category
            var mostBorrowedBook = booksInCategory.OrderByDescending(b => Borrowings.Count(br => br.bid == b.BID)).FirstOrDefault();
            var leastBorrowedBook = booksInCategory.OrderBy(b => Borrowings.Count(br => br.bid == b.BID)).FirstOrDefault();

            // Suggest buying the least borrowed book with a discount
            if (leastBorrowedBook != default)
            {
                double discount = 0.10; // 10% discount
                double discountedPrice = leastBorrowedBook.Price * (1 - discount);
                Console.WriteLine($"\nSpecial Offer: Buy '{leastBorrowedBook.BName}' by {leastBorrowedBook.BAuthor} at a 10% discount! Price: {discountedPrice.ToString("0.00")} OMR");
            }

            // Suggest the most borrowed book with a discount
            if (mostBorrowedBook != default)
            {
                double discount = 0.05; // 5% discount
                double discountedPrice = mostBorrowedBook.Price * (1 - discount);
                Console.WriteLine($"\nRecommended: Buy '{mostBorrowedBook.BName}' by {mostBorrowedBook.BAuthor}' with a 5% discount! Price: {discountedPrice.ToString("0.00")} OMR");
            }

            // Show additional purchase suggestions
            Console.WriteLine("\nAdditional Purchase Suggestions:");
            var similarBooks = booksInCategory.Where(b => b.BID != purchasedBook.BID).ToList();

            if (similarBooks.Any())
            {
                Console.WriteLine("Books you might also like:");
                foreach (var similarBook in similarBooks)
                {
                    Console.WriteLine($"- '{similarBook.BName}' by {similarBook.BAuthor} ({similarBook.Price.ToString("0.00")} OMR)");
                }
            }
            else
            {
                Console.WriteLine("No additional suggestions available.");
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
                var book = Books.FirstOrDefault(b => b.BID == borrowing.bid);

                if (book != default && borrowing.ISReturned !=true)
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
            List<int> suggestedBookIds = new List<int>();
            if (BookID == 0)
            {
                try
                {
                    Console.Clear();
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
                        if (book.BID == mostBorrowedBookId)
                            BookName = book.BName;
                    }
                    suggestedBookIds.Add(mostBorrowedBookId);
                    // Output the ID of the most borrowed book
                    Console.WriteLine($"Most Borrowed Book ID: {mostBorrowedBookId} the name book:{BookName}\n");


                    // Create lists to track authors and their corresponding borrow counts
                    List<string> authors = new List<string>();
                    List<int> authorCounts = new List<int>();

                    // Count the borrowings for each author
                    foreach (var borrowing in Borrowings)
                    {
                        // Find the book corresponding to the borrowing
                        var book = Books.FirstOrDefault(b => b.BID == borrowing.bid);
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
                    Console.WriteLine($"Best Author: {bestAuthor}\n");
                    foreach (var book in Books) 
                    {if (book.BAuthor == bestAuthor)
                        { suggestedBookIds.Add(book.BID); }
                    }


                        // List to keep track of book IDs borrowed along with the most borrowed book
                        List<int> borrowedWithMostBorrowedBook = new List<int>();

                    // Find the book IDs borrowed by the same users who borrowed the most borrowed book
                    foreach (var borrowing in Borrowings)
                    {
                        if (borrowing.bid == mostBorrowedBookId && !borrowing.ISReturned)
                        {
                            foreach (var otherBorrowing in Borrowings)
                            {
                                if (otherBorrowing.uid == borrowing.uid && otherBorrowing.bid != mostBorrowedBookId && !otherBorrowing.ISReturned)
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
                    Console.WriteLine("Books borrowed with the most borrowed book:\n");
                    foreach (var bookId in borrowedWithMostBorrowedBook)
                    {
                        string bookn = null;
                        foreach (var book in Books)
                        {
                            if (bookId == book.BID)
                            {
                                bookn = book.BName;
                                suggestedBookIds.Add(bookId);
                            }
                        }
                        Console.WriteLine(bookId + ": " + bookn);
                    }
                   borrowingAfterSuggestions(userID, suggestedBookIds);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while suggesting books: " + ex.Message);
                }
            }
            else
            {
                Console.Clear();


                // Check if the borrowed book exists
                var borrowedBook = Books.FirstOrDefault(b => b.BID == BookID);
                if (borrowedBook == default)
                {
                    Console.WriteLine("Book not found for suggestions.");
                    return;
                }

                Console.WriteLine("\nBook suggestions for you based on your recent borrowing:");

                // Suggest books by the same author
                Console.WriteLine("\nBooks by the same author:");
                var booksBySameAuthor = Books.Where(b => b.BAuthor == borrowedBook.BAuthor && b.BID != BookID).ToList();
                if (booksBySameAuthor.Count > 0)
                {
                    foreach (var book in booksBySameAuthor)
                    {
                        Console.WriteLine($"{book.BName} by {book.BAuthor}");
                        suggestedBookIds.Add(book.BID);
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
                    if (borrowing.bid == BookID && borrowing.ISReturned == false)
                    {
                        foreach (var otherBorrowing in Borrowings)
                        {
                            if (otherBorrowing.uid == borrowing.uid && otherBorrowing.bid != BookID && !otherBorrowing.ISReturned)
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
                        var book = Books.FirstOrDefault(b => b.BID == bookId);
                        if (book != default)
                        {
                            Console.WriteLine($"{book.BName} by {book.BAuthor}");
                            suggestedBookIds.Add(book.BID);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No other books borrowed with this one.");
                }

                // Suggest the most borrowed books
                Console.WriteLine("\nPopular books among other users:/n");
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
                    var book = Books.FirstOrDefault(b => b.BID == bookEntry.Key);
                    if (book != default)
                    {
                        Console.WriteLine($"{book.BName} by {book.BAuthor}");
                        suggestedBookIds.Add(book.BID );
                    }
                }
                borrowingAfterSuggestions(userID,suggestedBookIds);
            }
        }
        static void borrowingAfterSuggestions(int userId, List<int> suggestedBookIds)
        {
            Console.Clear();
            try
            {
                // Remove duplicate book IDs from suggestions
                suggestedBookIds = suggestedBookIds.Distinct().ToList();

                
                if (suggestedBookIds.Count == 0)
                {
                    Console.WriteLine("No books are available to borrow from the suggestions.");
                    return;
                }

                // Display suggested books
                Console.WriteLine("\nWould you like to borrow any of these suggested books?");
                for (int i = 0; i < suggestedBookIds.Count; i++)
                {
                    var book = Books.FirstOrDefault(b => b.BID == suggestedBookIds[i]);
                    if (book != default)
                    {
                        Console.WriteLine($"{i + 1}. {book.BName} by {book.BAuthor} (ID: {book.BID})");
                    }
                }

                // Ask the user if they want to borrow a book
                Console.WriteLine("\nEnter the number of the book you want to borrow, or press 0 to cancel:");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= suggestedBookIds.Count)
                {
                    int bookId = suggestedBookIds[choice - 1];
                    var bookToBorrow = Books.FirstOrDefault(b => b.BID == bookId);

                    if (bookToBorrow != default)
                    {
                        var existingBorrowing = Borrowings.FirstOrDefault(b => b.uid == userId && b.bid == bookToBorrow.BID && !b.ISReturned);

                        if (existingBorrowing != default)
                        {
                            Console.WriteLine($"Error: You have already borrowed {bookToBorrow.BName} and haven't returned it yet.");
                            return;
                        }

                        if (bookToBorrow.borrowedCopies > 0)
                        {
                            Console.WriteLine($"You have successfully borrowed {bookToBorrow.BName}.");
                            Books[Books.IndexOf(bookToBorrow)] = (bookToBorrow.BID, bookToBorrow.BName, bookToBorrow.BAuthor,  bookToBorrow.copies, bookToBorrow.borrowedCopies - 1, bookToBorrow.Price, bookToBorrow.catagory, bookToBorrow.BorrowPeriod);
                            //Borrowings.Add((userId, bookToBorrow.BID, DateTime.Now, false));
                        }
                        else
                        {
                            Console.WriteLine($"Sorry, {bookToBorrow.BName} is currently not available.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No book selected for borrowing.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while borrowing a suggested book: " + ex.Message);
            }
        }
        //........................helper Functions.....................................//
        static void LoadAllFiles()
        {
            // Load Books
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
                            if (parts.Length == 8)
                            {
                                // Parse each part into the appropriate data type
                                int bid = int.Parse(parts[0]);
                                string bookName = parts[1];
                                string author = parts[2];
                                int copies = int.Parse(parts[3]);
                                int borrowedCopies = int.Parse(parts[4]);
                                double price = double.Parse(parts[5]);
                                string category = parts[6];
                                int borrowPeriod = int.Parse(parts[7]);

                                // Add to the Books list
                                Books.Add((bid, bookName, author, copies, borrowedCopies, price, category, borrowPeriod));
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Books file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading books from file: {ex.Message}");
            }

            // Load Users
            try
            {
                if (File.Exists(userFilePath))
                {
                    using (StreamReader reader = new StreamReader(userFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 4)
                            {
                                Users.Add((int.Parse(parts[0]), parts[1], parts[2], parts[3]));
                            }
                        }
                    }
                    
                }
                else
                {
                    Console.WriteLine("Users file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users from file: {ex.Message}");
            }

            // Load Borrowings
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
                            if (parts.Length == 7)
                            {
                                int uid = int.Parse(parts[0]);
                                int bid = int.Parse(parts[1]);
                                DateTime date = DateTime.Parse(parts[2]);
                                DateTime returnDate = DateTime.Parse(parts[3]);

                                // Handle ActualReturnDate
                                DateTime? actualReturnDate = parts[4].Equals("N/A", StringComparison.OrdinalIgnoreCase) ? (DateTime?)null : DateTime.Parse(parts[4]);

                                // Handle Rating
                                int? rating = parts[5].Equals("N/A", StringComparison.OrdinalIgnoreCase) ? (int?)null : int.Parse(parts[5]);

                                bool isReturned = bool.Parse(parts[6]);

                                Borrowings.Add((uid, bid, date, returnDate, actualReturnDate, rating, isReturned));
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Borrowing file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading borrowings from file: {ex.Message}");
            }

            // Load Categories
            try
            {
                if (File.Exists(CategoriesFile))
                {
                    using (StreamReader reader = new StreamReader(CategoriesFile))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 3)
                            {
                                int cid = int.Parse(parts[0]);
                                string categoryName = parts[1];
                                int numberOfBooks = int.Parse(parts[2]);

                                Categories.Add((cid, categoryName, numberOfBooks));
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Categories file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading categories from file: {ex.Message}");
            }

            // Load Admins
            try
            {
                if (File.Exists(adminFilePath))
                {
                    using (StreamReader reader = new StreamReader(adminFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 4)
                            {
                                Admins.Add((int.Parse(parts[0]), parts[1], parts[2], parts[3]));
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Admins file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading admins from file: {ex.Message}");
            }
            // Load Purchases
            try
            {
                if (File.Exists(purchasesFilePath))
                {
                    using (StreamReader reader = new StreamReader(purchasesFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 5)
                            {
                                try
                                {
                                    int purchaseID = int.Parse(parts[0]);
                                    int uid = int.Parse(parts[1]);
                                    int bid = int.Parse(parts[2]);
                                    DateTime purchaseDate = DateTime.Parse(parts[3]);
                                    double price = double.Parse(parts[4]);

                                    Purchases.Add((purchaseID, uid, bid, purchaseDate, price));
                                }
                                catch (FormatException fe)
                                {
                                    Console.WriteLine($"Error parsing line: {line}. Exception: {fe.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Incorrect format in line: {line}");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Purchases file not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading purchases from file: {ex.Message}");
            }

            Console.WriteLine("All data loaded successfully from files!");
        }
        static void SaveBooksToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var book in Books)
                    {
                        writer.WriteLine($"{book.BID}|{book.BName}|{book.BAuthor}|{book.copies}|{book.borrowedCopies}|{book.Price}|{book.catagory}|{book.BorrowPeriod}");
                    }
                }
                Console.WriteLine("Books updated to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving books to file: {ex.Message}");
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(CategoriesFile))
                {
                    foreach (var c in Categories)
                    {
                        writer.WriteLine($"{c.CID}|{c.CName}|{c.NOFBooks}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving categories to file: {ex.Message}");
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
                        writer.WriteLine($"{user.UID}|{user.Uname}|{user.Email}|{user.Password}");
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
                       writer.WriteLine($"{admin.AID}|{admin.AName}|{admin.Email}|{admin.Password}");
                    }
                }
                Console.WriteLine("All users data saved to file successfully!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
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
                        // Format dates and handle null values
                        string actualReturnDate = b.ActualReturnDate.HasValue ? b.ActualReturnDate.Value.ToString("yyyy-MM-dd") : "N/A";
                        string rating = b.Rating.HasValue ? b.Rating.Value.ToString() : "N/A";

                        // Save all fields to the file
                        writer.WriteLine($"{b.uid}|{b.bid}|{b.date.ToString("yyyy-MM-dd")}|{b.ReturnDate.ToString("yyyy-MM-dd")}|{actualReturnDate}|{rating}|{b.ISReturned}");
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(purchasesFilePath))
                {
                    foreach (var purchase in Purchases)
                    {
                        // Format date and handle other values
                        writer.WriteLine($"{purchase.PurchaseID}|{purchase.UID}|{purchase.BID}|{purchase.PurchaseDate:yyyy-MM-dd}|{purchase.Price:0.00}");
                    }
                }
                Console.WriteLine("saved successfully...!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving purchases to file: {ex.Message}");
            }
        }


    }
}
