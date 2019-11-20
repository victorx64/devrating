using System.Data;
using DevRating.Database;
using DevRating.Domain;

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
                CREATE TABLE [Author]
                (
                    [Id]    [int] IDENTITY(1, 1) NOT NULL,
                    [Email] [nvarchar](50)       NOT NULL,
                    CONSTRAINT [PK_Author] PRIMARY KEY ([Id] ASC),
                    CONSTRAINT [UK_Author_Email] UNIQUE ([Email] ASC)
                );
                CREATE TABLE [Rating]
                (
                    [Id]               [int] IDENTITY(1, 1) NOT NULL,
                    [Rating]           [real]               NOT NULL,
                    [PreviousRatingId] [int]                NULL
                        CONSTRAINT [FK_Rating_PreviousRatingId] REFERENCES [Rating] ([Id]),
                    [WorkId]           [int]                NOT NULL
                        CONSTRAINT [FK_Rating_WorkId] REFERENCES [Work] ([Id]),
                    [AuthorId]         [int]                NOT NULL
                        CONSTRAINT [FK_Rating_AuthorId] REFERENCES [Author] ([Id]),
                    CONSTRAINT [PK_Rating] PRIMARY KEY ([Id] ASC)
                );
                CREATE TABLE [Work]
                (
                    [Id]           [int] IDENTITY(1, 1) NOT NULL,
                    [Repository]   [nvarchar]           NOT NULL,
                    [StartCommit]  [nvarchar](50)       NOT NULL,
                    [EndCommit]    [nvarchar](50)       NOT NULL,
                    [AuthorId]     [int]                NOT NULL
                        CONSTRAINT [FK_Work_AuthorId] REFERENCES [Author] ([Id]),
                    [Reward]       [real]               NOT NULL,
                    [UsedRatingId] [int]                NULL
                        CONSTRAINT [FK_Work_RatingId] REFERENCES [Rating] ([Id]),
                    CONSTRAINT [PK_Work] PRIMARY KEY ([Id] ASC),
                    CONSTRAINT [UK_Work_Commits] UNIQUE ([StartCommit] ASC, [EndCommit] ASC)
                );
                CREATE UNIQUE INDEX [UK_Rating_PreviousRatingId] ON [Rating] ([PreviousRatingId] ASC)
                    WHERE ([PreviousRatingId] IS NOT NULL);";

            command.ExecuteNonQuery();
        }

        public void Drop()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                DROP TABLE [Rating];
                DROP TABLE [Author];
                DROP TABLE [Work];";

            command.ExecuteNonQuery();
        }

        public bool Exist()
        {
            throw new System.NotImplementedException();
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