namespace service;

using System.Data.Common;
using dal;
using model;
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
        string newusername;
        while (true)
        {
            Console.Write("Enter Account Holder's username: ");
            newusername = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newusername)) //make sure input is not blank
                break;

            Console.WriteLine("Username cannot be blank.");
        }

        // ask for new account pin
        string newpasswordstr;
        while (true)
        {
            Console.Write("Enter Account Holder's Pin code: ");
            newpasswordstr = Console.ReadLine();

            if (newpasswordstr.Length == 5 && int.TryParse(newpasswordstr, out _)) //Check if pin is a 5 digit integer
                break;

            Console.WriteLine("Pin Code must be an integer of length 5.");
        }
        int newpassword = int.Parse(newpasswordstr);

        // ask for new account holder name
        string newcustname;
        while (true)
        {
            Console.Write("Enter Account Holder's Name: ");
            newcustname = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newcustname)) //make sure input is not blank
                break;

            Console.WriteLine("Customer name cannot be blank.");
        }

        // ask for new account balance
        string newbalancestr;
        decimal newbalance;
        while (true)
        {
            Console.Write("Enter Account Holder's Starting Balance: ");
            newbalancestr = Console.ReadLine();

            if (decimal.TryParse(newbalancestr, out newbalance) && newbalance > 0) // validate balance
                break;

            Console.WriteLine("Balance must be a valid amount greater than zero.");
        }

        // ask for new account status
        string newstatus;
        while (true)
        {
            Console.Write("Enter Account Holder's Status: ");
            newstatus = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newstatus)) //make sure input is not blank
                break;

            Console.WriteLine("Account status cannot be blank.");
        }


        Console.WriteLine("Added customer: " + newusername + " | "
            + newpasswordstr + " | "
            + newcustname + " | "
            + newbalancestr + " | "
            + newstatus + " | "
        );

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

        int cust_id;

        while (true)
        {
            Console.Write("Please Enter Customer ID: ");
            string custIdStr = Console.ReadLine();

            if (!int.TryParse(custIdStr, out cust_id) || string.IsNullOrWhiteSpace(custIdStr))
            {
                Console.WriteLine("Invalid customer ID. Please enter a valid number.");
                continue;
            }

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
