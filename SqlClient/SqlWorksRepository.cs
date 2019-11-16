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

        public SqlWorksRepository(IDbConnection connection)
            : this(new SqlWorks(connection),
                new SqlAuthors(connection),
                new SqlRatings(connection))
        {
        }

        internal SqlWorksRepository(Works works, Authors authors, Ratings ratings)
        {
            _works = works;
            _authors = authors;
            _ratings = ratings;
        }

        public void Add(WorkKey key, Modification addition, IEnumerable<Modification> deletions)
        {
            var author = Author(addition.Author());

            _works.Insert(key.Repository(), key.StartCommit(), key.EndCommit(), author, 1d,
                _ratings.LastRatingOf(author));

            foreach (var deletion in deletions)
            {
                // TODO
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