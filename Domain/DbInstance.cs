using System.Data;

namespace DevRating.Domain
{
    public interface DbInstance
    {
        void Create();
        bool Present();
        IDbConnection Connection();
    }
}