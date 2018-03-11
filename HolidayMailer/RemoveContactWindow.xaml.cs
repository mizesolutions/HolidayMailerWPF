using System.Data;
using System.Windows;

namespace HolidayMailer
{

    /// <summary>
    /// Allows user to remove contacts from the database
    /// </summary>
    public partial class RemoveContactWindow
    {
        public Database Database { get; set; }

        public RemoveContactWindow(Database database)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            Database = database;
            database.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
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
        /// Deletes contact from the contacts table and the member table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRemoveClick(object sender, RoutedEventArgs e)
        {
            var row = dataGrid_contacts.SelectedItems[0] as DataRowView;

            if (row?[0] != null)
            {
                var email = row["Email"].ToString();
                Database.ExecuteDatabaseQuery(Queries.DeleteContact(email));
                Database.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
                Database.ExecuteDatabaseQuery(Queries.DeleteMemberByEmail(email));
                dataGrid_contacts.SelectedItems.Clear();
            }
        }

        /// <summary>
        /// Closed the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
