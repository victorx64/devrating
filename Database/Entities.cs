using System.Data;

namespace DevRating.Database
{
    public interface Entities
    {
        Works Works();
        Authors Authors();
        Ratings Ratings();
        IDbConnection Connection();
    }
}