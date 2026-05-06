namespace dal;
using System.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics.CodeAnalysis;


/// <summary>
/// Defines the contract for user data access operations.
/// </summary>
public interface IUserDal
{
    /// <summary>Retrieves all users from the database.</summary>
    /// <returns>A <see cref="DataTable"/> containing all user records.</returns>
    DataTable GetAll();

    /// <summary>Retrieves a user by username.</summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>A <see cref="DataTable"/> containing the matching user record, or empty if not found.</returns>
    DataTable GetBy(string username);

    /// <summary>Retrieves a user by their numeric ID.</summary>
    /// <param name="id">The user ID to search for.</param>
    /// <returns>A <see cref="DataTable"/> containing the matching user record, or empty if not found.</returns>
    DataTable GetBy(int id);

    /// <summary>Inserts a new user into the database.</summary>
    /// <param name="username">The username for the new user.</param>
    /// <param name="password">The 5-digit PIN for the new user.</param>
    /// <param name="role">The role assigned to the new user.</param>
    /// <returns>The ID of the newly created user.</returns>
    int Create(string username, int password, string role);

    /// <summary>Updates an existing user's username and PIN.</summary>
    /// <param name="user_id">The ID of the user to update.</param>
    /// <param name="username">The new username.</param>
    /// <param name="password">The new 5-digit PIN.</param>
    /// <returns>The number of rows affected.</returns>
    int Update(int user_id, string username, int password);

    /// <summary>Deletes a user from the database by ID.</summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>The number of rows affected.</returns>
    int DeleteUser(int id);
}

[ExcludeFromCodeCoverage]
public class UserDal : IUserDal
{
    private readonly string _connectionString;

    public UserDal(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DataTable GetAll()
    {
        var dt = new DataTable();
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand("SELECT * FROM users;", connection);
        using var da = new MySqlDataAdapter(cmd);
        da.Fill(dt);
        return dt;
    }

    public DataTable GetBy(string username)
    {
        var dt = new DataTable();
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand("SELECT * FROM users WHERE username = @username;", connection);
        cmd.Parameters.AddWithValue("@username", username);
        using var da = new MySqlDataAdapter(cmd);
        da.Fill(dt);
        return dt;
    }

    public DataTable GetBy(int id)
    {
        var dt = new DataTable();
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand("SELECT * FROM users WHERE user_id = @id;", connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var da = new MySqlDataAdapter(cmd);
        da.Fill(dt);
        return dt;
    }

    public int Create(string username, int password, string role)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand(@"
            INSERT INTO users (username, password, role)
            VALUES (@username, @password, @role);
            SELECT LAST_INSERT_ID();
        ", connection);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);
        cmd.Parameters.AddWithValue("@role", role);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public int Update(int user_id, string username, int password)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand(@"
            UPDATE users
            SET username = @username, password = @password
            WHERE user_id = @user_id;
        ", connection);
        cmd.Parameters.AddWithValue("@user_id", user_id);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);
        return cmd.ExecuteNonQuery();
    }

    public int DeleteUser(int id)
    {
        using var connection = new MySqlConnection(_connectionString);
        connection.Open();
        using var cmd = new MySqlCommand("DELETE FROM users WHERE user_id = @id;", connection);
        cmd.Parameters.AddWithValue("@id", id);
        return cmd.ExecuteNonQuery();
    }
}
