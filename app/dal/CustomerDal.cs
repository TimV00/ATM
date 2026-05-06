namespace dal;
using System.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Defines the contract for customer data access operations.
/// </summary>
public interface ICustomerDal
{
    /// <summary>Retrieves all customers from the database.</summary>
    /// <returns>A <see cref="DataTable"/> containing all customer records.</returns>
    DataTable GetAll();

    /// <summary>Retrieves a customer by their numeric ID.</summary>
    /// <param name="id">The customer ID to search for.</param>
    /// <returns>A <see cref="DataTable"/> containing the matching customer record, or empty if not found.</returns>
    DataTable GetBy(int id);

    /// <summary>Retrieves a customer by their associated user ID.</summary>
    /// <param name="id">The user ID to search for.</param>
    /// <returns>A <see cref="DataTable"/> containing the matching customer record, or empty if not found.</returns>
    DataTable GetByUserID(int id);

    /// <summary>Inserts a new customer into the database.</summary>
    /// <param name="user_id">The associated user ID.</param>
    /// <param name="customer_name">The customer's name.</param>
    /// <param name="balance">The starting balance.</param>
    /// <param name="status">The account status.</param>
    /// <returns>The ID of the newly created customer.</returns>
    int Create(int user_id, string customer_name, decimal balance, string status);

    /// <summary>Updates an existing customer's information in the database.</summary>
    /// <param name="customer_id">The ID of the customer to update.</param>
    /// <param name="customer_name">The new customer name.</param>
    /// <param name="balance">The new balance.</param>
    /// <param name="status">The new status.</param>
    /// <returns>The number of rows affected.</returns>
    int Update(int customer_id, string customer_name, decimal balance, string status);

    /// <summary>Deletes a customer from the database by ID.</summary>
    /// <param name="id">The ID of the customer to delete.</param>
    /// <returns>The number of rows affected.</returns>
    int DeleteCustomer(int id);
}
[ExcludeFromCodeCoverage]
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
