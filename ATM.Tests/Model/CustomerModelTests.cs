namespace ATM.Tests.Model;
using Moq;
using model;
using dal;
using System.Data;

public class CustomerModelTests
{
    private readonly Mock<ICustomerDal> _mockDal;
    private readonly CustomerModel _customerModel;

    public CustomerModelTests()
    {
        _mockDal = new Mock<ICustomerDal>();
        _customerModel = new CustomerModel(_mockDal.Object);
    }

    private static DataTable BuildCustomerTable(int customer_id, int user_id, string customer_name, decimal balance, string status)
    {
        var dt = new DataTable();
        dt.Columns.Add("customer_id", typeof(int));
        dt.Columns.Add("user_id", typeof(int));
        dt.Columns.Add("customer_name", typeof(string));
        dt.Columns.Add("balance", typeof(decimal));
        dt.Columns.Add("status", typeof(string));
        dt.Rows.Add(customer_id, user_id, customer_name, balance, status);
        return dt;
    }

    [Fact]
    public void GetBy_ReturnsCustomer_WhenFound()
    {
        _mockDal.Setup(d => d.GetBy(1)).Returns(BuildCustomerTable(1, 1, "John Doe", 100.00m, "Active"));
        var customer = _customerModel.GetBy(1);
        Assert.NotNull(customer);
        Assert.Equal(1, customer.customer_id);
        Assert.Equal("John Doe", customer.customer_name);
    }

    [Fact]
    public void GetBy_ReturnsNull_WhenNotFound()
    {
        _mockDal.Setup(d => d.GetBy(99)).Returns(new DataTable());
        var customer = _customerModel.GetBy(99);
        Assert.Null(customer);
    }

    [Fact]
    public void GetByUserID_ReturnsCustomer_WhenFound()
    {
        _mockDal.Setup(d => d.GetByUserID(1)).Returns(BuildCustomerTable(1, 1, "John Doe", 100.00m, "Active"));
        var customer = _customerModel.GetByUserID(1);
        Assert.NotNull(customer);
        Assert.Equal(1, customer.user_id);
    }

    [Fact]
    public void GetByUserID_ReturnsNull_WhenNotFound()
    {
        _mockDal.Setup(d => d.GetByUserID(99)).Returns(new DataTable());
        var customer = _customerModel.GetByUserID(99);
        Assert.Null(customer);
    }

    [Fact]
    public void Create_CallsDalWithCorrectParameters()
    {
        var customer = Customer.Create(0, 1, "John Doe", 100.00m, "Active");
        _mockDal.Setup(d => d.Create(1, "John Doe", 100.00m, "Active")).Returns(1);
        var result = _customerModel.Create(customer);
        Assert.Equal(1, result);
        _mockDal.Verify(d => d.Create(1, "John Doe", 100.00m, "Active"), Times.Once);
    }

    [Fact]
    public void Update_CallsDalWithCorrectParameters()
    {
        var customer = Customer.Create(1, 1, "John Doe", 100.00m, "Active");
        _mockDal.Setup(d => d.Update(1, "John Doe", 100.00m, "Active")).Returns(1);
        var result = _customerModel.Update(customer);
        Assert.Equal(1, result);
        _mockDal.Verify(d => d.Update(1, "John Doe", 100.00m, "Active"), Times.Once);
    }

    [Fact]
    public void DeleteCustomer_CallsDalWithCorrectId()
    {
        _mockDal.Setup(d => d.DeleteCustomer(1)).Returns(1);
        var result = _customerModel.DeleteCustomer(1);
        Assert.Equal(1, result);
        _mockDal.Verify(d => d.DeleteCustomer(1), Times.Once);
    }

    [Fact]
    public void GetAll_ReturnsAllCustomers()
    {
        var dt = new DataTable();
        dt.Columns.Add("customer_id", typeof(int));
        dt.Columns.Add("user_id", typeof(int));
        dt.Columns.Add("customer_name", typeof(string));
        dt.Columns.Add("balance", typeof(decimal));
        dt.Columns.Add("status", typeof(string));
        dt.Rows.Add(1, 1, "John Doe", 100.00m, "Active");
        dt.Rows.Add(2, 2, "Jane Doe", 200.00m, "Inactive");
        _mockDal.Setup(d => d.GetAll()).Returns(dt);
        var customers = _customerModel.GetAll();
        Assert.Equal(2, customers.Count);
    }
}