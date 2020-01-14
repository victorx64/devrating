using System.Data;

namespace DevRating.Domain.Fake
{
    public sealed class FakeDbInstance : DbInstance
    {
        private readonly IDbConnection _connection;
        private bool present;

        public FakeDbInstance() : this(new FakeDbConnection())
        {
        }

        public FakeDbInstance(IDbConnection connection)
        {
            _connection = connection;
        }

        public void Create()
        {
            present = true;
        }

        public bool Present()
        {
            return present;
        }

        public IDbConnection Connection()
        {
            return _connection;
        }
    }
}