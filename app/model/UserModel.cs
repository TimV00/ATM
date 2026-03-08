namespace model;
using System.Data.Common;
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
    // Get all users
    public static List<User> GetAll()
    {
        var users = new List<User>();
        var dt = UserDal.GetAll();
        foreach (DataRow r in dt.Rows)
        {
            users.Add(new User
            {
                user_id = (int)r["user_id"],
                username = (string)r["username"],
                password = (int)r["password"],
                role = r["role"] == DBNull.Value ? null : r["role"].ToString(),
            });
        }

        return users;
    }

    public static User GetBy(int id)
    {
        var dt = UserDal.GetBy(id);

        // check if we are trying to get an user that doesn't exist
        if (dt == null || dt.Rows.Count == 0)
            return null;

        var r = dt.Rows[0];


        var user = new User
        {
                user_id = (int)r["user_id"],
                username = (string)r["username"],
                password = (int)r["password"],
                role = r["role"] == DBNull.Value ? null : r["role"].ToString(),
        };
        return user;
    }
    public static int Create(User user)
    {
        var newId = UserDal.Create(
            user.user_id!,
            user.username!,
            user.password!,
            user.role ?? ""
        );

        return newId;
    }
    public static int Update(User user)
    {
        var rows = UserDal.Update(
            user.user_id!,
            user.username!,
            user.password!
        );

        return rows;
    }
    public static int DeleteUser(int id)
    {
        return UserDal.DeleteUser(id);
    }
}
