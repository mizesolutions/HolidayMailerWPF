using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace HolidayMailer {

    public partial class MemberResult : Window
    {
        private Database db;

        public MemberResult(Database db) {
            InitializeComponent();
            CenterWindowOnScreen();
            this.db = db;
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

        private void Button_save_Click(object sender, RoutedEventArgs e) {
            if (IsValid()) {
                string fName = textBox_first_name.Text;
                string lName = textBox_last_name.Text;
                string email = textBox_email.Text;
                bool recv = (bool) checkBox_previous_mailer.IsChecked;
                string record = Queries.InsertContact(fName, lName, email, recv);
                db.InsertRecord(record);
                Close();
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Please fill in all fields and check that the email is valid or click close to return to the main window.", "Add Contact Error");
                this.Focus();
            }
        }

        private bool IsValid() {
            bool valid = true;
            Regex rgx = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");


            if (textBox_first_name.Text == null || textBox_first_name.Text == "")
            {
                textBox_first_name.Background = Brushes.MistyRose;
                valid = false;
            }
            else
            {
                textBox_first_name.Background = Brushes.White;
            }

            if(textBox_last_name.Text == null || textBox_last_name.Text == "") {
                textBox_last_name.Background = Brushes.MistyRose;
                valid = false;
            }
            else
            {
                textBox_last_name.Background = Brushes.White;
            }

            if (!rgx.IsMatch(textBox_email.Text)) {
                textBox_email.Background = Brushes.MistyRose;
                valid = false;
            }
            else
            {
                textBox_email.Background = Brushes.White;
            }

            return valid;
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
