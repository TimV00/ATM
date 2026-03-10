namespace model;
using System.Data.Common;
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
    // Get all customers
    public static List<Customer> GetAll()
    {
        var customers = new List<Customer>();
        var dt = CustomerDal.GetAll();
        foreach (DataRow r in dt.Rows)
        {
            customers.Add(new Customer
            {
                customer_id = (int)r["customer_id"],
                customer_name = (string)r["customer_name"],
                user_id = (int)r["user_id"],
                balance = (decimal)r["balance"],
                status = (string)r["status"],
            });
        }

        return customers;
    }

    public static Customer GetBy(int id)
    {
        var dt = CustomerDal.GetBy(id);

        // check if we are trying to get an customer that doesn't exist
        if (dt == null || dt.Rows.Count == 0)
            return null;

        var r = dt.Rows[0];


        var customer = new Customer
        {
                customer_id = (int)r["customer_id"],
                customer_name = (string)r["customer_name"],
                user_id = (int)r["user_id"],
                balance = (decimal)r["balance"],
                status = (string)r["status"],
        };
        return customer;
    }
    public static int Create(Customer customer)
    {
        var newId = CustomerDal.Create(
            customer.user_id!,
            customer.customer_name!,
            customer.balance!,
            customer.status ?? ""
        );

        return newId;
    }
    public static int Update(Customer customer)
    {
        var rows = CustomerDal.Update(
            customer.customer_id!,
            customer.customer_name!,
            customer.status ?? ""
        );

        return rows;
    }
    public static int DeleteCustomer(int id)
    {
        return CustomerDal.DeleteCustomer(id);
    }
}
