using System.Windows;
using System;

namespace HolidayMailer
{
    /// <summary>
    /// Allows user to remove lists
    /// </summary>
    public partial class RemoveListWindow
    {
        public Database Database { get; set; }

        public RemoveListWindow(Database database)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            Database = database;
            database.LoadListBox(listBox_mailingLists, Queries.SelectAll(Database.ListsTable));
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
        /// Removes the mailing list from the list table and the removes attached members
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRemoveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listBox_mailingLists.Items.Count <= 0) return;
                var listName = listBox_mailingLists.SelectedItem.ToString();
                Database.ExecuteDatabaseQuery(Queries.DeleteList(listName));
                Database.ExecuteDatabaseQuery(Queries.DeleteMemberByList(listName));
                listBox_mailingLists.Items.Clear();
                Database.LoadListBox(listBox_mailingLists, Queries.SelectAll(Database.ListsTable));
            }
            catch (Exception)
            {
                MessageBox.Show(Application.Current.MainWindow, "Please select a list to remove or click close to return to the main window.", "Rmove List");
                this.Focus();
            }
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
