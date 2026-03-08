namespace service;

using System.Data.Common;
using dal;
using model;
using System.Data;
public class AuthService
{
    public static User Authenticate(string username, int password)
    {
        var user = UserModel.GetBy(username);
        if (user == null)
            return null;

        if (user.password != password) // simplified
            return null;

        return user;
    }
}
