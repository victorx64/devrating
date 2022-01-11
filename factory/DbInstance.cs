using System.Data;

namespace devrating.entity;

public interface DbInstance
{
    void Create();
    bool Present();
    IDbConnection Connection();
}
