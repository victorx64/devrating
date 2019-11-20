using DevRating.Database;
using DevRating.EloRating;
using DevRating.SqlServerClient;
using Microsoft.Data.SqlClient;

namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            new Application(
                new DefaultArguments(args),
                new SqlServerInstance(
                    new TransactedDbConnection(
                        new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=devrating;" +
                                          "Integrated Security=True;Persist Security Info=False;" +
                                          "Pooling=False;MultipleActiveResultSets=False;Encrypt=False;" +
                                          "TrustServerCertificate=False")),
                    new EloFormula())).Run();
        }
    }
}