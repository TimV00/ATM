namespace ATM.Tests.Domain;
using model;

public class CustomerTests
{
    // Valid creation
    [Fact]
    public void Create_ValidInput_ReturnsCustomer()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        Assert.Equal(1, customer.customer_id);
        Assert.Equal(1, customer.user_id);
        Assert.Equal("John Doe", customer.customer_name);
        Assert.Equal(100.00m, customer.balance);
        Assert.Equal("Active", customer.status);
    }

    // Name validation
    [Fact]
    public void Create_EmptyName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Customer.Create(1, 1, "", 100.00m, "Active"));
    }

    [Fact]
    public void Create_WhitespaceName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Customer.Create(1, 1, "   ", 100.00m, "Active"));
    }

    // Balance validation
    [Fact]
    public void Create_NegativeBalance_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Customer.Create(1, 1, "John Doe", -1.00m, "Active"));
    }

    [Fact]
    public void Create_ZeroBalance_ReturnsCustomer()
    {
        var customer = Customer.Create(1, 1, "John Doe", 0.00m, "Active");
        Assert.Equal(0.00m, customer.balance);
    }

    // Status validation
    [Fact]
    public void Create_ActiveStatus_ReturnsCustomer()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        Assert.Equal("Active", customer.status);
    }

    [Fact]
    public void Create_InactiveStatus_ReturnsCustomer()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Inactive");
        Assert.Equal("Inactive", customer.status);
    }

    [Fact]
    public void Create_InvalidStatus_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Customer.Create(1, 1, "John Doe", 100.00m, "Suspended"));
    }

    [Fact]
    public void Create_StatusCaseInsensitive_ReturnsCustomer()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "active");
        Assert.Equal("active", customer.status);
    }

    // Withdraw
    [Fact]
    public void Withdraw_ValidAmount_UpdatesBalance()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        customer.Withdraw(50.00m);
        Assert.Equal(50.00m, customer.balance);
    }

    [Fact]
    public void Withdraw_ExactBalance_UpdatesBalanceToZero()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        customer.Withdraw(100.00m);
        Assert.Equal(0.00m, customer.balance);
    }

    [Fact]
    public void Withdraw_AmountExceedsBalance_ThrowsInvalidOperationException()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        Assert.Throws<InvalidOperationException>(() => customer.Withdraw(150.00m));
    }

    [Fact]
    public void Withdraw_ZeroAmount_ThrowsArgumentException()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        Assert.Throws<ArgumentException>(() => customer.Withdraw(0.00m));
    }

    [Fact]
    public void Withdraw_NegativeAmount_ThrowsArgumentException()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        Assert.Throws<ArgumentException>(() => customer.Withdraw(-50.00m));
    }

    // Deposit
    [Fact]
    public void Deposit_ValidAmount_UpdatesBalance()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        customer.Deposit(50.00m);
        Assert.Equal(150.00m, customer.balance);
    }

    [Fact]
    public void Deposit_ZeroAmount_ThrowsArgumentException()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        Assert.Throws<ArgumentException>(() => customer.Deposit(0.00m));
    }

    [Fact]
    public void Deposit_NegativeAmount_ThrowsArgumentException()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        Assert.Throws<ArgumentException>(() => customer.Deposit(-50.00m));
    }

    // UpdateName
    [Fact]
    public void UpdateName_ValidInput_UpdatesName()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        customer.UpdateName("Jane Doe");
        Assert.Equal("Jane Doe", customer.customer_name);
    }

    [Fact]
    public void UpdateName_EmptyInput_ThrowsArgumentException()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        Assert.Throws<ArgumentException>(() => customer.UpdateName(""));
    }

    [Fact]
    public void UpdateName_WhitespaceInput_ThrowsArgumentException()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        Assert.Throws<ArgumentException>(() => customer.UpdateName("   "));
    }

    // UpdateStatus
    [Fact]
    public void UpdateStatus_ValidStatus_UpdatesStatus()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        customer.UpdateStatus("Inactive");
        Assert.Equal("Inactive", customer.status);
    }

    [Fact]
    public void UpdateStatus_InvalidStatus_ThrowsArgumentException()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        Assert.Throws<ArgumentException>(() => customer.UpdateStatus("Suspended"));
    }
}