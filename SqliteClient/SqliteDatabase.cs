using System.Data;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    public sealed class SqliteDatabase : Database
    {
        private readonly Works _works;
        private readonly Ratings _ratings;
        private readonly Authors _authors;
        private readonly IDbConnection _connection;

        public SqliteDatabase(IDbConnection connection)
            : this(connection,
                new SqliteWorks(connection),
                new SqliteRatings(connection),
                new SqliteAuthors(connection))
        {
        }

        public SqliteDatabase(IDbConnection connection, Works works, Ratings ratings, Authors authors)
        {
            _connection = connection;
            _works = works;
            _ratings = ratings;
            _authors = authors;
        }

        public IDbConnection Connection()
        {
            return _connection;
        }

        public Works Works()
        {
            return _works;
        }

        public Ratings Ratings()
        {
            return _ratings;
        }

        public Authors Authors()
        {
            return _authors;
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
                    Link         nvarchar,
                    StartCommit  nvarchar(50) not null,
                    EndCommit    nvarchar(50) not null,
                    AuthorId     integer      not null
                        references Author on delete cascade,
                    Additions    integer      not null,
                    UsedRatingId integer
                        references Rating on delete cascade,
                    constraint UK_Work_Commits
                        unique (StartCommit, EndCommit)
                );";

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
    }
}