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
            var users = UserModel.GetAll();

            foreach (var user in users)
            {
                Console.WriteLine($"{user.user_id} | {user.username} | {user.role}");
            }
        }
    }
}
