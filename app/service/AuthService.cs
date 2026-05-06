namespace service;
using model;
using util;

/// <summary>
/// Defines the contract for the authentication service.
/// </summary>
public interface IAuthService
{
    /// <summary>Starts the main ATM login loop.</summary>
    /// <returns>A boolean indicating whether the user has chosen to exit the application.</returns>
    void Run();
}

/// <summary>
/// Handles user authentication and routes users to the appropriate menu.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserModel _userModel;
    private readonly IAdminService _adminService;
    private readonly ICustomerService _customerService;

    /// <summary>
    /// Initializes a new instance of <see cref="AuthService"/>.
    /// </summary>
    /// <param name="userModel">The user model for database access.</param>
    /// <param name="adminService">The admin menu service.</param>
    /// <param name="customerService">The customer menu service.</param>
    public AuthService(UserModel userModel, IAdminService adminService, ICustomerService customerService)
    {
        _userModel = userModel;
        _adminService = adminService;
        _customerService = customerService;
    }

    /// <summary>
    /// Authenticates a user by username and PIN.
    /// </summary>
    /// <param name="username">The username to authenticate.</param>
    /// <param name="password">The PIN to verify.</param>
    /// <returns>The authenticated <see cref="User"/>, or null if authentication fails.</returns>
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
        if (!string.Equals(user.role, "ADMIN", StringComparison.OrdinalIgnoreCase))
            return _customerService.DisplayCustomerMenu(user);
        else
            return _adminService.DisplayAdminMenu(user);
    }

    /// <summary>
    /// Starts the main ATM login loop, prompting for credentials until the user exits.
    /// </summary>
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
