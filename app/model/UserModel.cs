namespace model;

using dal;
using System.Data;

public class User
{
    public int user_id { get; private set; }
    public string username { get; private set; }
    public int password { get; private set; }
    public string role { get; private set; }

    private User() { }

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
    public void UpdateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.");
        this.username = username;
    }

    public void UpdatePassword(int password)
    {
        if (password < 10000 || password > 99999)
            throw new ArgumentException("PIN must be a 5-digit number (10000–99999).");
        this.password = password;
    }

}
public class UserModel
{
    private readonly IUserDal _dal;

    public UserModel(IUserDal dal)
    {
        _dal = dal;
    }

    public List<User> GetAll()
    {
        var users = new List<User>();
        var dt = _dal.GetAll();
        foreach (DataRow r in dt.Rows)
            users.Add(MapRow(r));
        return users;
    }

    public User? GetBy(int id)
    {
        var dt = _dal.GetBy(id);
        if (dt == null || dt.Rows.Count == 0) return null;
        return MapRow(dt.Rows[0]);
    }

    public User? GetBy(string username)
    {
        var dt = _dal.GetBy(username);
        if (dt == null || dt.Rows.Count == 0) return null;
        return MapRow(dt.Rows[0]);
    }

    public int Create(User user)
    {
        return _dal.Create(user.username!, user.password, user.role ?? "");
    }

    public int Update(User user)
    {
        return _dal.Update(user.user_id, user.username!, user.password);
    }

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