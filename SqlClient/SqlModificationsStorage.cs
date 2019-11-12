using System.Collections.Generic;
using System.Data;
using DevRating.Domain;
using DevRating.Domain.Git;
using DevRating.Domain.RatingSystem;
using DevRating.SqlClient.Collections;
using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient
{
    public sealed class SqlModificationsStorage : ModificationsStorage
    {
        private readonly Formula _formula;
        private readonly AuthorsCollection _authors;
        private readonly RewardsCollection _rewards;
        private readonly MatchesCollection _matches;
        private readonly RatingsCollection _ratings;

        public SqlModificationsStorage(IDbConnection connection, Formula formula)
            : this(
                new SqlAuthorsCollection(connection),
                new SqlRewardsCollection(connection),
                new SqlMatchesCollection(connection),
                new SqlRatingsCollection(connection),
                formula)
        {
        }

        internal SqlModificationsStorage(
            AuthorsCollection authors,
            RewardsCollection rewards,
            MatchesCollection matches,
            RatingsCollection ratings,
            Formula formula)
        {
            _formula = formula;
            _authors = authors;
            _rewards = rewards;
            _matches = matches;
            _ratings = ratings;
        }

        public void InsertAdditions(IEnumerable<Addition> additions)
        {
            foreach (var addition in additions)
            {
                var author = Author(addition.Commit().AuthorEmail());

                var commit = addition.Commit();

                if (_ratings.HasRatingOf(author))
                {
                    var rating = _ratings.LastRatingOf(author);

                    _rewards.Insert(
                        _formula.Reward(
                            rating.Value(),
                            addition.Count()),
                        commit.Sha(),
                        commit.RepositoryFirstUrl(),
                        addition.Count(),
                        rating,
                        author);
                }
                else
                {
                    _rewards.Insert(
                        _formula.Reward(
                            _formula.DefaultRating(),
                            addition.Count()),
                        commit.Sha(),
                        commit.RepositoryFirstUrl(),
                        addition.Count(),
                        author);
                }
            }
        }

        public void InsertDeletions(IEnumerable<Deletion> deletions)
        {
            foreach (var deletion in deletions)
            {
                var winner = Author(deletion.Commit().AuthorEmail());
                var loser = Author(deletion.PreviousCommit().AuthorEmail());

                var match = _matches
                    .Insert(
                        winner,
                        loser,
                        deletion.Commit().Sha(),
                        deletion.Commit().RepositoryFirstUrl(),
                        deletion.Count());

                var pair = new RatingsPair(winner, loser, _ratings, _formula, deletion.Count());

                InsertRating(winner, pair.WinnerNewRating(), match);
                InsertRating(loser, pair.LoserNewRating(), match);
            }
        }

        public IEnumerable<Reward> RewardsOf(Commit commit)
        {
            return _rewards.RewardsOf(commit.Sha(), commit.RepositoryFirstUrl());
        }

        public IEnumerable<Rating> RatingsOf(Commit commit)
        {
            return _ratings.RatingsOf(commit.Sha(), commit.RepositoryFirstUrl());
        }

        private IdentifiableAuthor Author(string email)
        {
            return _authors.Exist(email)
                ? _authors.Author(email)
                : _authors.Insert(email);
        }

        private void InsertRating(IdentifiableAuthor author, double rating, IdentifiableObject match)
        {
            if (_ratings.HasRatingOf(author))
            {
                var last = _ratings.LastRatingOf(author);

                _ratings.Insert(author, rating, last, match);
            }
            else
            {
                _ratings.Insert(author, rating, match);
            }
        }
    }
}