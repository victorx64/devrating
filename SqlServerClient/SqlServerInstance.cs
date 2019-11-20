using System.Data;
using DevRating.Database;
using DevRating.Domain;

namespace DevRating.SqlServerClient
{
    public sealed class SqlServerInstance : Instance
    {
        private readonly IDbConnection _connection;
        private readonly WorksRepository _works;

        public SqlServerInstance(IDbConnection connection, Formula formula)
            : this(connection,
                new DbWorksRepository(
                    new SqlServerWorks(connection),
                    new SqlServerAuthors(connection),
                    new SqlServerRatings(connection),
                    formula))
        {
        }

        public SqlServerInstance(IDbConnection connection, WorksRepository works)
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