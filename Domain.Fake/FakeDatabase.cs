namespace DevRating.Domain.Fake
{
    public sealed class FakeDatabase : Database
    {
        private readonly DbInstance _instance;
        private readonly Entities _entities;

        public FakeDatabase(DbInstance instance, Entities entities)
        {
            _instance = instance;
            _entities = entities;
        }

        public DbInstance Instance()
        {
            return _instance;
        }

        public Entities Entities()
        {
            return _entities;
        }
    }
}