using System.Windows;
using System;

namespace HolidayMailer {

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

        private void Button_remove_Click(object sender, RoutedEventArgs e) {

            try
            {
                if (listBox_mailingLists.Items.Count <= 0) return;
                string listName = listBox_mailingLists.SelectedItem.ToString();
                db.ExecuteDbQuery(Queries.DeleteList(listName));
                db.ExecuteDbQuery(Queries.DeleteMemberByList(listName));
                listBox_mailingLists.Items.Clear();
                db.LoadListBox(listBox_mailingLists, Queries.SelectAll(Database.ListsTable));
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, "Please select a list to remove or click close to return to the main window.", "Rmove List");
                this.Focus();
            }


        }

        private void Button_cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
