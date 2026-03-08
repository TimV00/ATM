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
            Console.WriteLine("=== ATM Login ===");

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string passwordstr = Console.ReadLine();
            int password = int.Parse(passwordstr);
            
            var user = AuthService.Authenticate(username, password);

            if (user != null)
            {
                Console.WriteLine("Login successful!");
                Console.WriteLine($"Welcome {user.username}");
                Console.WriteLine($"{user.user_id} | {user.username} | {user.role}");
            }
            else
            {
                Console.WriteLine("Login failed.");
            }
        }
    }
}
