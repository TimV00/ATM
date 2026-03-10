namespace service;

using System.Data.Common;
using dal;
using model;
using System.Data;
using System.Security.Cryptography.X509Certificates;
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
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey(true);
    }    
}
