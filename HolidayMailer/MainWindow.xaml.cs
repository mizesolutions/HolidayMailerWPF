using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;

namespace HolidayMailer {

    public partial class MainWindow : Window {
        private Database db;
        private delegate void LaunchWindow(Window window);
        private LaunchWindow launchContact;
        private LaunchWindow launchNewList;
        private LaunchWindow launchSendMail;
        private LaunchWindow launchRemoveList;
        private LaunchWindow launchRemoveContact;
        private LaunchWindow launchEditContactsWindow;
        private LaunchWindow launchHelpWindow;
        private static LaunchWindow launchCredWindow;
        private AssemblyInfo entryAssemblyInfo;
        

        #region Main Window

        public MainWindow() {
            InitializeComponent();
            this.Closed += new EventHandler(MainWindow_Closed);
            CenterWindowOnScreen();
            entryAssemblyInfo = new AssemblyInfo(Assembly.GetEntryAssembly());
            menuIt_logout.IsEnabled = false;

            db = new Database();
            UpdateDataGrid();

            launchCredWindow = (window) => {
                window.ShowDialog();
            };

            launchContact = (window) => {
                window.ShowDialog();
                UpdateDefaultLists();
                UpdateDataGrid();
            };

            launchNewList = (window) => {
                window.ShowDialog();
                UpdateListBox();
            };

            launchSendMail = (window) => {
                window.ShowDialog();
            };

            launchRemoveList = (window) => {
                window.ShowDialog();
                UpdateListBox();
            };

            launchRemoveContact = (window) => {
                window.ShowDialog();
                UpdateDataGrid();
            };

            launchEditContactsWindow = (window) => {
                window.ShowDialog();
                UpdateDataGrid();
            };

            launchHelpWindow = (window) => {
                window.Top = 10;
                window.Left = Left + Width;
                if(!window.IsVisible)
                    window.Show();
            };

            UpdateDefaultLists();
            UpdateListBox();
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

#endregion

        #region Updaters

        private void UpdateListBox()
        {
            listBox_mailing_list.Items.Clear();
            db.LoadListBox(listBox_mailing_list, Queries.SelectAll(Database.ListsTable));
        }

        private void UpdateDataGrid()
        {
            db.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
        }

        private void UpdateDefaultLists()
        {
            string allContacts = "All Contacts";
            string sentMeMail = "Contacts who have sent me mail previously.";
            db.InsertRecord(Queries.InsertList(allContacts));
            db.InsertRecord(Queries.InsertList(sentMeMail));
            List<Person> contacts = db.PersonQuery();
            foreach (Person c in contacts)
            {
                db.InsertRecord(Queries.InsertMember(allContacts, c.Email));
                if (c.Recieved)
                {
                    db.InsertRecord(Queries.InsertMember(sentMeMail, c.Email));
                }
            }
        }

#endregion

        #region Click Functions

        private void Button_new_contact_Click(object sender, RoutedEventArgs e) {
            launchContact.Invoke(new MemberResult(db));
        }

        private void MenuIt_new_list_Click(object sender, RoutedEventArgs e) {
            launchNewList.Invoke(new MailingListWindow(db));
        }

        private void MenuIt_edit_list_Click(object sender, RoutedEventArgs e) {
            if (listBox_mailing_list.SelectedValue != null) {
                tab_Contacts.IsSelected = false;
                tab_MailingLists.IsSelected = true;
                string name = listBox_mailing_list.SelectedValue.ToString();
                launchNewList.Invoke(new MailingListWindow(db, name));
            }
            else {
                MessageBox.Show(Application.Current.MainWindow, "Please select a list to edit and try again.", "List Selection");
            }
        }

        private void MenuIt_send_Click(object sender, RoutedEventArgs e)
        {
            Button_send_to_Click(sender, e);
        }

        private void Button_send_to_Click(object sender, RoutedEventArgs e) {

            if (Cred.CredReady())
            {
                if (listBox_mailing_list.SelectedItems.Count > 0)
                {
                    string listName = listBox_mailing_list.SelectedItem.ToString();
                    List<Member> members = db.MemberQuery();
                    List<string> recipients = (from member in members where member.ListName == listName select member.Email).ToList();
                    launchSendMail.Invoke(new SendWindow(recipients));
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
                if (Cred.CredReady())
                {
                    menuIt_login.IsEnabled = false;
                    menuIt_logout.IsEnabled = true;
                }
            }

        }

        private void MenuIt_new_contact_Click(object sender, RoutedEventArgs e) {
            launchContact.Invoke(new MemberResult(db));
        }

        private void MenuIt_edit_contact_Click(object sender, RoutedEventArgs e) {
            launchEditContactsWindow.Invoke(new EditContactWindow(db));
        }

        private void MenuIt_remove_list_Click(object sender, RoutedEventArgs e) {
            launchRemoveList(new RemoveListWindow(db));
        }

        private void Button_remove_contact_Click(object sender, RoutedEventArgs e) {
            launchRemoveContact(new RemoveContactWindow(db));
        }

        private void Button_edit_contact_Click(object sender, RoutedEventArgs e) {
            launchEditContactsWindow.Invoke(new EditContactWindow(db));
        }

        private void Button_filter_Click(object sender, RoutedEventArgs e) {
            if (textBox_filter.Text != null) {
                db.LoadDataGrid(dataGrid_contacts, Queries.SelectContactsByLastName(textBox_filter.Text));
            }
        }

        private void MenuIt_exit_Click(object sender, RoutedEventArgs e) {
            Close();
        }
        
        private void MenuIt_login_Click(object sender, RoutedEventArgs e)
        {
            launchCredWindow.Invoke(new CredWindow());
        }

        private void MenuIt_help_click(object sender, RoutedEventArgs e)
        {
            launchHelpWindow(new Help());
        }

        public static void LaunchCredWindow()
        {
            launchCredWindow.Invoke(new CredWindow());
        }

        private void MenuIt_logout_Click(object sender, RoutedEventArgs e)
        {
            Cred.NetCred = new NetworkCredential();
            Cred.User = "";
            if (!Cred.CredReady())
            {
                MessageBox.Show(Application.Current.MainWindow, "Your email credintials have been cleared. \n\n" +
                                                                "You may continue to edit members and mailing lists. \n" +
                                                                "In oder to send any new messages, you will be required\n" +
                                                                "to log in again.", "Logout");
            }
           ChangeLogInOut(true);
        }

        private void MenuIt_about_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Application.Current.MainWindow, entryAssemblyInfo.Company + "\n" +
                                                            entryAssemblyInfo.Product + "\n" +
                                                            entryAssemblyInfo.Copyright + "\n" +
                                                            entryAssemblyInfo.Description + "\n" +
                                                            "Version: " + entryAssemblyInfo.Version + "\n" +
                                                            "64 bit/ 32 bit prefered", "About Holiday Mailer");
        }

        #endregion

        #region Helper Methods

        private void MenuIt_Mailing_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tab_Contacts.IsSelected = false;
            tab_MailingLists.IsSelected = true;
        }

        private void MenuIt_Contact_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tab_Contacts.IsSelected = true;
            tab_MailingLists.IsSelected = false;
        }

        public void ChangeLogInOut(bool value)
        {
            menuIt_login.IsEnabled = value;
            menuIt_logout.IsEnabled = !value;
        }

        protected void MainWindow_Closed(object sender, EventArgs args)
        {
            CloseAllWindows();
        }

        private void CloseAllWindows()
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                App.Current.Windows[intCounter].Close();
        }

        #endregion


    }
}
