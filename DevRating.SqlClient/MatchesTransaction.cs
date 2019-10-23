using System.Collections.Generic;
using System.Data;
using DevRating.Git;
using DevRating.Rating;
using DevRating.SqlClient.Collections;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    public sealed class MatchesTransaction : Modifications, Transaction
    {
        private readonly IDbConnection _connection;
        private readonly Formula _formula;
        private readonly IList<Addition> _additions;
        private readonly IList<Deletion> _deletions;

        public MatchesTransaction(string connection, Formula formula) : this(new SqlConnection(connection), formula)
        {
        }

        public MatchesTransaction(IDbConnection connection, Formula formula)
        {
            _connection = connection;
            _formula = formula;
            _additions = new List<Addition>();
            _deletions = new List<Deletion>();
        }

        public void Start()
        {
            _additions.Clear();
            _deletions.Clear();
        }

        public void AddAddition(Addition addition)
        {
            _additions.Add(addition);
        }

        public void AddDeletion(Deletion deletion)
        {
            _deletions.Add(deletion);
        }

        public void Commit()
        {
            _connection.Open();

            var transaction = _connection.BeginTransaction();

            var authors = new DbAuthorsCollection(transaction);
            var matches = new DbMatchesCollection(transaction);
            var ratings = new DbRatingsCollection(transaction);
            var rewards = new DbRewardsCollection(transaction);

            try
            {
                PushDeletionsInto(authors, matches, ratings);
                PushAdditionsInto(authors, rewards, ratings);

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

        private void PushAdditionsInto(AuthorsCollection authors, RewardsCollection rewards, RatingsCollection ratings)
        {
            foreach (var addition in _additions)
            {
                var email = addition.Author().Email();

                var author = AuthorId(authors, email);

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

        private void PushDeletionsInto(AuthorsCollection authors, MatchesCollection matches, RatingsCollection ratings)
        {
            foreach (var deletion in _deletions)
            {
                var author = deletion.Author().Email();
                var victim = deletion.Victim().Email();

                if (author.Equals(victim))
                {
                    continue;
                }

                var winner = AuthorId(authors, author);
                var loser = AuthorId(authors, victim);

                var match = matches
                    .NewMatch(
                        winner,
                        loser,
                        deletion.Commit().Sha(),
                        deletion.Commit().Repository(),
                        deletion.Count())
                    .Id();

                CreateNewRatingsPair(winner, loser, ratings, deletion.Count(), match);
            }
        }

        private int AuthorId(AuthorsCollection authors, string email)
        {
            return (authors.Exist(email) ? authors.Author(email) : authors.NewAuthor(email)).Id();
        }

        private void CreateNewRatingsPair(int winner, int loser, RatingsCollection ratings, int count, int match)
        {
            var pair = new RatingsPair(winner, loser, ratings, _formula);

            CreateNewRating(winner, ratings, pair.WinnerNewRating(count), match);
            CreateNewRating(loser, ratings, pair.LoserNewRating(count), match);
        }

        private void CreateNewRating(int player, RatingsCollection ratings, double rating, int match)
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