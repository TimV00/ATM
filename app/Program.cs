using MySql.Data.MySqlClient;
using System.Data;
using service;
using model;
using util;

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

                string username = InputHelper.ReadString("Enter Username: ");
                int password = InputHelper.ReadPin("Enter Pin code: ");

                var user = AuthService.Authenticate(username, password);

                if (user != null) // login successful
                {
                    if (!user.role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase)) // user is not an admin, so display customer menu
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
