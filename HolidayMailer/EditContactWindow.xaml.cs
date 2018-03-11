using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace HolidayMailer
{
    /// <summary>
    /// Allows user to edit contacts in the database.
    /// </summary>
    public partial class EditContactWindow
    {
        public Database Database { get; }

        public EditContactWindow(Database database)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            Database = database;
            LoadNodes();
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
        /// Saves user input to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                var row = dataGrid_contacts.SelectedItems[0] as DataRowView;
                var oldEmail = row?["Email"].ToString();
                var newFname = textBox_fname.Text;
                var newLname = textBox_lName.Text;
                var newEmail = textBox_email.Text;
                var newRecv = (bool)checkBox_recieved_email.IsChecked;
                Database.ExecuteDatabaseQuery(Queries.UpdateContactRecord(oldEmail, newFname, newLname, newEmail, newRecv));
                Database.ExecuteDatabaseQuery(Queries.UpdateMemberRecordByEmail(oldEmail, newEmail));
                Database.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
                ClearTextBoxes();
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Please be sure all fields are filled out and that the email address is valid or click close to return to main window.", "Contact Selection");
                Focus();
            }
        }

        /// <summary>
        /// Checks if user input is valid
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            return IsValidTextBox(textBox_fname) && IsValidTextBox(textBox_lName) && IsValidEmail(textBox_email);
        }

        /// <summary>
        /// Checks if the passed textbox text is valid.
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        private bool IsValidTextBox(TextBox textBox)
        {
            return !string.IsNullOrEmpty(textBox.Text);
        }

        /// <summary>
        /// Checks if email is valid.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(TextBox email)
        {
            var rgx = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return rgx.IsMatch(email.Text) ? true : false;
        }

        /// <summary>
        /// Loads the data grid to update the contacts view
        /// </summary>
        private void LoadNodes()
        {
            Database.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
        }

        /// <summary>
        /// Updates the form information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridContactsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var row = (DataRowView)dataGrid_contacts.SelectedItems[0];

            if (row?[0] != null)
            {
                var oldFName = row["FirstName"].ToString();
                var oldLName = row["LastName"].ToString();
                var oldEmail = row["Email"].ToString();
                var oldRecv = (bool)row["RecievedMail"];
                textBox_fname.Text = oldFName;
                textBox_lName.Text = oldLName;
                textBox_email.Text = oldEmail;
                checkBox_recieved_email.IsChecked = oldRecv;
            }
        }

        /// <summary>
        /// Clears the form text boxes
        /// </summary>
        private void ClearTextBoxes()
        {
            textBox_email.Text = "";
            textBox_fname.Text = "";
            textBox_lName.Text = "";
        }

        /// <summary>
        /// Clsoes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
