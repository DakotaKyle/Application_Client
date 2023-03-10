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
using System.Globalization;
using System.IO;

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
        public static int UserID { get; set; }

        public LoginPage()
        {
            InitializeComponent();
        }

        private void authenticate()
        {
            int i = 0;
            DataTable userTable = new();
            String userName, Password;
            MySqlCommand userData = new("SELECT userId, userName, password FROM user", connection);

            try
            {
                connection.Open();

                userTable.Load(userData.ExecuteReader());

                connection.Close();

                foreach (DataRow row in userTable.Rows)
                {
                    userName = userTable.Rows[i]["userName"].ToString();
                    Password = userTable.Rows[i]["password"].ToString();

                    if (username == userName && password == Password)
                    {
                        isvalid = true;
                        UserID = (int)userTable.Rows[i]["userId"];
                        return;
                    }
                }

                if (!isvalid)
                {
                    CultureInfo culture = CultureInfo.CurrentUICulture;

                    if (culture.Name == "en-US")
                    {
                        MessageBox.Show(String.Format(culture, Properties.Resources.InvalidLoginEng));
                    }
                    else if (culture.Name == "fr-FR")
                    {
                        MessageBox.Show(String.Format(culture, Properties.Resources.InvalidLoginFr));
                    }
                    else
                    {
                        MessageBox.Show("Only English and French are supported.");
                    }
                }       
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                connection.Dispose();
            }
        }

        private void logUser()
        {
            var workingDirectory = Environment.CurrentDirectory;
            var file = (workingDirectory + @"\UserLog.txt");

            DateTime localTime = DateTime.Now;
            if (!File.Exists(file))
            {
                using (StreamWriter sw = File.CreateText(file))
                {
                    sw.WriteLine(UserID.ToString() + " [" + localTime + "]\n");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine(UserID.ToString() + " [" + localTime + "]\n");
                }
            }
        }

        private void Loginbuton_Click(object sender, RoutedEventArgs e)
        {
            username = UsernameTextbox.Text;
            password = PasswordBox.Password.ToString();
            
            authenticate();

            if (isvalid)
            {
                logUser();
                Close();
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
