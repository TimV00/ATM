namespace service;

using System.Data.Common;
using dal;
using model;
using util;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Data.SqlTypes;

public class CustomerService
{
    public static bool DisplayCustomerMenu(User user)
    {
        var customer = CustomerModel.GetByUserID(user.user_id);
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

    public static void WithdrawCash(Customer customer)
    {
        Console.Clear();
        Console.WriteLine("Withdrawing Cash...");
        decimal withdrawal;

        while (true)
        {
            withdrawal = InputHelper.ReadDeposit("Enter the cash amount to withdraw: ");

            if (withdrawal <= customer.balance)
                break;

            Console.WriteLine("Withdrawal amount cannot be greater than current balance.");
        }

        customer.balance -= withdrawal;
        CustomerModel.Update(customer);
        Console.WriteLine("Cash withdrawn successfully.");
        DisplayBalance(customer);
    }

    public static void DepositCash(Customer customer)
    {
        Console.Clear();
        Console.WriteLine("Depositing cash...");
        // ask for new account balance
        decimal deposit = InputHelper.ReadDeposit("Enter the cash amount to deposit: ");
        customer.balance += deposit;
        CustomerModel.Update(customer);
        Console.WriteLine("Cash deposited successfully.");
        DisplayBalance(customer);
    }

    public static void DisplayBalance(Customer customer)
    {
        Console.Clear();
        Console.WriteLine($"Account #{customer.customer_id}");
        Console.WriteLine($"Date: {DateTime.Today}");
        Console.WriteLine($"Balance: {customer.balance}");

        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }
}
