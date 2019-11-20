using System.Data;
using DevRating.Database;
using DevRating.Domain;

namespace DevRating.SqliteClient
{
    public sealed class SqliteInstance : Instance
    {
        private readonly IDbConnection _connection;
        private readonly WorksRepository _works;

        public SqliteInstance(IDbConnection connection, Formula formula)
            : this(connection,
                new DbWorksRepository(
                    new SqliteWorks(connection),
                    new SqliteAuthors(connection),
                    new SqliteRatings(connection),
                    formula))
        {
        }

        public SqliteInstance(IDbConnection connection, WorksRepository works)
        {
            _connection = connection;
            _works = works;
        }

        public void Create()
        {
            throw new System.NotImplementedException();
        }

        public void Drop()
        {
            throw new System.NotImplementedException();
        }

        public bool Exist()
        {
            throw new System.NotImplementedException();
        }

        public IDbConnection Connection()
        {
            return _connection;
        }

        public WorksRepository Works()
        {
            return _works;
        }
    }
}