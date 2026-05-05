namespace service;

using model;
using util;

/// <summary>
/// Defines the contract for the admin menu service.
/// </summary>
public interface IAdminService
{
    bool DisplayAdminMenu(User user);
}

/// <summary>
/// Handles all administrative operations including account creation, deletion, updating, and searching.
/// </summary>
public class AdminService : IAdminService
{
    private readonly UserModel _userModel;
    private readonly CustomerModel _customerModel;

    /// <summary>
    /// Initializes a new instance of <see cref="AdminService"/>.
    /// </summary>
    /// <param name="userModel">The user model for database access.</param>
    /// <param name="customerModel">The customer model for database access.</param>
    public AdminService(UserModel userModel, CustomerModel customerModel)
    {
        _userModel = userModel;
        _customerModel = customerModel;
    }

    /// <summary>
    /// Displays the admin menu and handles navigation to admin operations.
    /// </summary>
    /// <param name="user">The authenticated admin user.</param>
    /// <returns>True if the user chose to exit the application.</returns>
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

    /// <summary>
    /// Prompts the admin to enter details for a new customer account and creates it.
    /// </summary>
    public void CreateNewAccount()
    {
        Console.Clear();
        Console.WriteLine("Creating new account...");

        string newusername = InputHelper.ReadString("Enter Account Holder's username: ");
        int newpassword = InputHelper.ReadPin("Enter Account Holder's Pin code: ");
        string newcustname = InputHelper.ReadString("Enter Account Holder's Name: ");
        decimal newbalance = InputHelper.ReadBalance("Enter Account Holder's Starting Balance: ");
        string newstatus = InputHelper.ReadStatus("Enter Account Holder's Status (Active/Inactive): ");

        try
        {
            var newUser = User.Create(0, newusername, newpassword, "Customer");
            var newuserID = _userModel.Create(newUser);

            var newCustomer = Customer.Create(0, newuserID, newcustname, newbalance, newstatus);
            var newcustID = _customerModel.Create(newCustomer);

            Console.WriteLine("Account Successfully Created - the account number assigned is: " + newcustID);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Invalid input: {ex.Message}");
        }

        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Prompts the admin to select a customer account to delete and removes it.
    /// </summary>
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

    /// <summary>
    /// Prompts the admin to select a customer account to update and applies changes.
    /// </summary>
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

        string newcust_name = InputHelper.ReadStringOrSkip($"Current Holder: {customer.customer_name}\nNew Holder (or ENTER to skip): ");
        if (newcust_name != null) { customer.UpdateName(newcust_name); updated = true; }

        string newstatus = InputHelper.ReadStatusOrSkip($"Current Status: {customer.status}\nNew Status (Active/Inactive, or ENTER to skip): ");
        if (newstatus != null) { customer.UpdateStatus(newstatus); updated = true; }

        string newusername = InputHelper.ReadStringOrSkip($"Current Username: {user.username}\nNew Username (or ENTER to skip): ");
        if (newusername != null) { user.UpdateUsername(newusername); updated = true; }

        int? newpassword = InputHelper.ReadPinOrSkip($"Current PIN: {user.password}\nNew PIN (or ENTER to skip): ");
        if (newpassword != null) { user.UpdatePassword(newpassword.Value); updated = true; }

        if (updated)
        {
            _userModel.Update(user);
            _customerModel.Update(customer);
            Console.WriteLine("\nAccount updated successfully.");
        }
        else
        {
            Console.WriteLine("\nNo changes made.");
        }

        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Prompts the admin to search for a customer account by ID and displays the result.
    /// </summary>
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
