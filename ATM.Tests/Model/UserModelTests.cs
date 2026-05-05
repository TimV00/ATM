namespace ATM.Tests.Model;
using Moq;
using model;
using dal;
using System.Data;

public class UserModelTests
{
    private readonly Mock<IUserDal> _mockDal;
    private readonly UserModel _userModel;

    public UserModelTests()
    {
        _mockDal = new Mock<IUserDal>();
        _userModel = new UserModel(_mockDal.Object);
    }

    private static DataTable BuildUserTable(int user_id, string username, int password, string role)
    {
        var dt = new DataTable();
        dt.Columns.Add("user_id", typeof(int));
        dt.Columns.Add("username", typeof(string));
        dt.Columns.Add("password", typeof(int));
        dt.Columns.Add("role", typeof(string));
        dt.Rows.Add(user_id, username, password, role);
        return dt;
    }

    [Fact]
    public void GetBy_String_ReturnsUser_WhenFound()
    {
        _mockDal.Setup(d => d.GetBy("testuser")).Returns(BuildUserTable(1, "testuser", 12345, "Customer"));
        var user = _userModel.GetBy("testuser");
        Assert.NotNull(user);
        Assert.Equal("testuser", user.username);
    }

    [Fact]
    public void GetBy_String_ReturnsNull_WhenNotFound()
    {
        _mockDal.Setup(d => d.GetBy("nobody")).Returns(new DataTable());
        var user = _userModel.GetBy("nobody");
        Assert.Null(user);
    }

    [Fact]
    public void GetBy_Int_ReturnsUser_WhenFound()
    {
        _mockDal.Setup(d => d.GetBy(1)).Returns(BuildUserTable(1, "testuser", 12345, "Customer"));
        var user = _userModel.GetBy(1);
        Assert.NotNull(user);
        Assert.Equal(1, user.user_id);
    }

    [Fact]
    public void GetBy_Int_ReturnsNull_WhenNotFound()
    {
        _mockDal.Setup(d => d.GetBy(99)).Returns(new DataTable());
        var user = _userModel.GetBy(99);
        Assert.Null(user);
    }

    [Fact]
    public void Create_CallsDalWithCorrectParameters()
    {
        var user = User.Create(0, "testuser", 12345, "Customer");
        _mockDal.Setup(d => d.Create("testuser", 12345, "Customer")).Returns(1);
        var result = _userModel.Create(user);
        Assert.Equal(1, result);
        _mockDal.Verify(d => d.Create("testuser", 12345, "Customer"), Times.Once);
    }

    [Fact]
    public void Update_CallsDalWithCorrectParameters()
    {
        var user = User.Create(1, "testuser", 12345, "Customer");
        _mockDal.Setup(d => d.Update(1, "testuser", 12345)).Returns(1);
        var result = _userModel.Update(user);
        Assert.Equal(1, result);
        _mockDal.Verify(d => d.Update(1, "testuser", 12345), Times.Once);
    }

    [Fact]
    public void DeleteUser_CallsDalWithCorrectId()
    {
        _mockDal.Setup(d => d.DeleteUser(1)).Returns(1);
        var result = _userModel.DeleteUser(1);
        Assert.Equal(1, result);
        _mockDal.Verify(d => d.DeleteUser(1), Times.Once);
    }

    [Fact]
    public void GetAll_ReturnsAllUsers()
    {
        var dt = new DataTable();
        dt.Columns.Add("user_id", typeof(int));
        dt.Columns.Add("username", typeof(string));
        dt.Columns.Add("password", typeof(int));
        dt.Columns.Add("role", typeof(string));
        dt.Rows.Add(1, "user1", 12345, "Customer");
        dt.Rows.Add(2, "user2", 54321, "Customer");
        _mockDal.Setup(d => d.GetAll()).Returns(dt);
        var users = _userModel.GetAll();
        Assert.Equal(2, users.Count);
    }
}