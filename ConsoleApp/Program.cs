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
            new Application(
                    new LibGit2Diff(args[1], args[2], args[3]),
                    new DbWorksRepository(
                        new SqlServerEntities(
                            new TransactedDbConnection(
                                new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DevRating;" +
                                                  "Integrated Security=True;Persist Security Info=False;Pooling=False;" +
                                                  "MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False"))),
                        new EloFormula()))
                .HandleCommand(args[0]);
        }
    }
}