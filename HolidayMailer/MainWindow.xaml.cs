using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;

namespace HolidayMailer
{
    /// <summary>
    /// Main window for the application handles calling and opeing differnet windows
    /// to allow the user to add, edit, and remove users and mailing lists, as well 
    /// as send emails to contacts and list.
    /// </summary>
    public partial class MainWindow
    {
        #region Properties

        public Database Database { get; set; }
        internal delegate void LaunchWindow(Window window);
        internal LaunchWindow LaunchContact { get; set; }
        internal LaunchWindow LaunchNewList { get; set; }
        internal LaunchWindow LaunchSendMail { get; set; }
        internal LaunchWindow LaunchRemoveList { get; set; }
        internal LaunchWindow LaunchRemoveContact { get; set; }
        internal LaunchWindow LaunchEditContactsWindow { get; set; }
        internal LaunchWindow LaunchHelpWindow { get; set; }
        internal static LaunchWindow LaunchCredentialWindow { get; set; }
        internal AssemblyInfo EntryAssemblyInfo { get; set; }

        #endregion Properties

        #region MainWindow

        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindowClosed;
            CenterWindowOnScreen();
            EntryAssemblyInfo = new AssemblyInfo(Assembly.GetEntryAssembly());
            menuIt_logout.IsEnabled = false;

            Database = new Database();
            UpdateDataGrid();

            LaunchCredentialWindow = (window) =>
            {
                window.ShowDialog();
            };

            LaunchContact = (window) =>
            {
                window.ShowDialog();
                UpdateDefaultLists();
                UpdateDataGrid();
            };

            LaunchNewList = (window) =>
            {
                window.ShowDialog();
                UpdateListBox();
            };

            LaunchSendMail = (window) =>
            {
                window.ShowDialog();
            };

            LaunchRemoveList = (window) =>
            {
                window.ShowDialog();
                UpdateListBox();
            };

            LaunchRemoveContact = (window) =>
            {
                window.ShowDialog();
                UpdateDataGrid();
            };

            LaunchEditContactsWindow = (window) =>
            {
                window.ShowDialog();
                UpdateDataGrid();
            };

            LaunchHelpWindow = (window) =>
            {
                var form = Help.GetInstance();
                if (form.Visibility != Visibility.Visible)
                {
                    form.Top = 0;
                    form.Left = (Left + Width) - 5;
                    form.Show();
                }
                else
                {
                    form.Focus();
                }
            };

            UpdateDefaultLists();
            UpdateListBox();
        }

        /// <summary>
        /// Centers the window.
        /// </summary>
        private void CenterWindowOnScreen()
        {
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
            Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;
        }

        #endregion MainWindow

        #region Updaters
        /// <summary>
        /// Updates the mailing list display
        /// </summary>
        private void UpdateListBox()
        {
            listBox_mailing_list.Items.Clear();
            Database.LoadListBox(listBox_mailing_list, Queries.SelectAll(Database.ListsTable));
        }

        /// <summary>
        /// Updated the contacts list
        /// </summary>
        private void UpdateDataGrid()
        {
            Database.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
        }

        /// <summary>
        /// Updates the default lists
        /// </summary>
        private void UpdateDefaultLists()
        {
            var allContacts = "All Contacts";
            var sentMeMail = "Contacts who have sent me mail previously.";
            Database.InsertRecord(Queries.InsertList(allContacts));
            Database.InsertRecord(Queries.InsertList(sentMeMail));
            var contacts = Database.PersonQuery();
            foreach (var c in contacts)
            {
                Database.InsertRecord(Queries.InsertMember(allContacts, c.Email));
                if (c.Recieved)
                {
                    Database.InsertRecord(Queries.InsertMember(sentMeMail, c.Email));
                }
            }
        }

        #endregion Updaters

        #region Click Functions
        /// <summary>
        /// Opens add contact window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNewContactClick(object sender, RoutedEventArgs e)
        {
            LaunchContact.Invoke(new MemberResult(Database));
        }

        /// <summary>
        /// Opens add list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemNewListClick(object sender, RoutedEventArgs e)
        {
            LaunchNewList.Invoke(new MailingListWindow(Database));
        }

        /// <summary>
        /// Opens edit list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemEditListClick(object sender, RoutedEventArgs e)
        {
            if (listBox_mailing_list.SelectedValue != null)
            {
                tab_Contacts.IsSelected = false;
                tab_MailingLists.IsSelected = true;
                var name = listBox_mailing_list.SelectedValue.ToString();
                LaunchNewList.Invoke(new MailingListWindow(Database, name));
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Please select a list to edit and try again.", "List Selection");
            }
        }

        /// <summary>
        /// Calls BtnSendToClick function to open the Send To window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemSendClick(object sender, RoutedEventArgs e)
        {
            BtnSendToClick(sender, e);
        }

        /// <summary>
        /// Opens the Send To window if the user has validated their credentials, else they are prompted to validate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendToClick(object sender, RoutedEventArgs e)
        {
            if (Credential.CredReady())
            {
                if (listBox_mailing_list.SelectedItems.Count > 0)
                {
                    var listName = listBox_mailing_list.SelectedItem.ToString();
                    var members = Database.MemberQuery();
                    var recipients = (from member in members where member.ListName == listName select member.Email).ToList();
                    LaunchSendMail.Invoke(new SendWindow(recipients));
                }
                else
                {
                    MessageBox.Show(Application.Current.MainWindow, "Please select a mailing list and try again.", "List Not Selected");
                }
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Please login before sending mail.", "Not Signed In");
                LaunchCredWindow();
                if (Credential.CredReady())
                {
                    menuIt_login.IsEnabled = false;
                    menuIt_logout.IsEnabled = true;
                }
            }

        }

        /// <summary>
        /// Opens the new contact window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemNewContactClick(object sender, RoutedEventArgs e)
        {
            LaunchContact.Invoke(new MemberResult(Database));
        }

        /// <summary>
        /// Opens the edit contact window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemEditContactClick(object sender, RoutedEventArgs e)
        {
            LaunchEditContactsWindow.Invoke(new EditContactWindow(Database));
        }

        /// <summary>
        /// Opens remove list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenutItemRemoveListClick(object sender, RoutedEventArgs e)
        {
            LaunchRemoveList(new RemoveListWindow(Database));
        }

        /// <summary>
        /// Opens remove contact window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRemoveContactClick(object sender, RoutedEventArgs e)
        {
            LaunchRemoveContact(new RemoveContactWindow(Database));
        }

        /// <summary>
        /// Opens edit contact window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditContactClick(object sender, RoutedEventArgs e)
        {
            LaunchEditContactsWindow.Invoke(new EditContactWindow(Database));
        }

        /// <summary>
        /// Allows user to search list by last name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearchFilterClick(object sender, RoutedEventArgs e)
        {
            if (textBox_filter?.Text != null)
            {
                Database.LoadDataGrid(dataGrid_contacts, Queries.SelectContactsByLastName(textBox_filter.Text));
            }
        }

        /// <summary>
        /// Closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Opens the credentials window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemLoginClick(object sender, RoutedEventArgs e)
        {
            LaunchCredentialWindow.Invoke(new CredWindow());
        }

        /// <summary>
        /// Opens the help window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItenHelpClick(object sender, RoutedEventArgs e)
        {
            LaunchHelpWindow(new Help());
        }

        /// <summary>
        /// Opens the credential window
        /// </summary>
        public static void LaunchCredWindow()
        {
            LaunchCredentialWindow.Invoke(new CredWindow());
        }

        /// <summary>
        /// Allows user to clear their credentials
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemLogoutClick(object sender, RoutedEventArgs e)
        {
            Credential.NetworkCredential = new NetworkCredential();
            Credential.User = "";
            if (!Credential.CredReady())
            {
                MessageBox.Show(Application.Current.MainWindow, "Your email credintials have been cleared. \n\n" +
                                                                "You may continue to edit members and mailing lists. \n" +
                                                                "In oder to send any new messages, you will be required\n" +
                                                                "to log in again.", "Logout");
            }
            ChangeLogInOut(true);
        }

        /// <summary>
        /// Opens the About window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAboutClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Application.Current.MainWindow, EntryAssemblyInfo.Company + "\n" +
                                                            EntryAssemblyInfo.Product + "\n" +
                                                            EntryAssemblyInfo.Copyright + "\n" +
                                                            EntryAssemblyInfo.Description + "\n" +
                                                            "Version: " + EntryAssemblyInfo.Version + "\n" +
                                                            "64 bit/ 32 bit prefered", "About Holiday Mailer");
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Switches to the Mail tab based on mouse location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemMailingMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tab_Contacts.IsSelected = false;
            tab_MailingLists.IsSelected = true;
        }

        /// <summary>
        /// Switches to the Contacts tab based on mouse location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemContactMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tab_Contacts.IsSelected = true;
            tab_MailingLists.IsSelected = false;
        }

        /// <summary>
        /// Enables/disables log in and log out menu items
        /// </summary>
        /// <param name="value"></param>
        public void ChangeLogInOut(bool value)
        {
            menuIt_login.IsEnabled = value;
            menuIt_logout.IsEnabled = !value;
        }

        /// <summary>
        /// Closes all windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void MainWindowClosed(object sender, EventArgs args)
        {
            CloseAllWindows();
        }

        /// <summary>
        /// Closes all windows
        /// </summary>
        private void CloseAllWindows()
        {
            for (var intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                Application.Current.Windows[intCounter].Close();
        }

        #endregion
    }
}
