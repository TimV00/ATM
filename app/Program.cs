using service;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AuthService.Run();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}