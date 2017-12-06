using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace HolidayMailer {

    public partial class EditContactWindow : Window {

        private Database db;

        public EditContactWindow(Database db) {
            InitializeComponent();
            CenterWindowOnScreen();
            this.db = db;
            LoadNodes();
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

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                DataRowView row = dataGrid_contacts.SelectedItems[0] as DataRowView;
                string oldEmail = row["Email"].ToString();
                string newFname = textBox_fname.Text;
                string newLname = textBox_lName.Text;
                string newEmail = textBox_email.Text;
                bool newRecv = (bool) checkBox_recieved_email.IsChecked;
                db.ExecuteDbQuery(Queries.UpdateContactRecord(oldEmail, newFname, newLname, newEmail, newRecv));
                db.ExecuteDbQuery(Queries.UpdateMemberRecordByEmail(oldEmail, newEmail));
                db.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
                ClearTextBoxes();
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Please be sure all fields are filled out and that the email address is valid or click close to return to main window.", "Contact Selection");
                this.Focus();
            }
        }

        private bool IsValid() {
            return IsValidTextBox(textBox_fname) && IsValidTextBox(textBox_lName) && IsValidEmail(textBox_email);
        }

        private bool IsValidTextBox(TextBox tb) {
            return !string.IsNullOrEmpty(tb.Text);
        }

        private bool IsValidEmail(TextBox email)
        {
            Regex rgx = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return rgx.IsMatch(email.Text) ? true : false;
        }

        private void LoadNodes() {
            db.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));  
        }

        private void DataGrid_contacts_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            DataRowView row = dataGrid_contacts.SelectedItems[0] as DataRowView;

            if (row != null && row[0] != null) {
                string oldFName = row["FirstName"].ToString();
                string oldLName = row["LastName"].ToString();
                string oldEmail = row["Email"].ToString();
                bool oldRecv = (bool)row["RecievedMail"];
                textBox_fname.Text = oldFName;
                textBox_lName.Text = oldLName;
                textBox_email.Text = oldEmail;
                checkBox_recieved_email.IsChecked = oldRecv;
            }
        }

        private void ClearTextBoxes()
        {
            textBox_email.Text = "";
            textBox_fname.Text = "";
            textBox_lName.Text = "";
        }

        private void Button_cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
