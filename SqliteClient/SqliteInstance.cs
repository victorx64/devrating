using System.Data;
using DevRating.Database;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    public sealed class SqliteInstance : Instance
    {
        private readonly IDbConnection _connection;
        private readonly Storage _storage;

        public SqliteInstance(IDbConnection connection, Formula formula)
            : this(connection, formula,
                new SqliteWorks(connection),
                new SqliteAuthors(connection),
                new SqliteRatings(connection))
        {
        }

        public SqliteInstance(IDbConnection connection, Formula formula, Works works, Authors authors, Ratings ratings)
            : this(connection, new DbStorage(works, authors, ratings, formula))
        {
        }

        public SqliteInstance(IDbConnection connection, Storage storage)
        {
            _connection = connection;
            _storage = storage;
        }

        public void Create()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                create table Author
                (
                    Id    integer
                        primary key autoincrement,
                    Email nvarchar(50) not null
                        unique
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
                    Repository   nvarchar     not null,
                    StartCommit  nvarchar(50) not null,
                    EndCommit    nvarchar(50) not null,
                    AuthorId     integer      not null
                        references Author on delete cascade,
                    Reward       real         not null,
                    UsedRatingId integer
                        references Rating on delete cascade,
                    constraint UK_Work_Commits
                        unique (StartCommit, EndCommit)
                );";

            command.ExecuteNonQuery();
        }

        public void Drop()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                drop table Rating;
                drop table Work;
                drop table Author;";

            command.ExecuteNonQuery();
        }

        public bool Exist()
        {
            return TableExist("Author") &&
                   TableExist("Rating") &&
                   TableExist("Work");
        }

        private bool TableExist(string name)
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

            command.Parameters.Add(new SqliteParameter("@table", SqliteType.Text, 50) {Value = name});

            var reader = command.ExecuteReader();

            var exist = reader.Read();

            reader.Close();

            return exist;
        }

        public IDbConnection Connection()
        {
            return _connection;
        }

        public Storage Storage()
        {
            return _storage;
        }
    }
}