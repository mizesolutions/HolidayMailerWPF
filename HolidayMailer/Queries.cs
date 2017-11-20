using System;

/// Brian Mize
/// CSCD 371
/// Holiday Mailer
/// 
namespace HolidayMailer {
    class Queries {

        public static string InsertContact(string firstName, string lastName, string email, bool recieved) {
            if (firstName == null || lastName == null || email == null) throw new ArgumentNullException();
            string q = String.Format("INSERT OR IGNORE INTO {0} (FirstName, LastName, Email, RecievedMail) values ('{1}', '{2}', '{3}', '{4}')",
                Database.ContactsTable,
                firstName,
                lastName,
                email,
                recieved
                );
            return q;
        }

        public static string InsertList(string ListName) {
            if (ListName == null) throw new ArgumentNullException();
            string q = String.Format("INSERT OR IGNORE INTO {0} (ListName) values ('{1}')", Database.ListsTable, ListName);
            return q;
        }

        public static string InsertMember(string ListName, string Email) {
            if (ListName == null || Email == null) throw new ArgumentNullException();
            string q = String.Format("INSERT OR IGNORE INTO {0} (ListName, Email) VALUES ('{1}', '{2}')", Database.ListMembersTable, ListName, Email);
            return q;
        }

        public static string DeleteContact(string email) {
            if (email == null) throw new ArgumentNullException();
            string q = String.Format("DELETE FROM {0} WHERE Email = '{1}'", Database.ContactsTable, email);
            return q;
        }

        public static string DeleteList(string name) {
            if (name == null) throw new ArgumentNullException();
            string q = String.Format("DELETE FROM {0} WHERE ListName = '{1}'", Database.ListsTable, name);
            return q;
        }

        public static string DeleteMemberByList(string listName) {
            if (listName == null) throw new ArgumentNullException();
            string q = String.Format("DELETE FROM {0} WHERE ListName = '{1}'", Database.ListMembersTable, listName);
            return q;
        }

        public static string DeleteMemberByEmail(string email) {
            if (email == null) throw new ArgumentNullException();
            string q = String.Format("DELETE FROM {0} WHERE Email = '{1}'", Database.ListMembersTable, email);
            return q;
        }

        public static string UpdateListRecord(string newListName, string oldListName) {
            if (newListName == null || oldListName == null) throw new ArgumentNullException();
            string q = String.Format("UPDATE {0} SET ListName = '{1}' WHERE ListName = '{2}'", Database.ListsTable, newListName, oldListName);
            return q;
        }

        public static string UpdateContactRecord(string oldEmail, string fName, string lName, string email, bool recieved) {
            if (fName == null || lName == null || email == null) throw new ArgumentNullException();
            string q = String.Format("UPDATE {0} SET FirstName = '{1}', LastName = '{2}', Email = '{3}', RecievedMail = '{4}'  WHERE Email = '{5}'", Database.ContactsTable, fName, lName, email, recieved, oldEmail);
            return q;
        }

        public static string UpdateMemberRecordByEmail(string oldEmail, string newEmail) {
            if (oldEmail == null || newEmail == null) throw new ArgumentNullException();
            string q = String.Format("UPDATE {0} SET Email = '{1}' WHERE Email = '{2}'",Database.ListMembersTable, newEmail, oldEmail);
            return q;
        }

        public static string UpdateMemberRecordByList(string oldList, string newList) {
            if (oldList == null || newList == null) throw new ArgumentNullException();
            string q = String.Format("UPDATE {0} SET ListName = '{1}' WHERE ListName = '{2}'", Database.ListMembersTable, oldList, newList);
            return q;
        }

        public static string SelectAll(string table) {
            if (table == null) throw new ArgumentNullException();
            return String.Format("SELECT * FROM {0}", table);
        }

        public static string SelectContactsByLastName(string searchString) {
            if (searchString == null) throw new ArgumentNullException();
            return String.Format("SELECT * FROM {0} WHERE LastName LIKE '%{1}%'", Database.ContactsTable, searchString);
        }
    }
}
