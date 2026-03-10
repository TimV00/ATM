namespace service;

using System.Data.Common;
using dal;
using model;
using System.Data;
public class AuthService
{
    public static User Authenticate(string username, int password)
    {
        var user = UserModel.GetBy(username);
        if (user == null)
        {
            Console.Clear();
            Console.WriteLine("Invalid Username. Please try again");
            return null;
        }
        if (user.password != password) //passwords don't match
        {
            Console.Clear();
            Console.WriteLine("Incorrect Password. Please try again");
            return null;
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Login successful!");
            return user;
        }
    }
}
