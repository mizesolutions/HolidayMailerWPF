using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Navigation;

namespace HolidayMailer
{
    /// <summary>
    /// Interaction logic for CredWindow.xaml
    /// </summary>
    public partial class CredWindow
    {

        //private string user;
        //private string password;
        //private NetworkCredential netCred;
        #region Properties

        public string User { get; set; }
        public string Password { get; set; }
        public NetworkCredential NetworkCredential { get; set; }

        #endregion Properties


        public CredWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
        }

        /// <summary>
        /// Checks if the user creditials are set.
        /// </summary>
        /// <returns></returns>
        public bool CheckCreds()
        {
            return (User.Length > 0 && Password.Length > 0);
        }

        /// <summary>
        /// Gets the user names and password and creates a new NetworkCredential object.
        /// </summary>
        private void CredSet()
        {
            User = txtBx_User.Text;
            Password = pwdBx_pwd.Password;
            if (CheckCreds())
            {
                NetworkCredential = new NetworkCredential(User, Password);
            }
            else
            {
                throw new Exception("User name or password empty");
            }
        }

        /// <summary>
        /// Centers the window.
        /// </summary>
        private void CenterWindowOnScreen()
        {
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
            Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;
        }

        /// <summary>
        /// Opens the passed URL in the user's browser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        /// <summary>
        /// Closes the window when the user clicks cancel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Sets the user credentials, sets the NetworkCredentials object in the Credential object.
        /// Updates the ChangeLogInOut flag in the main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubmitClick(object sender, RoutedEventArgs e)
        {
            try
            {
                CredSet();
                Credential.NetworkCredential = NetworkCredential;
                Credential.User = User;
                ((MainWindow)Application.Current.MainWindow)?.ChangeLogInOut(false);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, ex.ToString(), "Input Error");
            }
        }

    }
}
