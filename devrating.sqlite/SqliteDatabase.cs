using System.Data;
using devrating.entity;

namespace devrating.sqlite;

public sealed class SqliteDatabase : Database
{
    private readonly Entities _entities;
    private readonly DbInstance _instance;

    public SqliteDatabase(IDbConnection connection)
        : this(new SqliteDbInstance(connection), new SqliteEntities(connection))
    {
    }

    public SqliteDatabase(DbInstance instance, Entities entities)
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
