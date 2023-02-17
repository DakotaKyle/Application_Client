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

        public AddCustomerWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            int primaryKey;
            MySqlCommand getForeignKey = new("SELECT addressId FROM address ORDER BY addressId Desc",connection);
            String updateCustomerName = "INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@Name, @addressId, 0, 02/13/2023, 0, 0, 0)";
            String updateCustomerAddress = "INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@address, 0, 1, @zipCode, @phone, 0, 0, 0, 0)";

            using (MySqlConnection con = new(connectionString)) {

                try
                {

                    connection.Open();

                    using (MySqlCommand addressCommand = new(updateCustomerAddress, connection))
                    {

                        addressCommand.Parameters.Add("@address", MySqlDbType.VarChar).Value = AddressTextBox.Text;
                        addressCommand.Parameters.Add("@zipCode", MySqlDbType.VarChar).Value = ZipcodeTextBox.Text;
                        addressCommand.Parameters.Add("@phone", MySqlDbType.VarChar).Value = PhoneTextBox.Text;

                        addressCommand.ExecuteNonQuery();

                        primaryKey = (int)getForeignKey.ExecuteScalar();
                    }

                    using (MySqlCommand nameCommand = new(updateCustomerName, connection))
                    {

                        nameCommand.Parameters.Add("@Name", MySqlDbType.VarChar).Value = NameTextBox.Text;
                        nameCommand.Parameters.Add("addressId", MySqlDbType.Int32).Value = primaryKey;

                        nameCommand.ExecuteNonQuery();
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
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
