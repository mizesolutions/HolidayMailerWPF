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
            this.db = db;
            db.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
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
