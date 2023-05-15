using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Essentials;
using System.Reflection;
using Xamarin.Forms;
using System.Linq;

namespace KsiazkaXAMARIN
{
    public static class Functions
    {
        static SQLiteConnection con;
        public static int max_users = 5;
        public static Action refresh;
        public static string Table_name = "persons";

        public static void Open()
        {
            if(con != null)
            {
                return;
            }

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "users.db");
            con = new SQLiteConnection(dbPath);
            con.Execute($"CREATE TABLE IF NOT EXISTS {Table_name} (id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE, Name STRING, Surname STRING, Number TEXT, Email STRING)");

            if (refresh != null)
            {
                refresh();
            }
        }

        public static void AddTable(string tableName)
        {
            con.Execute($"CREATE TABLE IF NOT EXISTS {tableName} (id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE, Name STRING, Surname STRING, Number TEXT, Email STRING)");
            Table_name = tableName;

            if (refresh != null)
            {
                refresh();
            }
        }

        public static int MaxPage(string query = "")
        {
            string where = "";

            if (query != "")
            {
                where = $" WHERE Name || ' ' || Surname LIKE '%{query}%'";
            }

            var data = con.ExecuteScalar<int>($"SELECT (COUNT(*) / {max_users}) FROM {Table_name} {where}");

            return data;
        }

        public static void AddPerson(Person person)
        {
            con.Execute($"INSERT INTO {Table_name} (Name, Surname, Number, Email) VALUES ('{person.Name}','{person.Surname}','{person.Number}','{person.Email}')");
            if (refresh != null)
            {
                refresh();
            }
        }

        public static void EditPerson(Person person, Person oldPerson)
        {
            con.Execute($"UPDATE {Table_name} SET Name='{person.Name}',Surname='{person.Surname}',Number='{person.Number}',Email='{person.Email}' WHERE Name='{oldPerson.Name}' AND Surname='{oldPerson.Surname}' AND Number='{oldPerson.Number}' AND Email='{oldPerson.Email}'");
            if (refresh != null)
            {
                refresh();
            }
        }

        public static void DeletePerson(Person person)
        {
            con.Execute($"DELETE FROM {Table_name} WHERE Name='{person.Name}' AND Surname='{person.Surname}' AND Number='{person.Number}' AND Email='{person.Email}'");
            if (refresh != null)
            {
                refresh();
            }
        }

        public static List<Person> GetPersons(int page = 0, string query = "")
        {
            string where = "";

            if (query != "")
            {
                where = $"WHERE Name || ' ' || Surname LIKE '%{query}%'";
            }

            var data = con.Query<Person>($"SELECT * FROM {Table_name} {where} LIMIT {page * max_users}, {max_users}");

            return data.ToList();
        }
    }
}
