using DevRating.Database;
using DevRating.EloRating;
using DevRating.SqliteClient;
using Microsoft.Data.Sqlite;

namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            new ConsoleApplication(
                new ConsoleArguments(args),
                new SqliteInstance(
                    new TransactedDbConnection(
                        new SqliteConnection("Data Source=local.db")),
                    new EloFormula())).Run();
        }
    }
}