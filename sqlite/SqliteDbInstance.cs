using System.Data;
using devrating.entity;
using Microsoft.Data.Sqlite;

namespace devrating.sqlite;

internal sealed class SqliteDbInstance : DbInstance
{
    private readonly IDbConnection _connection;

    public SqliteDbInstance(IDbConnection connection)
    {
        _connection = connection;
    }

    public void Create()
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
                create table Author
                (
                    Id               integer
                        primary key autoincrement,
                    CreatedAt        integer      not null,
                    Organization     nvarchar     not null,
                    Repository       nvarchar     not null,
                    Email            nvarchar     not null,
                    constraint UK_Author_Email_Organization
                        unique (Email, Repository, Organization)
                );

                create table Rating
                (
                    Id               integer
                        primary key autoincrement,
                    Rating           real    not null,
                    PreviousRatingId integer
                        references Rating on delete cascade,
                    WorkId           integer not null
                        references Work on delete cascade,
                    AuthorId         integer not null
                        references Author on delete cascade
                );

                create unique index UK_Rating_PreviousRatingId
                    on Rating (PreviousRatingId)
                    where [PreviousRatingId] IS NOT NULL;

                create table Work
                (
                    Id           integer
                        primary key autoincrement,
                    CreatedAt    integer      not null,
                    Link         nvarchar,
                    StartCommit  nvarchar(50) not null,
                    EndCommit    nvarchar(50) not null,
                    SinceCommit  nvarchar(50),
                    AuthorId     integer      not null
                        references Author on delete cascade,
                    UsedRatingId integer
                        references Rating on delete cascade
                );";

        command.ExecuteNonQuery();
    }

    public bool Present()
    {
        return HasTable("Author") &&
               HasTable("Rating") &&
               HasTable("Work");
    }

    public IDbConnection Connection()
    {
        return _connection;
    }

    private bool HasTable(string name)
    {
        using var command = _connection.CreateCommand();

        command.CommandText = @"
                SELECT 
                    name
                FROM 
                    sqlite_master 
                WHERE 
                    type ='table' AND 
                    name = @table";

        command.Parameters.Add(new SqliteParameter("@table", SqliteType.Text) { Value = name });

        var reader = command.ExecuteReader();

        var exist = reader.Read();

        reader.Close();

        return exist;
    }
}
