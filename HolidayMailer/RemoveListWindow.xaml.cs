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
            CenterWindowOnScreen();
            this.db = db;
            db.LoadListBox(listBox_mailingLists, Queries.SelectAll(Database.ListsTable));
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
