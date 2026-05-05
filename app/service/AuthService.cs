namespace service;
using model;
using util;

public interface IAuthService
{
    void Run();
}

public class AuthService : IAuthService
{
    private readonly UserModel _userModel;
    private readonly IAdminService _adminService;
    private readonly ICustomerService _customerService;

    public AuthService(UserModel userModel, IAdminService adminService, ICustomerService customerService)
    {
        _userModel = userModel;
        _adminService = adminService;
        _customerService = customerService;
    }

    public User? Authenticate(string username, int password)
    {
        var user = _userModel.GetBy(username);
        if (user == null)
        {
            Console.Clear();
            Console.WriteLine("Invalid Username. Please try again");
            return null;
        }
        if (user.password != password)
        {
            Console.Clear();
            Console.WriteLine("Incorrect Password. Please try again");
            return null;
        }
        Console.Clear();
        Console.WriteLine("Login successful!");
        return user;
    }

    private bool RouteUser(User user)
    {
        if (!user.role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase))
            return _customerService.DisplayCustomerMenu(user);
        else
            return _adminService.DisplayAdminMenu(user);
    }

    public void Run()
    {
        Console.Clear();
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("------ ATM Login -----");
            string username = InputHelper.ReadString("Enter Username: ");
            int password = InputHelper.ReadPin("Enter Pin code: ");
            var user = Authenticate(username, password);
            if (user != null)
                exit = RouteUser(user);
        }
    }
}