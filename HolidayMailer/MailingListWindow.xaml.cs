using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace HolidayMailer {

    public partial class MailingListWindow : Window {

        private Database db;
        private List<string> emails;

        public MailingListWindow(Database db, string listName=null) {
            InitializeComponent();
            CenterWindowOnScreen();
            this.db = db;
            db.LoadDataGrid(dataGrid_results, Queries.SelectAll(Database.ContactsTable));
            emails = new List<string>();
            if(listName != null) {
                LoadListData(listName);
            }
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

        private void Button_add_Click(object sender, RoutedEventArgs e) {
            if(dataGrid_results.SelectedCells.Count > 0) {
                DataRowView dataRow = (DataRowView) dataGrid_results.SelectedItem;
                int index = dataGrid_results.SelectedIndex;
                string cellValue = dataRow.Row.ItemArray[2].ToString();
                listBox_members.Items.Add(cellValue);
            }
        }

        private void Button_remove_Click(object sender, RoutedEventArgs e) {
            if(listBox_members.SelectedItem != null) {
                listBox_members.Items.Remove(listBox_members.SelectedItem);
            }
            else {
                listBox_members.Items.Clear();
            }
        }

        private void Button_cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void Button_save_Click(object sender, RoutedEventArgs e) {

            List<string> newEmails = CollectData();
            if (IsValid()) {
                string name = textBox_name.Text;
                db.InsertRecord(Queries.InsertList(name));
                foreach (Member m in db.MemberQuery()) {
                    if (!newEmails.Contains(m.Email)) {
                        db.ExecuteDbQuery(Queries.DeleteMemberByList(name));
                    }
                }

                foreach (string email in newEmails) {
                    db.InsertRecord(Queries.InsertMember(name, email));
                }
                Close();
            }
        }

        private List<string> CollectData() {
            List<string> emails = new List<string>();
            int len = listBox_members.Items.Count;
            for (int i = 0; i < len; i++) {
                emails.Add(listBox_members.Items[i].ToString());
            }
            return emails;
        }

        private bool IsValid() {
            bool valid = true;
            if (textBox_name.Text == null || textBox_name.Text == "") {
                valid = false;
                textBox_name.Background = Brushes.MistyRose;
            }
            if (listBox_members.Items.Count <= 0) {
                valid = false;
                MessageBox.Show(Application.Current.MainWindow, "Please add at least one contact.", "Contact Selection");
                this.Focus();
            }

            return valid;
        }

        private void LoadListData(string listName) {
            List<MailingList> listNames = db.ListQuery();
            var listsQuery = from mailer in listNames
                                  where  mailer.Name == listName
                                  select mailer;
            foreach(MailingList name in listsQuery) {
                textBox_name.Text = name.Name;
            }

            List<Member> members = db.MemberQuery();
            var membersQuery = from member in members
                               where member.ListName == listName
                               select member;
            foreach(Member m in membersQuery) {
                listBox_members.Items.Add(m.Email);
                emails.Add(m.Email);
            }

        }
    }
}
