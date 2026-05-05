using dal;
using model;
using service;
using Microsoft.Extensions.Configuration;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                string connectionString = config.GetConnectionString("ATM")
                    ?? throw new InvalidOperationException("Connection string 'ATM' not found in appsettings.json.");

                // DAL layer
                var userDal = new UserDal(connectionString);
                var customerDal = new CustomerDal(connectionString);

                // Model layer
                var userModel = new UserModel(userDal);
                var customerModel = new CustomerModel(customerDal);

                // Service layer
                var adminService = new AdminService(userModel, customerModel);
                var customerService = new CustomerService(customerModel);
                var authService = new AuthService(userModel, adminService, customerService);

                authService.Run();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}