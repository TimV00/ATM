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
            Console.Clear();
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("------ ATM Login -----");

                Console.Write("Enter Login: ");
                string username = Console.ReadLine();

                Console.Write("Enter Pin code: ");
                string passwordstr = Console.ReadLine();
                int password = int.Parse(passwordstr);

                var user = AuthService.Authenticate(username, password);

                if (user != null) // login successful
                {
                    if (user.role.ToUpper() != "ADMIN") // if user is a customer
                    {
                        exit = CustomerService.DisplayCustomerMenu(user);
                    }
                    else // admin menu
                    {
                        exit = AdminService.DisplayAdminMenu(user);
                    }
                }
            }
        }
    }
}
