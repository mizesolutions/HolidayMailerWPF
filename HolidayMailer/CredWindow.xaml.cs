using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HolidayMailer
{
    /// <summary>
    /// Interaction logic for CredWindow.xaml
    /// </summary>
    public partial class CredWindow : Window
    {

        private string user;
        private string password;
        private NetworkCredential netCred;

        public CredWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
        }

        public string User { get => user; set => user = value; }
        public string Password { get => password; set => password = value; }
        public NetworkCredential NetCred { get => netCred; set => netCred = value; }


        private void CredSet()
        {
            User = txtBx_User.Text;
            Password = pwdBx_pwd.Password;
            if (CheckCreds())
            {
                NetCred = new NetworkCredential(User, Password);
            }
            else
            {
                throw new Exception("User name or password empty");
            }
        }

        public bool CheckCreds()
        {
            return (User.Length > 0 && Password.Length > 0);
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Btn_submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CredSet();
                Cred.NetCred = NetCred;
                Cred.User = User;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, ex.ToString(), "Input Error");
            }
        }
        
    }
}
