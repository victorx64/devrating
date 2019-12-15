using System.Data;

namespace DevRating.Domain
{
    public interface Database
    {
        void Create();
        bool Exist();
        IDbConnection Connection();
        Works Works();
        Ratings Ratings();
        Authors Authors();
    }
}