namespace model;

using dal;
using System.Data;
/// <summary>
/// Represents a customer account in the ATM system.
/// </summary>
public class Customer
{
    /// <summary>Gets the unique identifier for the customer.</summary>
    public int customer_id { get; private set; }
    /// <summary>Gets the name of the customer.</summary>
    public string? customer_name { get; private set; }
    /// <summary>Gets the user ID associated with the customer.</summary>
    public int user_id { get; private set; }
    /// <summary>Gets the balance of the customer's account.</summary>
    public decimal balance { get; private set; }
    /// <summary>Gets the status of the customer's account.</summary>
    public string? status { get; private set; }

    private static readonly HashSet<string> ValidStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "Active", "Inactive"
    };

    private Customer() { }

    /// <summary>
    /// Creates a new <see cref="Customer"/> instance with validated input.
    /// </summary>
    /// <param name="customer_id">The unique identifier for the customer.</param>
    /// <param name="user_id">The associated user ID.</param>
    /// <param name="customer_name">The customer's name. Cannot be empty or whitespace.</param>
    /// <param name="balance">The starting balance. Cannot be negative.</param>
    /// <param name="status">The account status. Must be "Active" or "Inactive".</param>
    /// <returns>A new <see cref="Customer"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when any input fails validation.</exception>
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

    /// <summary>
    /// Withdraws an amount from the customer's balance.
    /// </summary>
    /// <param name="amount">The amount to withdraw. Must be greater than zero and not exceed the current balance.</param>
    /// <exception cref="ArgumentException">Thrown when amount is zero or negative.</exception>
    /// <exception cref="InvalidOperationException">Thrown when amount exceeds the current balance.</exception>
    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be greater than zero.");
        if (amount > balance)
            throw new InvalidOperationException("Insufficient funds.");
        balance -= amount;
    }

    /// <summary>
    /// Deposits an amount into the customer's balance.
    /// </summary>
    /// <param name="amount">The amount to deposit. Must be greater than zero.</param>
    /// <exception cref="ArgumentException">Thrown when amount is zero or negative.</exception>
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be greater than zero.");
        balance += amount;
    }

    /// <summary>
    /// Updates the customer's name.
    /// </summary>
    /// <param name="customer_name">The new name. Cannot be empty or whitespace.</param>
    /// <exception cref="ArgumentException">Thrown when name is empty or whitespace.</exception>
    public void UpdateName(string customer_name)
    {
        if (string.IsNullOrWhiteSpace(customer_name))
            throw new ArgumentException("Customer name cannot be empty.");
        this.customer_name = customer_name;
    }

    /// <summary>
    /// Updates the customer's account status.
    /// </summary>
    /// <param name="status">The new status. Must be "Active" or "Inactive".</param>
    /// <exception cref="ArgumentException">Thrown when status is invalid.</exception>
    public void UpdateStatus(string status)
    {
        if (!ValidStatuses.Contains(status))
            throw new ArgumentException("Status must be 'Active' or 'Inactive'.");
        this.status = status;
    }

}

/// <summary>
/// Provides data access operations for <see cref="Customer"/> objects.
/// </summary>
public class CustomerModel
{
    private readonly ICustomerDal _dal;

    /// <summary>
    /// Initializes a new instance of <see cref="CustomerModel"/> with the specified DAL.
    /// </summary>
    /// <param name="dal">The data access layer for customer operations.</param>
    public CustomerModel(ICustomerDal dal)
    {
        _dal = dal;
    }

    /// <summary>
    /// Retrieves all customers from the database.
    /// </summary>
    /// <returns>A list of all <see cref="Customer"/> objects.</returns>
    public List<Customer> GetAll()
    {
        var customers = new List<Customer>();
        var dt = _dal.GetAll();
        foreach (DataRow r in dt.Rows)
            customers.Add(MapRow(r));
        return customers;
    }

    /// <summary>
    /// Retrieves a customer by their numeric ID.
    /// </summary>
    /// <param name="id">The customer ID to search for.</param>
    /// <returns>The matching <see cref="Customer"/>, or null if not found.</returns>
    public Customer? GetBy(int id)
    {
        var dt = _dal.GetBy(id);
        if (dt == null || dt.Rows.Count == 0) return null;
        return MapRow(dt.Rows[0]);
    }

    /// <summary>
    /// Retrieves a customer by their associated user ID.
    /// </summary>
    /// <param name="id">The user ID to search for.</param>
    /// <returns>The matching <see cref="Customer"/>, or null if not found.</returns>
    public Customer? GetByUserID(int id)
    {
        var dt = _dal.GetByUserID(id);
        if (dt == null || dt.Rows.Count == 0) return null;
        return MapRow(dt.Rows[0]);
    }

    /// <summary>
    /// Inserts a new customer into the database.
    /// </summary>
    /// <param name="customer">The <see cref="Customer"/> to create.</param>
    /// <returns>The ID of the newly created customer.</returns>
    public int Create(Customer customer)
    {
        return _dal.Create(customer.user_id, customer.customer_name!, customer.balance, customer.status ?? "");
    }

    /// <summary>
    /// Updates an existing customer's information in the database.
    /// </summary>
    /// <param name="customer">The <see cref="Customer"/> with updated values.</param>
    /// <returns>The number of rows affected.</returns>
    public int Update(Customer customer)
    {
        return _dal.Update(customer.customer_id, customer.customer_name!, customer.balance, customer.status ?? "");
    }

    /// <summary>
    /// Deletes a customer from the database by ID.
    /// </summary>
    /// <param name="id">The ID of the customer to delete.</param>
    /// <returns>The number of rows affected.</returns>
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
