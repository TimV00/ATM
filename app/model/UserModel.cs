namespace model;

using dal;
using System.Data;

/// <summary>
/// Represents a user account in the ATM system.
/// </summary>
public class User
{
    /// <summary>Gets the unique identifier for the user.</summary>
    public int user_id { get; private set; }
    /// <summary>Gets the username of the user.</summary>
    public string? username { get; private set; }
    /// <summary>Gets the 5-digit PIN used for authentication.</summary>
    public int password { get; private set; }
    /// <summary>Gets the role assigned to the user (e.g. "Customer", "ADMIN").</summary>
    public string? role { get; private set; }

    private User() { }
    /// <summary>
    /// Creates a new <see cref="User"/> instance with validated input.
    /// </summary>
    /// <param name="user_id">The unique identifier for the user.</param>
    /// <param name="username">The username. Cannot be empty or whitespace.</param>
    /// <param name="password">The 5-digit PIN. Must be between 10000 and 99999.</param>
    /// <param name="role">The role assigned to the user. Can be null.</param>
    /// <returns>A new <see cref="User"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when username is empty or PIN is not 5 digits.</exception>
    public static User Create(int user_id, string username, int password, string? role)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.");
        if (password < 10000 || password > 99999)
            throw new ArgumentException("PIN must be a 5-digit number (10000–99999).");

        return new User
        {
            user_id = user_id,
            username = username,
            password = password,
            role = role
        };
    }
    /// <summary>
    /// Updates the username.
    /// </summary>
    /// <param name="username">The new username. Cannot be empty or whitespace.</param>
    /// <exception cref="ArgumentException">Thrown when username is empty or whitespace.</exception>
    public void UpdateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.");
        this.username = username;
    }

    /// <summary>
    /// Updates the PIN.
    /// </summary>
    /// <param name="password">The new 5-digit PIN. Must be between 10000 and 99999.</param>
    /// <exception cref="ArgumentException">Thrown when PIN is not a 5-digit number.</exception>
    public void UpdatePassword(int password)
    {
        if (password < 10000 || password > 99999)
            throw new ArgumentException("PIN must be a 5-digit number (10000–99999).");
        this.password = password;
    }

}
/// <summary>
/// Provides data access operations for <see cref="User"/> objects.
/// </summary>
public class UserModel
{
    private readonly IUserDal _dal;

    /// <summary>
    /// Initializes a new instance of <see cref="UserModel"/> with the specified DAL.
    /// </summary>
    /// <param name="dal">The data access layer for user operations.</param>
    public UserModel(IUserDal dal)
    {
        _dal = dal;
    }

    /// <summary>
    /// Retrieves all users from the database.
    /// </summary>
    /// <returns>A list of all <see cref="User"/> objects.</returns>
    public List<User> GetAll()
    {
        var users = new List<User>();
        var dt = _dal.GetAll();
        foreach (DataRow r in dt.Rows)
            users.Add(MapRow(r));
        return users;
    }

    /// <summary>
    /// Retrieves a user by their numeric ID.
    /// </summary>
    /// <param name="id">The user ID to search for.</param>
    /// <returns>The matching <see cref="User"/>, or null if not found.</returns>
    public User? GetBy(int id)
    {
        var dt = _dal.GetBy(id);
        if (dt == null || dt.Rows.Count == 0) return null;
        return MapRow(dt.Rows[0]);
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>The matching <see cref="User"/>, or null if not found.</returns>
    public User? GetBy(string username)
    {
        var dt = _dal.GetBy(username);
        if (dt == null || dt.Rows.Count == 0) return null;
        return MapRow(dt.Rows[0]);
    }

    /// <summary>
    /// Inserts a new user into the database.
    /// </summary>
    /// <param name="user">The <see cref="User"/> to create.</param>
    /// <returns>The ID of the newly created user.</returns>
    public int Create(User user)
    {
        return _dal.Create(user.username!, user.password, user.role ?? "");
    }

    /// <summary>
    /// Updates an existing user's username and PIN in the database.
    /// </summary>
    /// <param name="user">The <see cref="User"/> with updated values.</param>
    /// <returns>The number of rows affected.</returns>
    public int Update(User user)
    {
        return _dal.Update(user.user_id, user.username!, user.password);
    }

    /// <summary>
    /// Deletes a user from the database by ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public int DeleteUser(int id)
    {
        return _dal.DeleteUser(id);
    }

    private static User MapRow(DataRow r) => User.Create(
        (int)r["user_id"],
        (string)r["username"],
        (int)r["password"],
        r["role"] == DBNull.Value ? null : r["role"].ToString()
    );
}
