using MySql.Data.MySqlClient;
using System.Data;
using service;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            var dt = new DataTable();
            string connectionString = "server=host.docker.internal;port=3333;uid=root;pwd=pass;database=ATM;";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query1 = "select * from users limit 1;";
                using (var command = new MySqlCommand(query1, connection))
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object value = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i);
                            Console.Write(value);

                            if (i < reader.FieldCount - 1)
                                Console.Write(" | ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}