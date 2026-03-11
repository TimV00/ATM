namespace dal;
using System.Data;
using MySql.Data.MySqlClient;
public class CustomerDal
{
    private const string connectionString = "server=host.docker.internal;port=3333;uid=root;pwd=pass;database=ATM";

    public static DataTable GetAll()
    {
        var dt = new DataTable();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (var da = new MySqlDataAdapter(@"select * from customers;", connection))
            {
                da.Fill(dt);
            }
        }
        return dt;
    }
    public static DataTable GetBy(int id)
    {
        var dt = new DataTable();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (var da = new MySqlDataAdapter(@"select * from customers where customer_id=" + id.ToString() + ";", connection))
            {
                da.Fill(dt);
            }
        }
        return dt;
    }

    public static DataTable GetByUserID(int id)
    {
        var dt = new DataTable();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (var da = new MySqlDataAdapter(@"select * from customers where user_id=" + id.ToString() + ";", connection))
            {
                da.Fill(dt);
            }
        }
        return dt;
    }

    public static int Create(
        int user_id,
        string customer_name,
        decimal balance,
        string status)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(@"
            INSERT INTO customers
                (user_id, customer_name, balance, status)
            VALUES
                (@user_id, @customer_name, @balance, @status);
            SELECT LAST_INSERT_ID();
        ", connection);

        cmd.Parameters.AddWithValue("@user_id", user_id);
        cmd.Parameters.AddWithValue("@customer_name", customer_name);
        cmd.Parameters.AddWithValue("@balance", balance);
        cmd.Parameters.AddWithValue("@status", status);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }
    public static int Update(
        int customer_id,
        string customer_name,
        decimal balance,
        string status)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(@"
            UPDATE customers
            SET customer_name = @customer_name,
                balance = @balance,
                status = @status
            WHERE customer_id = @customer_id;
        ", connection);

        cmd.Parameters.AddWithValue("@customer_id", customer_id);
        cmd.Parameters.AddWithValue("@customer_name", customer_name);
        cmd.Parameters.AddWithValue("@balance", balance);
        cmd.Parameters.AddWithValue("@status", status);

        return cmd.ExecuteNonQuery();
    }
    public static int DeleteCustomer(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();

        using var cmd = new MySqlCommand("delete from customers where customer_id = @id;", connection);
        cmd.Parameters.AddWithValue("@id", id);

        return cmd.ExecuteNonQuery();
    }
}
