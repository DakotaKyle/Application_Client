using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Application_Client
{
    /// <summary>
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {

        private static String connectionString = "Host=localhost;Port=3306;Database=client_schedule;Username=sqlUser;Password=Passw0rd!";
        private MySqlConnection connection = new(connectionString);
        CustomerList customerList = new();

        public AddCustomerWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            int customerId, cityId, addressPrimaryKey, countryPrimaryKey;
            String name, address, city, zip, country, phone;

            MySqlCommand getAddressForeignKey = new("SELECT addressId FROM address ORDER BY addressId Desc",connection);
            MySqlCommand getCountryForeignKey = new("SELECT countryId FROM country ORDER BY countryId Desc", connection);
            MySqlCommand getCustomerId = new("SELECT customerId FROM customer ORDER BY customerId Desc", connection);
            MySqlCommand getCityId = new("SELECT cityId FROM city ORDER BY cityId Desc", connection);

            String updateCustomerName = "INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@Name, @addressId, 0, 02/13/2023, 0, 0, 0)";
            String updateCustomerAddress = "INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@address, 0, @cityId, @zipCode, @phone, 0, 0, 0, 0)";
            String updateCustomerCity = "INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@city, @countryId, 07/17/2023, 0, 0, 0)";
            String updateCustomerCountry = "INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@country, 07/17/2023, 0, 0, 0)";

            if (NameTextBox.Text.Length >= 1 && AddressTextBox.Text.Length >= 1 &&
                CityTextBox.Text.Length >= 1 && ZipcodeTextBox.Text.Length >= 1 &&
                CountryTextBox.Text.Length >= 1 && PhoneTextBox.Text.Length >= 1)
            {
                name = NameTextBox.Text;
                address = AddressTextBox.Text;
                city = CityTextBox.Text;
                zip = ZipcodeTextBox.Text;
                country = CountryTextBox.Text;
                phone = PhoneTextBox.Text;
            }
            else
            {
                MessageBox.Show("All fields are required to continue.");
                return;
            }

            using (MySqlConnection con = new(connectionString)) {

                try
                {

                    connection.Open();

                    using (MySqlCommand countryCommand = new(updateCustomerCountry, connection))
                    {
                        countryCommand.Parameters.Add("@country", MySqlDbType.VarChar).Value = country;
                        countryCommand.ExecuteNonQuery();

                        countryPrimaryKey = (int)getCountryForeignKey.ExecuteScalar();
                    }

                    using (MySqlCommand cityCommand = new(updateCustomerCity, connection))
                    {
                        cityCommand.Parameters.Add("@city", MySqlDbType.VarChar).Value = city;
                        cityCommand.Parameters.Add("@countryId", MySqlDbType.Int32).Value = countryPrimaryKey;
                        cityCommand.ExecuteNonQuery();
                    }

                    using (MySqlCommand addressCommand = new(updateCustomerAddress, connection))
                    {

                        addressCommand.Parameters.Add("@address", MySqlDbType.VarChar).Value = address;
                        addressCommand.Parameters.Add("@zipCode", MySqlDbType.VarChar).Value = zip;
                        addressCommand.Parameters.Add("@phone", MySqlDbType.VarChar).Value = phone;
                        addressCommand.Parameters.Add("@cityId", MySqlDbType.Int32).Value = countryPrimaryKey;
                        addressCommand.ExecuteNonQuery();

                        addressPrimaryKey = (int)getAddressForeignKey.ExecuteScalar();
                        cityId = (int)getCityId.ExecuteScalar();
                    }

                    using (MySqlCommand nameCommand = new(updateCustomerName, connection))
                    {

                        nameCommand.Parameters.Add("@Name", MySqlDbType.VarChar).Value = name;
                        nameCommand.Parameters.Add("addressId", MySqlDbType.Int32).Value = addressPrimaryKey;
                        nameCommand.ExecuteNonQuery();

                        customerId = (int)getCustomerId.ExecuteScalar();
                    }

                    connection.Close();

                    Customer newCustomer = new(name, customerId, address, addressPrimaryKey,
                        city, cityId, zip, country, countryPrimaryKey, phone);

                    customerList.addCustomer(newCustomer);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    connection.Dispose();
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to cancel? Any unsaved progress will be lost!", "", MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Close();
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
                return;
            }

        }
    }
}
