using System.Collections.Generic;

namespace CustomersAPIService.Data
{
    public class CustomerManager
    {
        static List<Customer> customersList = new List<Customer>();

        static CustomerManager()
        {
            AddData();
        }

        public static void AddData()
        {
            AddCustomer("1", "Amdocs", "Ranaana - North Amdocs, HaSheizaf St 4, Raanana, Israel");
            AddCustomer("2", "AmdocsDVCI", "Tower 7, Magarpatta, Pune, Maharashtra 411028, India");
            AddCustomer("3", "AmdocsCMI", "2109 Fox Dr Champaign IL 61820 USA");
        }

        public static void AddCustomer(Customer customer)
        {
            customersList.Add(customer);
        }

        public static void AddCustomer(string customerID, string customerName, string customerAddress)
        {
            Customer customer = new Customer();
            customer.CustomerID = customerID;
            customer.CustomerName = customerName;
            customer.CustomerAddress = customerAddress;
            customersList.Add(customer);
        }

        public static bool RemoveCustomerByID(string customerID)
        {
            bool customerFound = false;
            Customer customer = GetCustomerByID(customerID);
            if (customer != null)
            {
                customersList.Remove(customer);
                customerFound = true;
            }
            return customerFound;
        }

        public static Customer GetCustomerByID(string customerID)
        {
            Customer customer = customersList.Find(x => x.CustomerID.Equals(customerID));
            return customer;
        }

        public static Customer GetCustomerByName(string customerName)
        {
            Customer customer = customersList.Find(x => x.CustomerName.Equals(customerName));
            return customer;
        }

        public static List<Customer> GetCustomers()
        {
            return customersList;
        }

    }
}
