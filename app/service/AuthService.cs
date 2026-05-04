namespace service;

using dal;
using model;
using util;

public class AuthService
{
    public static User? Authenticate(string username, int password)
    {
        var user = UserModel.GetBy(username);
        if (user == null)
        {
            Console.Clear();
            Console.WriteLine("Invalid Username. Please try again");
            return null;
        }
        if (user.password != password)
        {
            Console.Clear();
            Console.WriteLine("Incorrect Password. Please try again");
            return null;
        }

        Console.Clear();
        Console.WriteLine("Login successful!");
        return user;
    }

    private static bool RouteUser(User user)
    {
        if (!user.role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase))
            return CustomerService.DisplayCustomerMenu(user);
        else
            return AdminService.DisplayAdminMenu(user);
    }

    public static void Run()
    {
        Console.Clear();
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("------ ATM Login -----");

            string username = InputHelper.ReadString("Enter Username: ");
            int password = InputHelper.ReadPin("Enter Pin code: ");

            var user = Authenticate(username, password);
            if (user != null)
                exit = RouteUser(user);
        }
    }
}