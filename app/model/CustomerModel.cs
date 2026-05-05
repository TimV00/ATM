namespace model;
using dal;
using System.Data;

public class Customer
{
    public int customer_id { get; set; }
    public string customer_name { get; set; }
    public int user_id { get; set; }
    public decimal balance { get; set; }
    public string status { get; set; }
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

    private static Customer MapRow(DataRow r) => new Customer
    {
        customer_id = (int)r["customer_id"],
        customer_name = (string)r["customer_name"],
        user_id = (int)r["user_id"],
        balance = (decimal)r["balance"],
        status = (string)r["status"],
    };
}