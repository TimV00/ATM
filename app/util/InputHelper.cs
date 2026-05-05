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
                string input = Console.ReadLine() ?? string.Empty;

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
                string input = Console.ReadLine() ?? string.Empty;

                if (decimal.TryParse(input, out value) && value > 0)
                    return value;

                Console.WriteLine("Balance must be a valid amount greater than zero.");
            }
        }

        public static decimal ReadCashAmount(string prompt)
        {
            decimal value;

            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                if (decimal.TryParse(input, out value) && value > 0)
                    return value;

                Console.WriteLine("Deposit amount must be a valid amount greater than zero.");
            }
        }

        public static string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.WriteLine("Input cannot be blank.");
            }
        }

        public static string ReadStatus(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;
                if (input.Equals("Active", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                    return input;
                Console.WriteLine("Invalid status. Please enter 'Active' or 'Inactive'.");
            }
        }

        public static int ReadPin(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                if (input.Length == 5 && int.TryParse(input, out int pin))
                    return pin;

                Console.WriteLine("PIN must be a 5 digit number.");
            }
        }

        public static bool ConfirmId(string prompt, int expectedId)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;

            if (int.TryParse(input, out int confirmId))
                return confirmId == expectedId;

            return false;
        }
        public static string ReadStringOrSkip(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;
            return string.IsNullOrWhiteSpace(input) ? null : input;
        }

        public static string ReadStatusOrSkip(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(input)) return null;
            while (!input.Equals("Active", StringComparison.OrdinalIgnoreCase) &&
                   !input.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Invalid status. Please enter 'Active' or 'Inactive'.");
                Console.Write(prompt);
                input = Console.ReadLine() ?? string.Empty;
            }
            return input;
        }

        public static int? ReadPinOrSkip(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(input)) return null;
            while (input.Length != 5 || !int.TryParse(input, out _))
            {
                Console.WriteLine("PIN must be a 5 digit number.");
                Console.Write(prompt);
                input = Console.ReadLine() ?? string.Empty;
            }
            return int.Parse(input);
        }


    }
}