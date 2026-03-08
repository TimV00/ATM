using MySql.Data.MySqlClient;
using System.Data;
using service;
using model;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {
                Console.WriteLine("------ ATM Login -----");

                Console.Write("Enter Login: ");
                string username = Console.ReadLine();

                Console.Write("Enter Pin code: ");
                string passwordstr = Console.ReadLine();
                int password = int.Parse(passwordstr);

                var user = AuthService.Authenticate(username, password);

                if (user != null)
                {
                    if (user.role.ToUpper() != "ADMIN") // if user is a customer
                    {
                        Console.Clear();
                        Console.WriteLine("----- Customer Menu -----");
                        Console.WriteLine("Login successful!");
                        Console.WriteLine($"Welcome {user.username}!");
                        Console.WriteLine($"{user.user_id} | {user.username} | {user.role}");
                        Console.WriteLine("5------Exit");
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        if (keyInfo.KeyChar == '5') exit=true;
                    }
                    else // admin menu
                    {
                        Console.Clear();
                        Console.WriteLine("----- Admin Menu -----");
                        Console.WriteLine("Login successful!");
                        Console.WriteLine($"Welcome {user.username}!");
                        Console.WriteLine($"{user.user_id} | {user.username} | {user.role}");
                        Console.WriteLine("5------Exit");
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        if (keyInfo.KeyChar == '5') exit=true;
                    }
                }
                else
                {
                    Console.WriteLine("Login failed. Please try again");
                }
            }
        }
    }
}
