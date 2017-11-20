using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HolidayMailer {
    /// Brian Mize
    /// CSCD 371
    /// Holiday Mailer
    /// 
    public class Database {
        private static string DATABASE_NAME = "ContactsDb.sqlite";
        private static string CONTACTS = "Contacts";
        private static string LISTS = "Lists";
        private static string LIST_MEMBERS = "ListMembers";

        public static string DatabaseName {
            get { return DATABASE_NAME; }
        }

        public static string ContactsTable {
            get { return CONTACTS; }
        }

        public static string ListsTable {
            get { return LISTS; }
        }

        public static string ListMembersTable {
            get { return LIST_MEMBERS; }
        }

        public Database() {
            initDB();
        }

        /// <summary>
        /// Initialize the database
        /// </summary>
        private void initDB() {
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

                MessageBox.Show(ex.Message);
            }
            finally {
                conn.Close();
            }

        }

        /// <summary>
        /// Returns a connection to the database
        /// </summary>
        /// <returns>SQLiteConnection</returns>
        private SQLiteConnection GetConnection() {
            SQLiteConnection conn = new SQLiteConnection(String.Format("Data Source={0}; Version=3;", DATABASE_NAME));
            return conn;
        }

        /// <summary>
        /// Insert a record into the database.
        /// </summary>
        public void InsertRecord(string query) {
            try {
                SQLiteConnection conn = GetConnection();
                conn.Open();
                SQLiteCommand c2 = new SQLiteCommand(query, conn);
                c2.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Populate the given control with the results of the given database query.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="commandText"></param>
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
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Populate the given ListBox with the results of the given database query.
        /// </summary>
        /// <param name="listbox"></param>
        /// <param name="commandText"></param>
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
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Query the database for all the mailing lists.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Query the database for all contacts
        /// </summary>
        /// <returns></returns>
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

                MessageBox.Show(ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Query the database for all contacts that are part of a list.
        /// </summary>
        /// <returns></returns>
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

                MessageBox.Show(ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Delete a row given the passed in command.
        /// </summary>
        /// <param name="commandText"></param>
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
