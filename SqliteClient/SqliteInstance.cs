using System.Data;
using DevRating.Database;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    public sealed class SqliteInstance : Instance
    {
        private readonly IDbConnection _connection;
        private readonly WorksRepository _works;

        public SqliteInstance(IDbConnection connection, Formula formula)
            : this(connection,
                new DbWorksRepository(
                    new SqliteWorks(connection),
                    new SqliteAuthors(connection),
                    new SqliteRatings(connection),
                    formula))
        {
        }

        public SqliteInstance(IDbConnection connection, WorksRepository works)
        {
            _connection = connection;
            _works = works;
        }

        public void Create()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                create table Author
                (
                    Id    int identity
                        constraint PK_Author
                            primary key,
                    Email nvarchar(50) not null
                        constraint UK_Author_Email
                            unique
                );

                create table Rating
                (
                    Id               int identity
                        constraint PK_Rating
                            primary key,
                    Rating           real not null,
                    PreviousRatingId int
                        constraint FK_Rating_PreviousRatingId
                            references Rating,
                    WorkId           int  not null
                        constraint FK_Rating_WorkId
                            references Work,
                    AuthorId         int  not null
                        constraint FK_Rating_AuthorId
                            references Author
                );

                create unique index UK_Rating_PreviousRatingId
                    on Rating (PreviousRatingId)
                    where [PreviousRatingId] IS NOT NULL;

                create table Work
                (
                    Id           int identity
                        constraint PK_Work
                            primary key,
                    Repository   nvarchar     not null,
                    StartCommit  nvarchar(50) not null,
                    EndCommit    nvarchar(50) not null,
                    AuthorId     int          not null
                        constraint FK_Work_AuthorId
                            references Author,
                    Reward       real         not null,
                    UsedRatingId int
                        constraint FK_Work_RatingId
                            references Rating,
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
                drop table Author;
                drop table Work;";

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

        public WorksRepository Works()
        {
            return _works;
        }
    }
}