namespace dal;
using System.Data;
using MySql.Data.MySqlClient;
public class UserDal
{
    private const string connectionString = "server=host.docker.internal;port=3333;uid=root;pwd=pass;database=ATM";

    public static DataTable GetAll()
    {
        var dt = new DataTable();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (var da = new MySqlDataAdapter(@"select * from users;", connection))
            {
                da.Fill(dt);
            }
        }
        return dt;
    }
    public static DataTable GetBy(string username)
    {
        var dt = new DataTable();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (var da = new MySqlDataAdapter(@"select * from users where username='" + username.ToString() + "';", connection))
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
            using (var da = new MySqlDataAdapter(@"select * from users where user_id=" + id.ToString() + ";", connection))
            {
                da.Fill(dt);
            }
        }
        return dt;
    }
    public static int Create(
        string username,
        int password,
        string role)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(@"
            INSERT INTO users
                (username, password, role)
            VALUES
                (@username, @password, @role);
            SELECT LAST_INSERT_ID();
        ", connection);

        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);
        cmd.Parameters.AddWithValue("@role", role);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }
    public static int Update(
        int user_id,
        string username,
        int password)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();

        using var cmd = new MySqlCommand(@"
            UPDATE users
            SET username = @username,
                password = @password
            WHERE user_id = @user_id;
        ", connection);

        cmd.Parameters.AddWithValue("@user_id", user_id);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);

        return cmd.ExecuteNonQuery();
    }
    public static int DeleteUser(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();

        using var cmd = new MySqlCommand("delete from users where user_id = @id;", connection);
        cmd.Parameters.AddWithValue("@id", id);

        return cmd.ExecuteNonQuery();
    }
}
