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
            Console.WriteLine($"Welcome {user.username}!");
            Console.WriteLine("1 ------ Withdraw Cash");
            Console.WriteLine("2 ------ Deposit Cash");
            Console.WriteLine("3 ------ Display Balance");
            Console.WriteLine("4 ------ Exit");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.D1: // 1----Withdraw Cash
                    WithdrawCash();
                    break;

                case ConsoleKey.D2: // 2----Deposit Cash
                    DepositCash();
                    break;

                case ConsoleKey.D3: // 3----Display Balance
                    DisplayBalance();
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

    public static void WithdrawCash()
    {
        Console.Clear();
        Console.WriteLine("Withdrawing Cash...");
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }

    public static void DepositCash()
    {
        Console.Clear();
        Console.WriteLine("Depositing cash...");
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }

    public static void DisplayBalance()
    {
        Console.Clear();
        Console.WriteLine("Displaying Balance...");
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }
}
