using System.Collections.Generic;
using System.Data;
using DevRating.Domain;

namespace DevRating.SqlClient
{
    public class SqlWorksRepository : WorksRepository
    {
        private readonly Works _works;
        private readonly Authors _authors;
        private readonly Ratings _ratings;
        private readonly Formula _formula;

        public SqlWorksRepository(IDbConnection connection, Formula formula)
            : this(new SqlWorks(connection),
                new SqlAuthors(connection),
                new SqlRatings(connection),
                formula)
        {
        }

        internal SqlWorksRepository(Works works, Authors authors, Ratings ratings, Formula formula)
        {
            _works = works;
            _authors = authors;
            _ratings = ratings;
            _formula = formula;
        }

        public void Add(WorkKey key, string email, uint additions, IDictionary<string, uint> deletions)
        {
            var author = Author(email);

            IdentifiableWork work;

            if (_ratings.HasRatingOf(author))
            {
                var rating = _ratings.RatingOf(author);

                var reward = _formula.Reward(rating.Value(), additions);

                work = _works.Insert(key.Repository(), key.StartCommit(), key.EndCommit(), author, reward, rating);
            }
            else
            {
                var rating = _formula.DefaultRating();

                var reward = _formula.Reward(rating, additions);

                work = _works.Insert(key.Repository(), key.StartCommit(), key.EndCommit(), author, reward);
            }

            foreach (var deletion in deletions)
            {
                var victim = Author(deletion.Key);

                var rating = 0d * deletion.Value; // TODO 

                if (_ratings.HasRatingOf(victim))
                {
                    _ratings.Insert(victim, rating, _ratings.RatingOf(victim), work);
                }
                else
                {
                    _ratings.Insert(victim, rating, work);
                }
            }
        }

        private Author Author(string email)
        {
            return _authors.Exist(email)
                ? _authors.Author(email)
                : _authors.Insert(email);
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