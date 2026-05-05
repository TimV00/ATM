namespace ATM.Tests.Domain;
using model;

public class UserTests
{
    // Valid creation
    [Fact]
    public void Create_ValidInput_ReturnsUser()
    {
        var user = User.Create(1, "testuser", 12345, "Customer");
        Assert.Equal(1, user.user_id);
        Assert.Equal("testuser", user.username);
        Assert.Equal(12345, user.password);
        Assert.Equal("Customer", user.role);
    }

    // Username validation
    [Fact]
    public void Create_EmptyUsername_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => User.Create(1, "", 12345, "Customer"));
    }

    [Fact]
    public void Create_WhitespaceUsername_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => User.Create(1, "   ", 12345, "Customer"));
    }

    // PIN validation
    [Fact]
    public void Create_PinTooShort_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => User.Create(1, "testuser", 1234, "Customer"));
    }

    [Fact]
    public void Create_PinTooLong_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => User.Create(1, "testuser", 123456, "Customer"));
    }

    [Fact]
    public void Create_PinAtLowerBound_ReturnsUser()
    {
        var user = User.Create(1, "testuser", 10000, "Customer");
        Assert.Equal(10000, user.password);
    }

    [Fact]
    public void Create_PinAtUpperBound_ReturnsUser()
    {
        var user = User.Create(1, "testuser", 99999, "Customer");
        Assert.Equal(99999, user.password);
    }

    // Role validation
    [Fact]
    public void Create_EmptyRole_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => User.Create(1, "testuser", 12345, ""));
    }

    [Fact]
    public void Create_WhitespaceRole_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => User.Create(1, "testuser", 12345, "   "));
    }

    // UpdateUsername
    [Fact]
    public void UpdateUsername_ValidInput_UpdatesUsername()
    {
        var user = User.Create(1, "testuser", 12345, "Customer");
        user.UpdateUsername("newuser");
        Assert.Equal("newuser", user.username);
    }

    [Fact]
    public void UpdateUsername_EmptyInput_ThrowsArgumentException()
    {
        var user = User.Create(1, "testuser", 12345, "Customer");
        Assert.Throws<ArgumentException>(() => user.UpdateUsername(""));
    }

    [Fact]
    public void UpdateUsername_WhitespaceInput_ThrowsArgumentException()
    {
        var user = User.Create(1, "testuser", 12345, "Customer");
        Assert.Throws<ArgumentException>(() => user.UpdateUsername("   "));
    }

    // UpdatePassword
    [Fact]
    public void UpdatePassword_ValidPin_UpdatesPassword()
    {
        var user = User.Create(1, "testuser", 12345, "Customer");
        user.UpdatePassword(54321);
        Assert.Equal(54321, user.password);
    }

    [Fact]
    public void UpdatePassword_PinTooShort_ThrowsArgumentException()
    {
        var user = User.Create(1, "testuser", 12345, "Customer");
        Assert.Throws<ArgumentException>(() => user.UpdatePassword(1234));
    }

    [Fact]
    public void UpdatePassword_PinTooLong_ThrowsArgumentException()
    {
        var user = User.Create(1, "testuser", 12345, "Customer");
        Assert.Throws<ArgumentException>(() => user.UpdatePassword(123456));
    }
}