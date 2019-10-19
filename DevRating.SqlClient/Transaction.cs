namespace DevRating.SqlClient
{
    public interface Transaction
    {
        void Start();
        void Commit();
    }
}