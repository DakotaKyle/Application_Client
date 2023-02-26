using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Application_Client
{
    class CustomerList
    {
        private static String connectionString = "Host=localhost;Port=3306;Database=client_schedule;Username=sqlUser;Password=Passw0rd!";
        private MySqlConnection connection = new(connectionString);
        public static BindingList<Customer> Customers = new();

        public void initCustomer()
        {
            String name, address, city, postalCode, country, phone;
            int customerId, addressId, cityId, countryId;
            int i = 0;
            MySqlCommand customerList = new("SELECT customer.customerName, customer.customerId, address.address, address.addressId, city.city, city.cityId, address.postalCode, country.country, country.countryId, address.phone FROM customer JOIN address ON customer.addressId = address.addressId JOIN city ON address.cityId = city.cityId JOIN country ON city.countryId = country.countryId", connection);

            try
            {
                connection.Open();

                DataTable customerTable = new();
                customerTable.Load(customerList.ExecuteReader());

                foreach (DataRow row in customerTable.Rows)
                {
                    

                    name = customerTable.Rows[i]["customerName"].ToString();
                    customerId = (int)customerTable.Rows[i]["customerId"];
                    address = customerTable.Rows[i]["address"].ToString();
                    addressId = (int)customerTable.Rows[i]["addressId"];
                    city = customerTable.Rows[i]["city"].ToString();
                    cityId = (int)customerTable.Rows[i]["cityId"];
                    postalCode = customerTable.Rows[i]["postalCode"].ToString();
                    country = customerTable.Rows[i]["country"].ToString();
                    countryId = (int)customerTable.Rows[i]["countryId"];
                    phone = customerTable.Rows[i]["phone"].ToString();

                    Customer initcustomers = new(name, customerId, address, addressId, city, cityId, postalCode, country, countryId, phone);
                    addCustomer(initcustomers);

                    i++;
                }

                    connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        public void addCustomer(Customer customer)
        {
            Customers.Add(customer);
        }
    }
}
