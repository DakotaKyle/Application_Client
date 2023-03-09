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
    /// Interaction logic for UserViewWindow.xaml
    /// </summary>
    public partial class UserViewWindow : Window
    {
        public UserViewWindow()
        {
            InitializeComponent();
            UserViewDataGrid.ItemsSource = AppointmentList.UserView;
        }
    }
}
