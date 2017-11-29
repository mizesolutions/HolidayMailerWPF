using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HolidayMailer {

    public class Database {

        private const string DATABASE_NAME = "ContactsDb.sqlite";
        private const string CONTACTS = "Contacts";
        private const string LISTS = "Lists";
        private const string LIST_MEMBERS = "ListMembers";

        public static string DatabaseName { get => DATABASE_NAME; }

        public static string ContactsTable {  get => CONTACTS; }
 
        public static string ListsTable { get => LISTS; }

        public static string ListMembersTable { get => LIST_MEMBERS; }

        public Database() {
            InitDB();
        }

        private void InitDB() {
            if (!File.Exists(DATABASE_NAME)) {
                SQLiteConnection.CreateFile(DATABASE_NAME);
            }

            SQLiteConnection conn = GetConnection();
            try {
                conn.Open();
                string q = String.Format(
                    "CREATE TABLE IF NOT EXISTS {0} (FirstName VARCHAR(20), LastName VARCHAR(10), Email VARCHAR(20), RecievedMail BOOLEAN, UNIQUE (Email))",
                    CONTACTS);
                SQLiteCommand c = new SQLiteCommand(q, conn);
                c.ExecuteNonQuery();

                q = String.Format(
                    "CREATE TABLE IF NOT EXISTS {0} (ListName VARCHAR(40), UNIQUE (ListName))",
                    LISTS);

                c = new SQLiteCommand(q, conn);
                c.ExecuteNonQuery();

                q = String.Format(
                    "CREATE TABLE IF NOT EXISTS {0} (ListName VARCHAR(40), Email VARCHAR(20), UNIQUE(ListName, Email))",
                    LIST_MEMBERS);

                c = new SQLiteCommand(q, conn);
                c.ExecuteNonQuery();

            }
            catch (Exception ex) {

                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }
            finally {
                conn.Close();
            }

        }

        private SQLiteConnection GetConnection() {
            SQLiteConnection conn = new SQLiteConnection(String.Format("Data Source={0}; Version=3;", DATABASE_NAME));
            return conn;
        }

        public void InsertRecord(string query) {
            try {
                SQLiteConnection conn = GetConnection();
                conn.Open();
                SQLiteCommand c2 = new SQLiteCommand(query, conn);
                c2.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }

        }

        public void LoadDataGrid(ItemsControl control, string commandText) {
            try {

                using (SQLiteCommand command = new SQLiteCommand(commandText, GetConnection()))
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command)) {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    control.ItemsSource = dataTable.AsDataView();
                }
            }
            catch (Exception ex) {
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }
        }

        public void LoadListBox(ListBox listbox, string commandText) {
            try {
                SQLiteConnection connect = GetConnection();
                connect.Open();
                SQLiteCommand command = connect.CreateCommand();
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    listbox.Items.Add(reader[0]);
                    
                }
            }
            catch (Exception ex) {
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }
        }

        public List<MailingList> ListQuery() {
            List<MailingList> list = new List<MailingList>();
            try {
                SQLiteDataReader reader = ExcecuteQuery(Queries.SelectAll(LISTS));
                while (reader.Read()) {
                    MailingList mailer = new MailingList();
                    mailer.Name = reader[0].ToString();
                    list.Add(mailer);
                }
            }
            catch (Exception) {

                throw;
            }
            return list;
        }

        public List<Person> PersonQuery() {
            List<Person> list = new List<Person>();
            try {
                SQLiteDataReader reader = ExcecuteQuery(Queries.SelectAll(CONTACTS));
                while (reader.Read()) {
                    Person person = new Person();
                    person.Fname = reader[0].ToString();
                    person.Lname = reader[1].ToString();
                    person.Email = reader[2].ToString();
                    person.Recieved = (bool) reader[3];
                    list.Add(person);
                }

            }
            catch (Exception ex) {

                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }
            return list;
        }

        public List<Member> MemberQuery() {
            List<Member> list = new List<Member>();
            try {
                SQLiteDataReader reader = ExcecuteQuery(Queries.SelectAll(LIST_MEMBERS));
                while (reader.Read()) {
                    Member member = new Member();
                    member.ListName = reader[0].ToString();
                    member.Email = reader[1].ToString();
                    list.Add(member);
                }
            }
            catch (Exception ex) {

                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }
            return list;
        }

        public void ExecuteDbQuery(string commandText) {
            using (SQLiteConnection conn = GetConnection()) {
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(commandText, conn);
                command.ExecuteNonQuery();
            }
        }

        private SQLiteDataReader ExcecuteQuery(string commandText) {
            SQLiteConnection connect = GetConnection();
            connect.Open();
            SQLiteCommand command = connect.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            SQLiteDataReader reader = command.ExecuteReader();
            return reader;
        }


    }
}
