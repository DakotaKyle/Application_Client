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
    /// Interaction logic for ModifyCustomerWindow.xaml
    /// </summary>
    public partial class ModifyCustomerWindow : Window
    {
        private static String connectionString = "Host=localhost;Port=3306;Database=client_schedule;Username=sqlUser;Password=Passw0rd!";
        private MySqlConnection connection = new(connectionString);

        Customer oldCustomer;
        CustomerList customers = new();

        int customerId, addressId, cityId, countryId;

        public ModifyCustomerWindow(Customer customer)
        {
            InitializeComponent();
            NameTextBox.Text = customer.Name;
            AddressTextBox.Text = customer.Address;
            CityTextBox.Text = customer.City;
            ZipcodeTextBox.Text = customer.Zip;
            CountryTextBox.Text = customer.Country;
            PhoneTextBox.Text = customer.Phone;
            customerId = customer.CustomerId;
            addressId = customer.AddressId;
            cityId = customer.CityId;
            countryId = customer.CountryId;
            oldCustomer = customer;
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            String name, address, city, zipcode, country, phone;
            String modifyName = "UPDATE customer SET customerName=@name WHERE customerId=@customerId";
            String modifyAddress = "UPDATE address SET address=@address, postalCode=@postalCode, phone=@phone WHERE addressId=@addressId";
            String modifyCountry = "UPDATE country SET country=@country WHERE countryId=@countryId";

            if (NameTextBox.Text.Length >= 1 && AddressTextBox.Text.Length >= 1 &&
                CityTextBox.Text.Length >= 1 && ZipcodeTextBox.Text.Length >= 1 &&
                CountryTextBox.Text.Length >= 1 && PhoneTextBox.Text.Length >= 1)
            {
                name = NameTextBox.Text;
                address = AddressTextBox.Text;
                city = CityTextBox.Text;
                zipcode = ZipcodeTextBox.Text;
                country = CountryTextBox.Text;
                phone = PhoneTextBox.Text;

                using (MySqlConnection con = new(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (MySqlCommand updateNameCommand = new(modifyName, connection))
                        {
                            updateNameCommand.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
                            updateNameCommand.Parameters.Add("@customerId", MySqlDbType.Int32).Value = customerId;
                            updateNameCommand.ExecuteNonQuery();
                        }

                        using (MySqlCommand updateAddressCommand = new(modifyAddress, connection))
                        {
                            updateAddressCommand.Parameters.Add("@address", MySqlDbType.VarChar).Value = address;
                            updateAddressCommand.Parameters.Add("@postalCode", MySqlDbType.VarChar).Value = zipcode;
                            updateAddressCommand.Parameters.Add("@phone", MySqlDbType.VarChar).Value = phone;
                            updateAddressCommand.Parameters.Add("@addressId", MySqlDbType.Int32).Value = addressId;
                            updateAddressCommand.ExecuteNonQuery();
                        }

                        using (MySqlCommand updateCountryCommand = new(modifyCountry, connection))
                        {
                            updateCountryCommand.Parameters.Add("@country", MySqlDbType.VarChar).Value = country;
                            updateCountryCommand.Parameters.Add("@countryId", MySqlDbType.Int32).Value = countryId;
                            updateCountryCommand.ExecuteNonQuery();
                        }

                        customers.removeCustomer(oldCustomer);
                        Customer modifiedCustomer = new(name, customerId, address, addressId, city, cityId, zipcode, country, countryId, phone);
                        customers.addCustomer(modifiedCustomer);

                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex);
                    }

                    MessageBox.Show("Customer has been updated.");
                    Close();
                }
            }
            else
            {
                MessageBox.Show("All fields are required to continue.");
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
