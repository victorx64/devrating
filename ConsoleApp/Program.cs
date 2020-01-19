using DevRating.Domain;
using DevRating.EloRating;
using DevRating.SqliteClient;
using Microsoft.Data.Sqlite;

namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            new ConsoleArguments
            (
                args,
                new ConsoleApplication(
                    new SqliteDatabase
                    (
                        new TransactedDbConnection
                        (
                            new SqliteConnection("Data Source=devrating.db")
                        )
                    ),
                    new EloFormula()
                )
            ).Run();
        }
    }
}