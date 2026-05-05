namespace service;
using model;
using util;

public interface IAdminService
{
    bool DisplayAdminMenu(User user);
}

public class AdminService : IAdminService
{
    private readonly UserModel _userModel;
    private readonly CustomerModel _customerModel;

    public AdminService(UserModel userModel, CustomerModel customerModel)
    {
        _userModel = userModel;
        _customerModel = customerModel;
    }
    public bool DisplayAdminMenu(User user)
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

    public void CreateNewAccount()
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
        var newuserID = _userModel.Create(newUser);

        //Create a new customer table record
        var newCustomer = new Customer
        {
            user_id = newuserID,
            customer_name = newcustname,
            balance = newbalance,
            status = newstatus
        };
        var newcustID = _customerModel.Create(newCustomer);

        Console.WriteLine("Account Successfully Created - the account number assigned is: " + newcustID);
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }

    public void DeleteAccount()
    {
        Console.Clear();
        Console.WriteLine("Deleting account...");
        int cust_id = InputHelper.ReadID("Please Enter Customer ID: ");

        var customer = _customerModel.GetBy(cust_id); // get customer info
        if (customer == null)
        {
            Console.WriteLine("Customer does not exist. Please try again.");
            return;
        }

        var user = _userModel.GetBy(customer.user_id);

        if (!InputHelper.ConfirmId($"\nYou wish to delete the account held by {customer.customer_name} . If this information is correct, please re-enter the account number: ", cust_id))
        {
            Console.WriteLine("Customer ID did not match. Account deletion cancelled.");
            Console.ReadKey(true);
            return;
        }

        _customerModel.DeleteCustomer(cust_id);
        _userModel.DeleteUser(user.user_id);

        Console.WriteLine("Customer account deleted successfully.");
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }

    public void UpdateAccount()
    {
        Console.Clear();
        Console.WriteLine("Updating account...");

        bool updated = false;

        int cust_id = InputHelper.ReadID("Enter Customer ID to update: ");
        var customer = _customerModel.GetBy(cust_id);

        if (customer == null)
        {
            Console.WriteLine("Customer does not exist. Please try again.");
            Console.ReadKey(true);
            return;
        }

        var user = _userModel.GetBy(customer.user_id);

        Console.WriteLine("\nPress ENTER to keep the current value.\n");

        // update customer name
        Console.WriteLine($"Current Holder: {customer.customer_name}");
        Console.Write("New Holder: ");
        string newcust_name = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newcust_name))
        {
            customer.customer_name = newcust_name;
            updated = true;
        }

        // update customer status
        Console.WriteLine($"Current Status: {customer.status}");
        Console.Write("New Status: ");
        string newstatus = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newstatus))
        {
            customer.status = newstatus;
            updated = true;
        }

        // update username
        Console.WriteLine($"Current Username: {user.username}");
        Console.Write("New Username: ");
        string newusername = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newusername))
        {
            user.username = newusername;
            updated = true;
        }

        // update PIN
        Console.WriteLine($"\nCurrent PIN: {user.password}");
        Console.Write("New PIN: ");
        string newpasswordstr = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newpasswordstr))
        {
            if (newpasswordstr.Length == 5 && int.TryParse(newpasswordstr, out int newpassword))
            {
                user.password = newpassword;
                updated = true;
            }
            else
            {
                Console.WriteLine("Invalid PIN. Keeping previous value.");
            }
        }

        if (updated)
        {
            _userModel.Update(user); // update username or pin
            _customerModel.Update(customer); // update customer_name or status
            Console.WriteLine("\nAccount updated successfully.");
        }
        else
        {
            Console.WriteLine("\nNo changes made.");
        }

        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }

    public void SearchAccount()
    {
        Console.Clear();
        Console.WriteLine("Searching for account...");

        while (true)
        {
            int cust_id = InputHelper.ReadID("Please Enter Customer ID: ");

            var customer = _customerModel.GetBy(cust_id); // get customer info
            if (customer == null)
            {
                Console.WriteLine("Customer does not exist. Please try again.");
                continue;
            }

            var user = _userModel.GetBy(customer.user_id); // get user record associated with customer for login info


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
