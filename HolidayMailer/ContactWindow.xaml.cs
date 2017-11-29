using System.Windows;
using System.Windows.Media;

namespace HolidayMailer {

    public partial class MemberResult : Window {
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
        }

        private bool IsValid() {
            bool valid = true;
            if(textBox_first_name.Text == null || textBox_first_name.Text == "") {
                textBox_first_name.Background = Brushes.Red;
                valid = false;
            }

            if(textBox_last_name.Text == null || textBox_last_name.Text == "") {
                textBox_last_name.Background = Brushes.Red;
                valid = false;
            }

            if(textBox_email.Text == null || textBox_email.Text == "") {
                textBox_email.Background = Brushes.Red;
                valid = false;
            }

            return valid;
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
