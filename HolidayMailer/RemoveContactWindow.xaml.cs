using System.Data;
using System.Windows;


namespace HolidayMailer {
    /// <summary>
    /// Brian Mize
    /// CSCD 371
    /// Holiday Mailer
    /// 
    /// Interaction logic for RemoveContactWindow.xaml
    /// </summary>
    public partial class RemoveContactWindow : Window {
        private Database db;
        public RemoveContactWindow(Database db) {
            InitializeComponent();
            CenterWindowOnScreen();
            this.db = db;
            db.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
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

        private void button_remove_Click(object sender, RoutedEventArgs e) {
            if (dataGrid_contacts.SelectedItems.Count > 0) {
                DataRowView row = (DataRowView)dataGrid_contacts.SelectedItems[0];
                string email = row["Email"].ToString();
                db.ExecuteDbQuery(Queries.DeleteContact(email));
                db.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
                db.ExecuteDbQuery(Queries.DeleteMemberByEmail(email));
            }
        }

        private void button_exit_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
