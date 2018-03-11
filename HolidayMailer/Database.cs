using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HolidayMailer
{

    /// <summary>
    /// Handles all interaction with the database.
    /// </summary>
    public class Database
    {
        public static string DatabaseName { get; } = "ContactsDb.sqlite";
        public static string ContactsTable { get; } = "Contacts";
        public static string ListsTable { get; } = "Lists";
        public static string ListMembersTable { get; } = "ListMembers";

        public Database()
        {
            InitializeDatabase();
        }

        /// <summary>
        /// Creates the needed database if it doesn't exist and creates the needed tables if they do not exist.
        /// </summary>
        private void InitializeDatabase()
        {
            if (!File.Exists(DatabaseName))
            {
                SQLiteConnection.CreateFile(DatabaseName);
            }
            var conn = GetConnection();
            try
            {
                conn.Open();
                var q = $"CREATE TABLE IF NOT EXISTS {ContactsTable} (FirstName VARCHAR(20), LastName VARCHAR(10), Email VARCHAR(20), RecievedMail BOOLEAN, UNIQUE (Email))";
                var c = new SQLiteCommand(q, conn);
                c.ExecuteNonQuery();

                q = $"CREATE TABLE IF NOT EXISTS {ListsTable} (ListName VARCHAR(40), UNIQUE (ListName))";
                c = new SQLiteCommand(q, conn);
                c.ExecuteNonQuery();

                q = $"CREATE TABLE IF NOT EXISTS {ListMembersTable} (ListName VARCHAR(40), Email VARCHAR(20), UNIQUE(ListName, Email))";
                c = new SQLiteCommand(q, conn);
                c.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Creates and returns a new connection to the database
        /// </summary>
        /// <returns></returns>
        private SQLiteConnection GetConnection()
        {
            var conn = new SQLiteConnection($"Data Source={DatabaseName}; Version=3;");
            return conn;
        }

        /// <summary>
        /// Takes the passed query and tries to insert it into the database.
        /// </summary>
        /// <param name="query"></param>
        public void InsertRecord(string query)
        {
            try
            {
                var conn = GetConnection();
                conn.Open();
                var c2 = new SQLiteCommand(query, conn);
                c2.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }

        }

        /// <summary>
        /// Loads data from database to update the Contact view.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="commandText"></param>
        public void LoadDataGrid(ItemsControl control, string commandText)
        {
            try
            {
                using (var command = new SQLiteCommand(commandText, GetConnection()))
                using (var dataAdapter = new SQLiteDataAdapter(command))
                {
                    var dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    control.ItemsSource = dataTable.AsDataView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }
        }

        /// <summary>
        /// Loads data from the database to update the list view
        /// </summary>
        /// <param name="listbox"></param>
        /// <param name="commandText"></param>
        public void LoadListBox(ListBox listbox, string commandText)
        {
            try
            {
                var connect = GetConnection();
                connect.Open();
                var command = connect.CreateCommand();
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listbox.Items.Add(reader[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }
        }

        /// <summary>
        /// Return the mailing list from the database
        /// </summary>
        /// <returns></returns>
        public List<MailingList> ListQuery()
        {
            var list = new List<MailingList>();
            var reader = ExcecuteQuery(Queries.SelectAll(ListsTable));
            while (reader.Read())
            {
                var mailer = new MailingList { Name = reader[0].ToString() };
                list.Add(mailer);
            }
            return list;
        }

        /// <summary>
        /// Builds list of people from the database.
        /// </summary>
        /// <returns></returns>
        public List<Person> PersonQuery()
        {
            var list = new List<Person>();
            try
            {
                var reader = ExcecuteQuery(Queries.SelectAll(ContactsTable));
                while (reader.Read())
                {
                    var person = new Person
                    {
                        Fname = reader[0].ToString(),
                        Lname = reader[1].ToString(),
                        Email = reader[2].ToString(),
                        Recieved = (bool)reader[3]
                    };
                    list.Add(person);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }
            return list;
        }

        /// <summary>
        /// Builds list of members.
        /// </summary>
        /// <returns></returns>
        public List<Member> MemberQuery()
        {
            var list = new List<Member>();
            try
            {
                var reader = ExcecuteQuery(Queries.SelectAll(ListMembersTable));
                while (reader.Read())
                {
                    var member = new Member
                    {
                        ListName = reader[0].ToString(),
                        Email = reader[1].ToString()
                    };
                    list.Add(member);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Application.Current.MainWindow, ex.Message, "Error");
            }
            return list;
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="commandText"></param>
        public void ExecuteDatabaseQuery(string commandText)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var command = new SQLiteCommand(commandText, conn);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        private SQLiteDataReader ExcecuteQuery(string commandText)
        {
            var connect = GetConnection();
            connect.Open();
            var command = connect.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
            var reader = command.ExecuteReader();
            return reader;
        }
    }
}
