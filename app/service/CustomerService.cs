namespace service;

using model;
using util;

public interface ICustomerService
{
    bool DisplayCustomerMenu(User user);
}

public class CustomerService : ICustomerService
{
    private readonly CustomerModel _customerModel;

    public CustomerService(CustomerModel customerModel)
    {
        _customerModel = customerModel;
    }
    public bool DisplayCustomerMenu(User user)
    {
        var customer = _customerModel.GetByUserID(user.user_id);
        bool exit = false;

        while (!exit)
        {
            /*
                1----Withdraw Cash
                2----Deposit Cash
                3----Display Balance
                4----Exit
            */
            Console.Clear();
            Console.WriteLine("----- Customer Menu -----");
            Console.WriteLine($"Welcome {customer.customer_name}!");
            Console.WriteLine("1 ------ Withdraw Cash");
            Console.WriteLine("2 ------ Deposit Cash");
            Console.WriteLine("3 ------ Display Balance");
            Console.WriteLine("4 ------ Exit");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.D1: // 1----Withdraw Cash
                    WithdrawCash(customer);
                    break;

                case ConsoleKey.D2: // 2----Deposit Cash
                    DepositCash(customer);
                    break;

                case ConsoleKey.D3: // 3----Display Balance
                    DisplayBalance(customer);
                    break;

                case ConsoleKey.D4: // 4----Exit
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

    public void WithdrawCash(Customer customer)
    {
        Console.Clear();
        Console.WriteLine("Withdrawing Cash...");
        decimal withdrawal;

        while (true)
        {
            withdrawal = InputHelper.ReadCashAmount("Enter the cash amount to withdraw: ");
            try
            {
                customer.Withdraw(withdrawal);
                break;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        _customerModel.Update(customer);
        Console.WriteLine("Cash withdrawn successfully.");
        DisplayBalance(customer);
    }

    public void DepositCash(Customer customer)
    {
        Console.Clear();
        Console.WriteLine("Depositing cash...");
        decimal deposit = InputHelper.ReadCashAmount("Enter the cash amount to deposit: ");
        customer.Deposit(deposit);
        _customerModel.Update(customer);
        Console.WriteLine("Cash deposited successfully.");
        DisplayBalance(customer);
    }

    public void DisplayBalance(Customer customer)
    {
        Console.Clear();
        Console.WriteLine($"Account #{customer.customer_id}");
        Console.WriteLine($"Date: {DateTime.Today}");
        Console.WriteLine($"Balance: {customer.balance}");

        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }
}
