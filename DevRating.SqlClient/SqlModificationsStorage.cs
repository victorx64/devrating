using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using DevRating.Domain.Git;
using DevRating.Domain.RatingSystem;
using DevRating.SqlClient.Collections;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    public sealed class SqlModificationsStorage : ModificationsStorage
    {
        private readonly IDbConnection _connection;
        private readonly Formula _formula;

        public SqlModificationsStorage(string connection, Formula formula)
            : this(new SqlConnection(connection), formula)
        {
        }

        public SqlModificationsStorage(IDbConnection connection, Formula formula)
            : this(new TransactedDbConnection(connection), formula)
        {
        }

        internal SqlModificationsStorage(TransactedDbConnection connection, Formula formula)
        {
            _connection = connection;
            _formula = formula;
        }

        public string Insert(IEnumerable<Addition> additions, IEnumerable<Deletion> deletions)
        {
            var builder = new StringBuilder();

            _connection.Open();

            var transaction = _connection.BeginTransaction();

            try
            {
                var authors = new SqlAuthorsCollection(_connection);
                var rewards = new SqlRewardsCollection(_connection);
                var matches = new SqlMatchesCollection(_connection);
                var ratings = new SqlRatingsCollection(_connection);

                builder.AppendLine(InsertRewards(additions, authors, rewards, ratings));
                builder.AppendLine(InsertRatings(deletions, authors, matches, ratings));

                transaction.Commit();

                return builder.ToString();
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

        private string InsertRewards(IEnumerable<Addition> additions,
            AuthorsCollection authors,
            RewardsCollection rewards,
            RatingsCollection ratings)
        {
            var builder = new StringBuilder("Rewards (additions)" + Environment.NewLine);

            foreach (var addition in additions)
            {
                var author = AuthorId(authors, addition.Commit().Author());

                var commit = addition.Commit();

                if (ratings.HasRating(author))
                {
                    var rating = ratings.LastRatingOf(author);

                    var reward = _formula.Reward(rating.Value(), addition.Count());

                    builder.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "In {0} {1} added {2} lines. DR {3:F2}. Reward {4:F4}",
                        addition.Commit().Sha(),
                        addition.Commit().Author(),
                        addition.Count(),
                        rating.Value(),
                        reward));

                    rewards.NewReward(reward, commit.Sha(), commit.Repository(), addition.Count(), rating.Id(), author);
                }
                else
                {
                    var reward = _formula.Reward(_formula.DefaultRating(), addition.Count());

                    builder.AppendLine(
                        string.Format(CultureInfo.InvariantCulture,
                            "In {0} {1} added {2} lines. DR {3:F2}. Reward {4:F2}",
                            addition.Commit().Sha(),
                            addition.Commit().Author(),
                            addition.Count(),
                            _formula.DefaultRating(),
                            reward));

                    rewards.NewReward(reward, commit.Sha(), commit.Repository(), addition.Count(), author);
                }
            }

            return builder.ToString();
        }

        private string InsertRatings(IEnumerable<Deletion> deletions,
            AuthorsCollection authors,
            MatchesCollection matches,
            RatingsCollection ratings)
        {
            var builder = new StringBuilder("Ratings (deletions)" + Environment.NewLine);

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

                builder.AppendLine(
                    string.Format(CultureInfo.InvariantCulture,
                        "In {0} deleted {1} lines from {2}",
                        deletion.Commit().Sha(),
                        deletion.Count(),
                        deletion.PreviousCommit().Sha()));

                builder.AppendLine(
                    string.Format(CultureInfo.InvariantCulture,
                        "{0} Old DR {1:F2}. New DR {2:F2}",
                        deletion.Commit().Author(),
                        pair.WinnerRating(),
                        pair.WinnerNewRating()));

                builder.AppendLine(
                    string.Format(CultureInfo.InvariantCulture,
                        "{0} Old DR {1:F2}. New DR {2:F2}",
                        deletion.PreviousCommit().Author(),
                        pair.LoserRating(),
                        pair.LoserNewRating()));

                builder.AppendLine();

                InsertNewRating(ratings, winner, pair.WinnerNewRating(), match);
                InsertNewRating(ratings, loser, pair.LoserNewRating(), match);
            }

            return builder.ToString();
        }

        private int AuthorId(AuthorsCollection authors, string email)
        {
            return (authors.Exist(email) ? authors.Author(email) : authors.NewAuthor(email)).Id();
        }

        private void InsertNewRating(RatingsCollection ratings, int author, double rating, int match)
        {
            if (ratings.HasRating(author))
            {
                var last = ratings.LastRatingOf(author).Id();

                ratings.NewRating(author, rating, last, match);
            }
            else
            {
                ratings.NewRating(author, rating, match);
            }
        }
    }
}