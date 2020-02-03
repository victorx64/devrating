using System.Data;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeDbInstance : DbInstance
    {
        private readonly IDbConnection _connection;
        private bool _present;

        public FakeDbInstance() : this(new FakeDbConnection())
        {
        }

        public FakeDbInstance(IDbConnection connection)
        {
            _connection = connection;
        }

        public void Create()
        {
            _present = true;
        }

        public bool Present()
        {
            return _present;
        }

        public IDbConnection Connection()
        {
            return _connection;
        }
    }
}