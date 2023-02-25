using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Client
{
    class CustomerList
    {
        public static BindingList<Customer> Customers = new();

        public void addCustomer(Customer customer)
        {
            Customers.Add(customer);
        }
    }
}
