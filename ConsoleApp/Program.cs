using System;
using System.Threading.Tasks;
using DevRating.Domain.Git;
using DevRating.Domain.RatingSystem;
using DevRating.LibGit2SharpClient;
using DevRating.SqlClient;
using Microsoft.Data.SqlClient;

namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var modifications = new DefaultModificationsCollection();

            var repository = new LibGit2Repository(args[0]);

            foreach (var commit in repository.Commits(args[1], args[2]))
            {
                await commit.WriteInto(modifications);
            }

            var connection = new TransactedDbConnection(new SqlConnection(args[3]));

            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                var storage = new SqlModificationsStorage(connection, new EloFormula());

                modifications.InsertAdditionsTo(storage);
                modifications.InsertDeletionsTo(storage);

                Console.WriteLine("Rewards:");
                
                foreach (var commit in repository.Commits(args[1], args[2]))
                {
                    foreach (var reward in storage.RewardsOf(commit))
                    {
                        Console.WriteLine($"{reward.Author()} {reward.Value():F2} {reward.HasRating()}");
                    }
                }

                Console.WriteLine("New Ratings:");
                
                foreach (var commit in repository.Commits(args[1], args[2]))
                {
                    foreach (var rating in storage.RatingsOf(commit))
                    {
                        Console.WriteLine($"{rating.Author()} {rating.Value():F2}");
                    }
                }

                //transaction.Commit();
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}