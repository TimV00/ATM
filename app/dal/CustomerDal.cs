namespace dal;
using System.Data;
using MySql.Data.MySqlClient;

public interface ICustomerDal
{
    DataTable GetAll();
    DataTable GetBy(int id);
    DataTable GetByUserID(int id);
    int Create(int user_id, string customer_name, decimal balance, string status);
    int Update(int customer_id, string customer_name, decimal balance, string status);
    int DeleteCustomer(int id);
}

public class CustomerDal : ICustomerDal
{
    private readonly string _connectionString;

    public CustomerDal(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DataTable GetAll()
    {
        var dt = new DataTable();
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand("SELECT * FROM customers;", connection);
        using var da = new MySqlDataAdapter(cmd);
        da.Fill(dt);
        return dt;
    }

    public DataTable GetBy(int id)
    {
        var dt = new DataTable();
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand("SELECT * FROM customers WHERE customer_id = @id;", connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var da = new MySqlDataAdapter(cmd);
        da.Fill(dt);
        return dt;
    }

    public DataTable GetByUserID(int id)
    {
        var dt = new DataTable();
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand("SELECT * FROM customers WHERE user_id = @id;", connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var da = new MySqlDataAdapter(cmd);
        da.Fill(dt);
        return dt;
    }

    public int Create(int user_id, string customer_name, decimal balance, string status)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand(@"
            INSERT INTO customers (user_id, customer_name, balance, status)
            VALUES (@user_id, @customer_name, @balance, @status);
            SELECT LAST_INSERT_ID();
        ", connection);
        cmd.Parameters.AddWithValue("@user_id", user_id);
        cmd.Parameters.AddWithValue("@customer_name", customer_name);
        cmd.Parameters.AddWithValue("@balance", balance);
        cmd.Parameters.AddWithValue("@status", status);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public int Update(int customer_id, string customer_name, decimal balance, string status)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand(@"
            UPDATE customers
            SET customer_name = @customer_name, balance = @balance, status = @status
            WHERE customer_id = @customer_id;
        ", connection);
        cmd.Parameters.AddWithValue("@customer_id", customer_id);
        cmd.Parameters.AddWithValue("@customer_name", customer_name);
        cmd.Parameters.AddWithValue("@balance", balance);
        cmd.Parameters.AddWithValue("@status", status);
        return cmd.ExecuteNonQuery();
    }

    public int DeleteCustomer(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand("DELETE FROM customers WHERE customer_id = @id;", connection);
        cmd.Parameters.AddWithValue("@id", id);
        return cmd.ExecuteNonQuery();
    }
}