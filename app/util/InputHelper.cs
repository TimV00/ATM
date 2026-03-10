namespace util
{
    public static class InputHelper
    {
        public static int ReadID(string prompt)
        {
            int value;

            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out value))
                    return value;

                Console.WriteLine("Invalid number. Please try again.");
            }
        }

        public static decimal ReadBalance(string prompt)
        {
            decimal value;

            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (decimal.TryParse(input, out value) && value > 0)
                    return value;

                Console.WriteLine("Invalid amount. Please enter a positive number.");
            }
        }

        public static string ReadUsername(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.WriteLine("Input cannot be blank.");
            }
        }

        public static int ReadPin(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (input.Length == 5 && int.TryParse(input, out int pin))
                    return pin;

                Console.WriteLine("PIN must be a 5 digit number.");
            }
        }

        public static bool ConfirmId(string prompt, int expectedId)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (int.TryParse(input, out int confirmId))
                return confirmId == expectedId;

            return false;
        }
    }
}