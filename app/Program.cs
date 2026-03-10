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

                string passwordstr;
                while (true)
                {
                    Console.Write("Enter Pin code: ");
                    passwordstr = Console.ReadLine();

                    if (passwordstr.Length == 5 && int.TryParse(passwordstr, out _)) //Check if pin is a 5 digit integer
                        break;

                    Console.WriteLine("Pin must be 5 digits. Please try again");
                }

                int password = int.Parse(passwordstr);

                var user = AuthService.Authenticate(username, password);

                if (user != null) // login successful
                {
                    if (user.role.ToUpper() != "ADMIN") // customer menu
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
