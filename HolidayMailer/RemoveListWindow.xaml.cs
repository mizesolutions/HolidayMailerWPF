using System.Windows;

namespace HolidayMailer {
    /// <summary>
    /// Brian Mize
    /// CSCD 371
    /// Holiday Mailer
    /// 
    /// Interaction logic for RemoveListWindow.xaml
    /// </summary>
    public partial class RemoveListWindow : Window {
        private Database db;
        public RemoveListWindow(Database db) {
            InitializeComponent();
            this.db = db;
            db.LoadListBox(listBox_mailingLists, Queries.SelectAll(Database.ListsTable));
        }

        private void button_remove_Click(object sender, RoutedEventArgs e) {
            if (listBox_mailingLists.Items.Count > 0) {
                string listName = listBox_mailingLists.SelectedItem.ToString();
                db.ExecuteDbQuery(Queries.DeleteList(listName));
                db.ExecuteDbQuery(Queries.DeleteMemberByList(listName));
                listBox_mailingLists.Items.Clear();
                db.LoadListBox(listBox_mailingLists, Queries.SelectAll(Database.ListsTable));
            }

        }

        private void button_cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
