using System.Collections.Generic;
using System.Windows;

namespace HolidayMailer {
    /// <summary>
    /// Brian Mize
    /// CSCD 371
    /// Holiday Mailer
    /// 
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Database db;
        private delegate void LaunchWindow(Window window);
        private LaunchWindow launchContact;
        private LaunchWindow launchNewList;
        private LaunchWindow launchSendMail;
        private LaunchWindow launchRemoveList;
        private LaunchWindow launchRemoveContact;
        private LaunchWindow launchEditContactsWindow;

        public MainWindow() {
            InitializeComponent();

            CenterWindowOnScreen();

            db = new Database();
            UpdateDataGrid();

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

        private void button_new_contact_Click(object sender, RoutedEventArgs e) {
            launchContact.Invoke(new MemberResult(db));
        }

        private void menu_item_new_list_Click(object sender, RoutedEventArgs e) {
            launchNewList.Invoke(new MailingListWindow(db));
        }

        private void UpdateListBox() {
            listBox_mailing_list.Items.Clear();
            db.LoadListBox(listBox_mailing_list, Queries.SelectAll(Database.ListsTable));
        }

        private void UpdateDataGrid() {
            db.LoadDataGrid(dataGrid_contacts, Queries.SelectAll(Database.ContactsTable));
        }

        private void menu_item_edit_list_Click(object sender, RoutedEventArgs e) {
            if (listBox_mailing_list.SelectedValue != null) {
                tab_Contacts.IsSelected = false;
                tab_MailingLists.IsSelected = true;
                string name = listBox_mailing_list.SelectedValue.ToString();
                launchNewList.Invoke(new MailingListWindow(db, name));
            }
            else {
                MessageBox.Show("Please select a list to edit and try again.");
            }
        }

        private void button_send_to_Click(object sender, RoutedEventArgs e) {
            if (listBox_mailing_list.SelectedItems.Count > 0) {
                string listName = listBox_mailing_list.SelectedItem.ToString();
                List<string> recipients = new List<string>();
                List<Member> members = db.MemberQuery();
                foreach (Member member in members) {
                    if (member.ListName == listName) {
                        recipients.Add(member.Email);
                    }
                }
                launchSendMail.Invoke(new SendWindow(recipients));
            }else {
                MessageBox.Show("Please select a mailing list and try again.");
            }

        }

        private void menu_item_new_contact_Click(object sender, RoutedEventArgs e) {
            launchContact.Invoke(new MemberResult(db));
        }

        private void menu_item_edit_contact_Click(object sender, RoutedEventArgs e) {
            launchEditContactsWindow.Invoke(new EditContactWindow(db));
        }

        private void menu_item_remove_list_Click(object sender, RoutedEventArgs e) {
            launchRemoveList(new RemoveListWindow(db));
        }

        private void button_remove_contact_Click(object sender, RoutedEventArgs e) {
            launchRemoveContact(new RemoveContactWindow(db));
        }

        private void button_edit_contact_Click(object sender, RoutedEventArgs e) {
            launchEditContactsWindow.Invoke(new EditContactWindow(db));
        }

        private void UpdateDefaultLists() {
            string allContacts = "All Contacts";
            string sentMeMail = "Contacts who have sent me mail previously.";
            db.InsertRecord(Queries.InsertList(allContacts));
            db.InsertRecord(Queries.InsertList(sentMeMail));
            List<Person> contacts = db.PersonQuery();
            foreach (Person c in contacts) {
                db.InsertRecord(Queries.InsertMember(allContacts, c.Email));
                if (c.Recieved) {
                    db.InsertRecord(Queries.InsertMember(sentMeMail, c.Email));
                }
            }
        }

        private void button_filter_Click(object sender, RoutedEventArgs e) {
            if (textBox_filter.Text != null) {
                db.LoadDataGrid(dataGrid_contacts, Queries.SelectContactsByLastName(textBox_filter.Text));
            }

        }

        private void menu_item_exit_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void menu_item_edit_list_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tab_Contacts.IsSelected = false;
            tab_MailingLists.IsSelected = true;
        }

        private void menu_item_new_contact_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tab_Contacts.IsSelected = true;
            tab_MailingLists.IsSelected = false;
        }

        private void menu_item_edit_contact_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tab_Contacts.IsSelected = true;
            tab_MailingLists.IsSelected = false;
        }

    }
}
