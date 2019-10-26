using System.Collections.Generic;
using System.Data;
using DevRating.Vcs;
using DevRating.Rating;
using DevRating.SqlClient.Collections;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    public sealed class MatchesTransaction
    {
        private readonly IDbConnection _connection;
        private readonly Formula _formula;

        public MatchesTransaction(string connection, Formula formula)
            : this(new SqlConnection(connection), formula)
        {
        }

        public MatchesTransaction(IDbConnection connection, Formula formula)
        {
            _connection = connection;
            _formula = formula;
        }

        public void Commit(IEnumerable<Addition> additions, IEnumerable<Deletion> deletions)
        {
            _connection.Open();

            var transaction = _connection.BeginTransaction();

            try
            {
                var authors = new DbAuthorsCollection(transaction);
                var matches = new DbMatchesCollection(transaction);
                var ratings = new DbRatingsCollection(transaction);
                var rewards = new DbRewardsCollection(transaction);

                InsertAdditions(additions, authors, rewards, ratings);
                InsertDeletions(deletions, authors, matches, ratings);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();

                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        private void InsertAdditions(
            IEnumerable<Addition> additions,
            AuthorsCollection authors,
            RewardsCollection rewards,
            RatingsCollection ratings)
        {
            foreach (var addition in additions)
            {
                var author = AuthorId(authors, addition.Commit().Author());

                var commit = addition.Commit();

                if (ratings.HasRating(author))
                {
                    var rating = ratings.LastRatingOf(author);

                    var reward = _formula.Reward(rating.Value(), addition.Count());

                    rewards.NewReward(reward, commit.Sha(), commit.Repository(), addition.Count(), rating.Id());
                }
                else
                {
                    var reward = _formula.Reward(_formula.DefaultRating(), addition.Count());

                    rewards.NewReward(reward, commit.Sha(), commit.Repository(), addition.Count());
                }
            }
        }

        private void InsertDeletions(
            IEnumerable<Deletion> deletions,
            AuthorsCollection authors,
            MatchesCollection matches,
            RatingsCollection ratings)
        {
            foreach (var deletion in deletions)
            {
                var winner = AuthorId(authors, deletion.Commit().Author());
                var loser = AuthorId(authors, deletion.PreviousCommit().Author());

                var match = matches
                    .NewMatch(
                        winner,
                        loser,
                        deletion.Commit().Sha(),
                        deletion.Commit().Repository(),
                        deletion.Count())
                    .Id();

                var pair = new RatingsPair(winner, loser, ratings, _formula, deletion.Count());

                InsertNewRating(ratings, winner, pair.WinnerNewRating(), match);
                InsertNewRating(ratings, loser, pair.LoserNewRating(), match);
            }
        }

        private int AuthorId(AuthorsCollection authors, Author author)
        {
            var email = author.Email();

            return (authors.Exist(email) ? authors.Author(email) : authors.NewAuthor(email)).Id();
        }

        private void InsertNewRating(RatingsCollection ratings, int player, double rating, int match)
        {
            if (ratings.HasRating(player))
            {
                var last = ratings.LastRatingOf(player).Id();

                ratings.NewRating(player, rating, last, match);
            }
            else
            {
                ratings.NewRating(player, rating, match);
            }
        }
    }
}