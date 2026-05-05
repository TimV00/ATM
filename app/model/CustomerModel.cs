namespace model;

using dal;
using System.Data;

public class Customer
{
    public int customer_id { get; private set; }
    public string customer_name { get; private set; }
    public int user_id { get; private set; }
    public decimal balance { get; private set; }
    public string status { get; private set; }

    private static readonly HashSet<string> ValidStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "Active", "Inactive"
    };

    private Customer() { }

    public static Customer Create(int customer_id, int user_id, string customer_name, decimal balance, string status)
    {
        if (string.IsNullOrWhiteSpace(customer_name))
            throw new ArgumentException("Customer name cannot be empty.");
        if (balance < 0)
            throw new ArgumentException("Balance cannot be negative.");
        if (!ValidStatuses.Contains(status))
            throw new ArgumentException("Status must be 'Active' or 'Inactive'.");

        return new Customer
        {
            customer_id = customer_id,
            user_id = user_id,
            customer_name = customer_name,
            balance = balance,
            status = status
        };
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be greater than zero.");
        if (amount > balance)
            throw new InvalidOperationException("Insufficient funds.");
        balance -= amount;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be greater than zero.");
        balance += amount;
    }

    public void UpdateName(string customer_name)
    {
        if (string.IsNullOrWhiteSpace(customer_name))
            throw new ArgumentException("Customer name cannot be empty.");
        this.customer_name = customer_name;
    }

    public void UpdateStatus(string status)
    {
        if (!ValidStatuses.Contains(status))
            throw new ArgumentException("Status must be 'Active' or 'Inactive'.");
        this.status = status;
    }

}
public class CustomerModel
{
    private readonly ICustomerDal _dal;

    public CustomerModel(ICustomerDal dal)
    {
        _dal = dal;
    }

    public List<Customer> GetAll()
    {
        var customers = new List<Customer>();
        var dt = _dal.GetAll();
        foreach (DataRow r in dt.Rows)
            customers.Add(MapRow(r));
        return customers;
    }

    public Customer? GetBy(int id)
    {
        var dt = _dal.GetBy(id);
        if (dt == null || dt.Rows.Count == 0) return null;
        return MapRow(dt.Rows[0]);
    }

    public Customer? GetByUserID(int id)
    {
        var dt = _dal.GetByUserID(id);
        if (dt == null || dt.Rows.Count == 0) return null;
        return MapRow(dt.Rows[0]);
    }

    public int Create(Customer customer)
    {
        return _dal.Create(customer.user_id, customer.customer_name!, customer.balance, customer.status ?? "");
    }

    public int Update(Customer customer)
    {
        return _dal.Update(customer.customer_id, customer.customer_name!, customer.balance, customer.status ?? "");
    }

    public int DeleteCustomer(int id)
    {
        return _dal.DeleteCustomer(id);
    }

    private static Customer MapRow(DataRow r) => Customer.Create(
        (int)r["customer_id"],
        (int)r["user_id"],
        (string)r["customer_name"],
        (decimal)r["balance"],
        (string)r["status"]
    );
}