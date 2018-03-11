using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace HolidayMailer
{
    /// <summary>
    /// Allows user to manage mailing lists.
    /// </summary>
    public partial class MailingListWindow
    {
        public Database Database { get; set; }
        public List<string> Emails { get; set; }


        public MailingListWindow(Database database, string listName = null)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            Database = database;
            Database.LoadDataGrid(dataGrid_results, Queries.SelectAll(Database.ContactsTable));
            Emails = new List<string>();
            if (listName != null)
            {
                LoadListData(listName);
            }
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
        /// Adds selction to the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid_results.SelectedCells.Count > 0)
            {
                var dataRow = (DataRowView)dataGrid_results.SelectedItem;
                var cellValue = dataRow.Row.ItemArray[2].ToString();
                listBox_members.Items.Add(cellValue);
            }
        }

        /// <summary>
        /// Removes selection from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRemoveClick(object sender, RoutedEventArgs e)
        {
            if (listBox_members.SelectedItem != null)
            {
                listBox_members.Items.Remove(listBox_members.SelectedItem);
            }
            else
            {
                listBox_members.Items.Clear();
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

        /// <summary>
        /// Saves the created list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveClick(object sender, RoutedEventArgs e)
        {
            var newEmails = CollectData();
            if (IsValid())
            {
                var name = textBox_name.Text;
                Database.InsertRecord(Queries.InsertList(name));
                foreach (var m in Database.MemberQuery())
                {
                    if (!newEmails.Contains(m.Email))
                    {
                        Database.ExecuteDatabaseQuery(Queries.DeleteMemberByList(name));
                    }
                }
                foreach (var email in newEmails)
                {
                    Database.InsertRecord(Queries.InsertMember(name, email));
                }
                Close();
            }
        }

        /// <summary>
        /// Builds email list to update the view
        /// </summary>
        /// <returns></returns>
        private List<string> CollectData()
        {
            var emails = new List<string>();
            var len = listBox_members.Items.Count;
            for (var i = 0; i < len; i++)
            {
                emails.Add(listBox_members.Items[i].ToString());
            }
            return emails;
        }

        /// <summary>
        /// Checks for valid input
        /// </summary>
        /// <returns></returns>
        private bool IsValid()
        {
            var valid = true;
            if (string.IsNullOrEmpty(textBox_name.Text))
            {
                valid = false;
                textBox_name.Background = Brushes.MistyRose;
            }
            if (listBox_members.Items.Count <= 0)
            {
                valid = false;
                MessageBox.Show(Application.Current.MainWindow, "Please add at least one contact.", "Contact Selection");
                Focus();
            }
            return valid;
        }

        /// <summary>
        /// Updates list view
        /// </summary>
        /// <param name="listName"></param>
        private void LoadListData(string listName)
        {
            var listNames = Database.ListQuery();
            var listsQuery = from mailer in listNames
                             where mailer.Name == listName
                             select mailer;
            foreach (var name in listsQuery)
            {
                textBox_name.Text = name.Name;
            }
            var members = Database.MemberQuery();
            var membersQuery = from member in members
                               where member.ListName == listName
                               select member;
            foreach (var m in membersQuery)
            {
                listBox_members.Items.Add(m.Email);
                Emails.Add(m.Email);
            }
        }
    }
}
