using System.Data;
using DevRating.Domain;

namespace DevRating.SqliteClient
{
    public sealed class SqliteDatabase : Database
    {
        private readonly Works _works;
        private readonly Ratings _ratings;
        private readonly Authors _authors;
        private readonly DbInstance _instance;

        public SqliteDatabase(IDbConnection connection)
            : this(new SqliteDbInstance(connection),
                new SqliteWorks(connection),
                new SqliteRatings(connection),
                new SqliteAuthors(connection))
        {
        }

        public SqliteDatabase(DbInstance instance, Works works, Ratings ratings,
            Authors authors)
        {
            _instance = instance;
            _works = works;
            _ratings = ratings;
            _authors = authors;
        }

        public DbInstance Instance()
        {
            return _instance;
        }

        public Works Works()
        {
            return _works;
        }

        public Ratings Ratings()
        {
            return _ratings;
        }

        public Authors Authors()
        {
            return _authors;
        }
    }
}