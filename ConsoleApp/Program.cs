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
            new ConsoleArguments(
                    args,
                    new ConsoleApplication(
                        new SqliteInstance(
                            new TransactedDbConnection(
                                new SqliteConnection("Data Source=devrating.db")),
                            new EloFormula()),
                        new EloFormula()))
                .Run();
        }
    }
}