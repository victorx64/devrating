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
            var arguments = new DefaultArguments(args);

            new Application(
                    new LibGit2Diff(arguments.Path(), arguments.StartCommit(), arguments.EndCommit()),
                    new SqlServerInstance(
                        new TransactedDbConnection(
                            new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DevRating;" +
                                              "Integrated Security=True;Persist Security Info=False;" +
                                              "Pooling=False;MultipleActiveResultSets=False;Encrypt=False;" +
                                              "TrustServerCertificate=False")),
                        new EloFormula()))
                .Run(arguments.Command());
        }
    }
}