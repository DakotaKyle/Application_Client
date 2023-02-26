using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        private static String connectionString = "Host=localhost;Port=3306;Database=client_schedule;Username=sqlUser;Password=Passw0rd!";
        private MySqlConnection connection = new(connectionString);
        private string username, password;
        public static bool isvalid { get; set; }

        public LoginPage()
        {
            InitializeComponent();
        }

        private void authenticate()
        {
            String user, pass;
            MySqlCommand userData = new("SELECT userName, password FROM user",connection);

            try
            {
                connection.Open();

                using (var reader = userData.ExecuteReader())
                {
                    reader.Read();
                    user = reader.GetString(0);
                    pass = reader.GetString(1);
                }

                connection.Close();

                if (username == user && password == pass)
                {
                    isvalid = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Username and password not found. Please try again or create an account.");
                    return;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

        }

        private void Loginbuton_Click(object sender, RoutedEventArgs e)
        {
            username = UsernameTextbox.Text;
            password = PasswordBox.Password.ToString();
            
            authenticate();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
