using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevRating.Domain;
using DevRating.LibGit2SharpClient;
using DevRating.SqlClient;
using Microsoft.Data.SqlClient;

namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var repository = new LibGit2Repository(args[0]);
            
            var additions = new List<Addition>();
            var deletions = new List<Deletion>();
            
            foreach (var commit in repository.Commits(args[1], args[2]))
            {
                await commit.WriteInto(additions, deletions);
            }

            var connection = new TransactedDbConnection(new SqlConnection(args[3]));

            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                var storage = new SqlModificationsStorage(connection, new EloFormula());

                storage.InsertAdditions(NonDeletedAdditions(additions, deletions));
                storage.InsertDeletions(NonSelfDeletions(deletions));

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
        
        public static IEnumerable<Addition> NonDeletedAdditions(IEnumerable<Addition> additions, IEnumerable<Deletion> deletions)
        {
            foreach (var addition in additions)
            {
                yield return addition.NewAddition(
                    addition.Count() -
                    AddedThenDeletedLinesCount(addition.Commit(), deletions));
            }
        }

        private static uint AddedThenDeletedLinesCount(Commit commit, IEnumerable<Deletion> deletions)
        {
            foreach (var deletion in deletions)
            {
                if (deletion.PreviousCommit().Equals(commit) &&
                    deletion.PreviousCommit().AuthorEmail().Equals(deletion.Commit().AuthorEmail()))
                {
                    return deletion.Count();
                }
            }

            return 0;
        }
        
        public static IEnumerable<Deletion> NonSelfDeletions(IEnumerable<Deletion> deletions)
        {
            foreach (var deletion in deletions)
            {
                if (!deletion.Commit().AuthorEmail().Equals(deletion.PreviousCommit().AuthorEmail()))
                {
                    yield return deletion;
                }
            }
        }
    }
}