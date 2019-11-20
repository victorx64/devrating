using System.Data;
using DevRating.Database;
using DevRating.Domain;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlServerClient
{
    public sealed class SqlServerInstance : Instance
    {
        private readonly IDbConnection _connection;
        private readonly WorksRepository _works;

        public SqlServerInstance(IDbConnection connection, Formula formula)
            : this(connection,
                new DbWorksRepository(
                    new SqlServerWorks(connection),
                    new SqlServerAuthors(connection),
                    new SqlServerRatings(connection),
                    formula))
        {
        }

        public SqlServerInstance(IDbConnection connection, WorksRepository works)
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
                    Id int identity
                        constraint PK_Author
                            primary key,
                    Email nvarchar(50) not null
                        constraint UK_Author_Email
                            unique
                );

                create table Rating
                (
                    Id int identity
                        constraint PK_Rating
                            primary key,
                    Rating real not null,
                    PreviousRatingId int
                        constraint FK_Rating_PreviousRatingId
                            references Rating,
                    WorkId int not null,
                    AuthorId int not null
                        constraint FK_Rating_AuthorId
                            references Author
                );

                create unique index UK_Rating_PreviousRatingId
                    on Rating (PreviousRatingId)
                    where [PreviousRatingId] IS NOT NULL;

                create table Work
                (
                    Id int identity
                        constraint PK_Work
                            primary key,
                    Repository nvarchar(max) not null,
                    StartCommit nvarchar(50) not null,
                    EndCommit nvarchar(50) not null,
                    AuthorId int not null
                        constraint FK_Work_AuthorId
                            references Author,
                    Reward real not null,
                    UsedRatingId int
                        constraint FK_Work_RatingId
                            references Rating,
                    constraint UK_Work_Commits
                        unique (StartCommit, EndCommit)
                );

                alter table Rating
                    add constraint FK_Rating_WorkId
                        foreign key (WorkId) references Work;";

            command.ExecuteNonQuery();
        }

        public void Drop()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                alter table Work drop constraint FK_Work_RatingId
                alter table Work drop constraint FK_Work_AuthorId
                alter table Rating drop constraint FK_Rating_WorkId
                alter table Rating drop constraint FK_Rating_PreviousRatingId
                alter table Rating drop constraint FK_Rating_AuthorId
                drop table Work
                drop table Rating
                drop table Author";

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
                SELECT TABLE_NAME 
                FROM [devrating].INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_TYPE = 'BASE TABLE'
                AND TABLE_NAME = @table";

            command.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar, 50) {Value = name});

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