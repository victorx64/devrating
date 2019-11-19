using DevRating.Database;
using DevRating.EloRating;
using DevRating.LibGit2SharpClient;
using DevRating.SqlServerClient;
using Microsoft.Data.SqlClient;

namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var command = args[0];
            var path = args[1];
            var start = args[2];
            var end = args[3];
            var database =
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DevRating;" +
                @"Integrated Security=True;Persist Security Info=False;Pooling=False;" +
                @"MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";

            var connection = new TransactedDbConnection(new SqlConnection(database));

            var application = new Application(new LibGit2Diff(start, end, path),
                connection,
                new DbWorksRepository(new SqlWorks(connection),
                    new SqlAuthors(connection),
                    new SqlRatings(connection),
                    new EloFormula()));

            if (command.Equals("show"))
            {
                application.PrintToConsole();
            }
            else if (command.Equals("show-saved"))
            {
                application.PrintSavedToConsole();
            }
            else if (command.Equals("save"))
            {
                application.Save();
            }
            else
            {
                throw new System.Exception("Command is not recognized");
            }
        }
    }
}