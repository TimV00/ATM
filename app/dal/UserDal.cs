namespace dal;
using System.Data;
using MySql.Data.MySqlClient;

public interface IUserDal
{
    DataTable GetAll();
    DataTable GetBy(string username);
    DataTable GetBy(int id);
    int Create(string username, int password, string role);
    int Update(int user_id, string username, int password);
    int DeleteUser(int id);
}

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