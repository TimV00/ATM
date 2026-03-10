namespace service;

using System.Data.Common;
using dal;
using model;
using util;
using System.Data;
using System.Data.SqlTypes;

public class AdminService
{
    public static bool DisplayAdminMenu(User user)
    {
        bool exit = false;

        while (!exit)
        {
            /*
                1----Create New Account
                2----Delete Existing Account
                3----Update Account Information
                4----Search for Account
                5----Exit
            */
            Console.Clear();
            Console.WriteLine("----- Admin Menu -----");
            Console.WriteLine($"Welcome {user.username}!");
            Console.WriteLine("1 ------ Create New Account");
            Console.WriteLine("2 ------ Delete Existing Account");
            Console.WriteLine("3 ------ Update Account Information");
            Console.WriteLine("4 ------ Search for Account");
            Console.WriteLine("5 ------ Exit");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.D1: // 1----Create New Account
                    CreateNewAccount();
                    break;

                case ConsoleKey.D2: // 2----Delete Existing Account
                    DeleteAccount();
                    break;

                case ConsoleKey.D3: // 3----Update Account Information
                    UpdateAccount();
                    break;

                case ConsoleKey.D4: // 4----Search for Account
                    SearchAccount();
                    break;

                case ConsoleKey.D5: // 5----Exit
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Invalid selection.");
                    Console.ReadKey();
                    break;
            }
        }

        return exit;
    }

    public static void CreateNewAccount()
    {
        Console.Clear();
        Console.WriteLine("Creating new account...");

        // ask for new account username
        string newusername = InputHelper.ReadString("Enter Account Holder's username: ");

        // ask for new account pin
        int newpassword = InputHelper.ReadPin("Enter Account Holder's Pin code: ");

        // ask for new account holder name
        string newcustname = InputHelper.ReadString("Enter Account Holder's Name: ");

        // ask for new account balance
        decimal newbalance = InputHelper.ReadBalance("Enter Account Holder's Starting Balance: ");

        // ask for new account status
        string newstatus = InputHelper.ReadString("Enter Account Holder's Status: ");

        //Create a new user table record
        var newUser = new User
        {
            user_id = 0,
            username = newusername,
            password = newpassword,
            role = "Customer"
        };
        var newuserID = UserModel.Create(newUser);

        //Create a new customer table record
        var newCustomer = new Customer
        {
            user_id = newuserID,
            customer_name = newcustname,
            balance = newbalance,
            status = newstatus
        };
        var newcustID = CustomerModel.Create(newCustomer);

        Console.WriteLine("Account Successfully Created - the account number assigned is: " + newcustID);
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }

    public static void DeleteAccount()
    {
        Console.Clear();
        Console.WriteLine("Deleting account...");
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }

    public static void UpdateAccount()
    {
        Console.Clear();
        Console.WriteLine("Updating account...");
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }

    public static void SearchAccount()
    {
        Console.Clear();
        Console.WriteLine("Searching for account...");

        while (true)
        {
            int cust_id = InputHelper.ReadID("Please Enter Customer ID: ");

            var customer = CustomerModel.GetBy(cust_id); // get customer info
            if (customer == null)
            {
                Console.WriteLine("Customer does not exist. Please try again.");
                continue;
            }

            var user = UserModel.GetBy(customer.user_id); // get user record associated with customer for login info


            // Customer found
            Console.WriteLine($"Customer found. The account information is:");
            Console.WriteLine($"Holder: {customer.customer_name}");
            Console.WriteLine($"Balance: {customer.balance}");
            Console.WriteLine($"Status: {customer.status}");
            Console.WriteLine($"Username: {user.username}");
            Console.WriteLine($"Pin Code: {user.password}");
            break;
        }

        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }
}
