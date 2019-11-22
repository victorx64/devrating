using System.Data;
using DevRating.Domain;

namespace DevRating.Database
{
    public interface Instance
    {
        void Create();
        void Drop();
        bool Exist();
        IDbConnection Connection();
        Storage Storage();
    }
}