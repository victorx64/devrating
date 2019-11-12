namespace DevRating.SqlClient.Entities
{
    internal sealed class SqlMatch : IdentifiableObject
    {
        private readonly int _id;

        public SqlMatch(int id)
        {
            _id = id;
        }

        public int Id()
        {
            return _id;
        }
    }
}