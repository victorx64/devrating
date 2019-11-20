using DevRating.Database;
using DevRating.EloRating;
using DevRating.SqliteClient;
using DevRating.SqlServerClient;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            new DefaultApplication(
                new DefaultArguments(args),
                new SqliteInstance(
                    new TransactedDbConnection(
                        new SqliteConnection("Data Source=local.db")),
                    new EloFormula())).Run();
        }
    }
}