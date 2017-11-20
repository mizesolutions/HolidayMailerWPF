using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace HolidayMailer {
    /// <summary>
    /// Brian Mize
    /// CSCD 371
    /// Holiday Mailer
    /// 
    /// Interaction logic for EditContactWindow.xaml
    /// </summary>
    public partial class EditContactWindow : Window {
        private Database db;
        public EditContactWindow(Database db) {
            InitializeComponent();
            this.db = db;
            LoadNodes();
        }

        private void button_save_Click(object sender, RoutedEventArgs e) {
            if (IsValid()) {
                DataRowView row = (DataRowView)dataGrid_contacts.SelectedItems[0];
                string oldFName = row["FirstName"].ToString();
                string oldLName = row["LastName"].ToString();
                string oldEmail = row["Email"].ToString();
                bool oldRecv = (bool)row["RecievedMail"];
                string newFname = textBox_fname.Text;
                string newLname = textBox_lName.Text;
                string newEmail = textBox_email.Text;
                bool newRecv = (bool)checkBox_recieved_email.IsChecked;
                db.ExecuteDbQuery(Queries.UpdateContactRecord(oldEmail, newFname, newLname, newEmail, newRecv));
                db.ExecuteDbQuery(Queries.UpdateMemberRecordByEmail(oldEmail, newEmail));
                db.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
            }
            else {
                MessageBox.Show("Please select a contact and try again.");
            }
        }

        private bool IsValid() {
            bool valid = IsValidTextBox(textBox_fname);
            valid = IsValidTextBox(textBox_lName);
            valid = IsValidTextBox(textBox_email);
            return valid;

        }

        private bool IsValidTextBox(TextBox tb) {
            return tb.Text != null && tb.Text != "";
        }

        private void LoadNodes() {
            db.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));  
        }

        private void dataGrid_contacts_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (dataGrid_contacts.SelectedItems.Count > 0) {
                DataRowView row = (DataRowView)dataGrid_contacts.SelectedItems[0];
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

        private void button_cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
