using System.Collections.Generic;
using System.Data;
using DevRating.Domain;

namespace DevRating.Database
{
    public sealed class DbWorksRepository : WorksRepository
    {
        private readonly Works _works;
        private readonly Authors _authors;
        private readonly Ratings _ratings;
        private readonly IDbConnection _connection;
        private readonly Formula _formula;

        public DbWorksRepository(Entities entities, Formula formula)
            : this(entities.Works(), entities.Authors(), entities.Ratings(), entities.Connection(), formula)
        {
        }

        public DbWorksRepository(Works works, Authors authors, Ratings ratings, IDbConnection connection,
            Formula formula)
        {
            _works = works;
            _authors = authors;
            _ratings = ratings;
            _connection = connection;
            _formula = formula;
        }

        public IDbConnection Connection()
        {
            return _connection;
        }

        public void Add(WorkKey key, string email, uint additions, IDictionary<string, uint> deletions)
        {
            var author = Author(email);

            InsertAuthorNewRating(author, deletions, InsertedWork(key, additions, author));
        }

        private DbAuthor Author(string email)
        {
            return _authors.Exist(email)
                ? _authors.Author(email)
                : _authors.Insert(email);
        }

        private DbWork InsertedWork(WorkKey key, uint additions, DbObject author)
        {
            if (_ratings.HasRatingOf(author))
            {
                var rating = _ratings.RatingOf(author);

                return _works.Insert(key.Repository(), key.StartCommit(), key.EndCommit(), author,
                    _formula.Reward(rating.Value(), additions), rating);
            }
            else
            {
                return _works.Insert(key.Repository(), key.StartCommit(), key.EndCommit(), author,
                    _formula.Reward(_formula.DefaultRating(), additions));
            }
        }

        private double RatingOf(DbObject author)
        {
            return _ratings.HasRatingOf(author)
                ? _ratings.RatingOf(author).Value()
                : _formula.DefaultRating();
        }

        private void InsertAuthorNewRating(DbObject author, IDictionary<string, uint> deletions,
            DbObject work)
        {
            var current = RatingOf(author);

            var @new = _formula.WinnerNewRating(current, InsertVictimsNewRatings(deletions, work, current));

            if (_ratings.HasRatingOf(author))
            {
                _ratings.Insert(author, @new, _ratings.RatingOf(author), work);
            }
            else
            {
                _ratings.Insert(author, @new, work);
            }
        }

        private IEnumerable<Match> InsertVictimsNewRatings(IDictionary<string, uint> deletions, DbObject work,
            double rating)
        {
            var matches = new List<Match>();

            foreach (var deletion in deletions)
            {
                var victim = Author(deletion.Key);

                var current = RatingOf(victim);

                matches.Add(new DefaultMatch(current, deletion.Value));

                var @new = _formula.LoserNewRating(current, new DefaultMatch(rating, deletion.Value));

                if (_ratings.HasRatingOf(victim))
                {
                    _ratings.Insert(victim, @new, _ratings.RatingOf(victim), work);
                }
                else
                {
                    _ratings.Insert(victim, @new, work);
                }
            }

            return matches;
        }

        public Work Work(WorkKey key)
        {
            return _works.Work(key);
        }

        public bool Exist(WorkKey key)
        {
            return _works.Exist(key);
        }
    }
}