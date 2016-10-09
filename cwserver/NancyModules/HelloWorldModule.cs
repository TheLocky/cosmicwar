using Nancy;
using System.Data.SQLite;

namespace cwserver.NancyModules
{
    public class HelloWorldModule : NancyModule {
        public HelloWorldModule() {
            using (SQLiteCommand command = DBConnection.GetCommand()) {
                command.CommandText = @"SELECT name FROM users";
                string name = command.ExecuteScalar() as string;
                Get["/"] = x => "Hello " + name;
            }
        }
    }
}
