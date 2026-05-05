using dal;
using model;
using service;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                const string connectionString = "server=host.docker.internal;port=3333;uid=root;pwd=pass;database=ATM";

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