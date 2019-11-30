using System.Data;
using DevRating.Domain;

namespace DevRating.Database
{
    public interface Instance
    {
        void Create();
        bool Exist();
        IDbConnection Connection();
        Storage Storage();
    }
}