namespace model;
using dal;
using System.Data;

public class User
{
    public int user_id { get; set; }
    public string username { get; set; }
    public int password { get; set; }
    public string role { get; set; }
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

    private static User MapRow(DataRow r) => new User
    {
        user_id = (int)r["user_id"],
        username = (string)r["username"],
        password = (int)r["password"],
        role = r["role"] == DBNull.Value ? null : r["role"].ToString(),
    };
}