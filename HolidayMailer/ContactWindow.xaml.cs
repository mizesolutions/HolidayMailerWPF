using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace HolidayMailer
{

    /// <summary>
    /// Handles user adding contact.
    /// </summary>
    public partial class MemberResult
    {
        public Database Database { get; set; }

        public MemberResult(Database database)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            Database = database;
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
        /// Saves user input to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                var fName = textBox_first_name.Text;
                var lName = textBox_last_name.Text;
                var email = textBox_email.Text;
                var recv = (bool)checkBox_previous_mailer.IsChecked;
                var record = Queries.InsertContact(fName, lName, email, recv);
                Database.InsertRecord(record);
                Close();
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Please fill in all fields and check that the email is valid or click close to return to the main window.", "Add Contact Error");
                Focus();
            }
        }

        /// <summary>
        /// Checks form for correct user input
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            var valid = true;
            var rgx = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");


            if (string.IsNullOrEmpty(textBox_first_name.Text))
            {
                textBox_first_name.Background = Brushes.MistyRose;
                valid = false;
            }
            else
            {
                textBox_first_name.Background = Brushes.White;
            }

            if (string.IsNullOrEmpty(textBox_last_name.Text))
            {
                textBox_last_name.Background = Brushes.MistyRose;
                valid = false;
            }
            else
            {
                textBox_last_name.Background = Brushes.White;
            }

            if (!rgx.IsMatch(textBox_email.Text))
            {
                textBox_email.Background = Brushes.MistyRose;
                valid = false;
            }
            else
            {
                textBox_email.Background = Brushes.White;
            }

            return valid;
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
