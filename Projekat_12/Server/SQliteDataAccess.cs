using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SQliteDataAccess
    {
        public static List<string> ReadMessages()
        {
            using(IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var ret = connection.Query<string>("SELECT Text FROM Message", new DynamicParameters());
                return ret.ToList();
            }
        }

        public static void WriteMessage(string text)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                List<int> idx = connection.Query<int>("SELECT MAX(ID) FROM Message", new DynamicParameters()).ToList();
                Console.WriteLine(idx[0]);
                Message me = new Message(++idx[0], text);
                connection.Execute("insert into Message (id ,text) values (@Id, @Text)", me);
                
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
