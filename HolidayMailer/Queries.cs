using System;

namespace HolidayMailer
{
    /// <summary>
    /// Holds all queries for database operations.
    /// </summary>
    class Queries
    {
        /// <summary>
        /// Inserts contact
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="recieved"></param>
        /// <returns></returns>
        public static string InsertContact(string firstName, string lastName, string email, bool recieved)
        {
            if (firstName == null || lastName == null || email == null) throw new ArgumentNullException();
            var q = $"INSERT OR IGNORE INTO {Database.ContactsTable} (FirstName, LastName, Email, RecievedMail) values ('{firstName}', '{lastName}', '{email}', '{recieved}')";
            return q;
        }

        /// <summary>
        /// Inserts list
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public static string InsertList(string listName)
        {
            if (listName == null) throw new ArgumentNullException();
            var q = $"INSERT OR IGNORE INTO {Database.ListsTable} (ListName) values ('{listName}')";
            return q;
        }

        /// <summary>
        /// Inserts member
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string InsertMember(string listName, string email)
        {
            if (listName == null || email == null) throw new ArgumentNullException();
            var q = $"INSERT OR IGNORE INTO {Database.ListMembersTable} (ListName, Email) VALUES ('{listName}', '{email}')";
            return q;
        }

        /// <summary>
        /// Deletes contact by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string DeleteContact(string email)
        {
            if (email == null) throw new ArgumentNullException();
            var q = $"DELETE FROM {Database.ContactsTable} WHERE Email = '{email}'";
            return q;
        }

        /// <summary>
        /// Deletes list by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string DeleteList(string name)
        {
            if (name == null) throw new ArgumentNullException();
            var q = $"DELETE FROM {Database.ListsTable} WHERE ListName = '{name}'";
            return q;
        }

        /// <summary>
        /// Deletes member by list
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public static string DeleteMemberByList(string listName)
        {
            if (listName == null) throw new ArgumentNullException();
            var q = $"DELETE FROM {Database.ListMembersTable} WHERE ListName = '{listName}'";
            return q;
        }

        /// <summary>
        /// Deletes member by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string DeleteMemberByEmail(string email)
        {
            if (email == null) throw new ArgumentNullException();
            var q = $"DELETE FROM {Database.ListMembersTable} WHERE Email = '{email}'";
            return q;
        }

        /// <summary>
        /// Updates list
        /// </summary>
        /// <param name="newListName"></param>
        /// <param name="oldListName"></param>
        /// <returns></returns>
        public static string UpdateListRecord(string newListName, string oldListName)
        {
            if (newListName == null || oldListName == null) throw new ArgumentNullException();
            var q = $"UPDATE {Database.ListsTable} SET ListName = '{newListName}' WHERE ListName = '{oldListName}'";
            return q;
        }

        /// <summary>
        /// Updates contact
        /// </summary>
        /// <param name="oldEmail"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="email"></param>
        /// <param name="recieved"></param>
        /// <returns></returns>
        public static string UpdateContactRecord(string oldEmail, string fName, string lName, string email, bool recieved)
        {
            if (fName == null || lName == null || email == null) throw new ArgumentNullException();
            var q =
                $"UPDATE {Database.ContactsTable} SET FirstName = '{fName}', LastName = '{lName}', Email = '{email}', RecievedMail = '{recieved}'  WHERE Email = '{oldEmail}'";
            return q;
        }

        /// <summary>
        /// Updates member by email
        /// </summary>
        /// <param name="oldEmail"></param>
        /// <param name="newEmail"></param>
        /// <returns></returns>
        public static string UpdateMemberRecordByEmail(string oldEmail, string newEmail)
        {
            if (oldEmail == null || newEmail == null) throw new ArgumentNullException();
            var q = $"UPDATE {Database.ListMembersTable} SET Email = '{newEmail}' WHERE Email = '{oldEmail}'";
            return q;
        }

        /// <summary>
        /// Uodates member by list
        /// </summary>
        /// <param name="oldList"></param>
        /// <param name="newList"></param>
        /// <returns></returns>
        public static string UpdateMemberRecordByList(string oldList, string newList)
        {
            if (oldList == null || newList == null) throw new ArgumentNullException();
            var q = $"UPDATE {Database.ListMembersTable} SET ListName = '{oldList}' WHERE ListName = '{newList}'";
            return q;
        }

        /// <summary>
        /// Gets all data from table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string SelectAll(string table)
        {
            if (table == null) throw new ArgumentNullException();
            return $"SELECT * FROM {table}";
        }

        /// <summary>
        /// Searches contact by name
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static string SelectContactsByLastName(string searchString)
        {
            if (searchString == null) throw new ArgumentNullException();
            return $"SELECT * FROM {Database.ContactsTable} WHERE LastName LIKE '%{searchString}%'";
        }
    }
}
