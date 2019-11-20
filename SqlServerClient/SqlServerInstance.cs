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
                USE [devrating]
                SET ANSI_NULLS ON
                SET QUOTED_IDENTIFIER ON
                CREATE TABLE [dbo].[Author](
	                [Id] [int] IDENTITY(1,1) NOT NULL,
	                [Email] [nvarchar](50) NOT NULL,
                 CONSTRAINT [PK_Author] PRIMARY KEY CLUSTERED 
                (
	                [Id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [UK_Author_Email] UNIQUE NONCLUSTERED 
                (
	                [Email] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY]
                CREATE TABLE [dbo].[Rating](
	                [Id] [int] IDENTITY(1,1) NOT NULL,
	                [Rating] [real] NOT NULL,
	                [PreviousRatingId] [int] NULL,
	                [WorkId] [int] NOT NULL,
	                [AuthorId] [int] NOT NULL,
                 CONSTRAINT [PK_Rating] PRIMARY KEY CLUSTERED 
                (
	                [Id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY]
                CREATE TABLE [dbo].[Work](
	                [Id] [int] IDENTITY(1,1) NOT NULL,
	                [Repository] [nvarchar](max) NOT NULL,
	                [StartCommit] [nvarchar](50) NOT NULL,
	                [EndCommit] [nvarchar](50) NOT NULL,
	                [AuthorId] [int] NOT NULL,
	                [Reward] [real] NOT NULL,
	                [UsedRatingId] [int] NULL,
                 CONSTRAINT [PK_Work] PRIMARY KEY CLUSTERED 
                (
	                [Id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
                 CONSTRAINT [UK_Work_Commits] UNIQUE NONCLUSTERED 
                (
	                [StartCommit] ASC,
	                [EndCommit] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                CREATE UNIQUE NONCLUSTERED INDEX [UK_Rating_PreviousRatingId] ON [dbo].[Rating]
                (
	                [PreviousRatingId] ASC
                )
                WHERE ([PreviousRatingId] IS NOT NULL)
                WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_AuthorId] FOREIGN KEY([AuthorId])
                REFERENCES [dbo].[Author] ([Id])
                ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Rating_AuthorId]
                ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_PreviousRatingId] FOREIGN KEY([PreviousRatingId])
                REFERENCES [dbo].[Rating] ([Id])
                ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Rating_PreviousRatingId]
                ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_WorkId] FOREIGN KEY([WorkId])
                REFERENCES [dbo].[Work] ([Id])
                ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Rating_WorkId]
                ALTER TABLE [dbo].[Work]  WITH CHECK ADD  CONSTRAINT [FK_Work_AuthorId] FOREIGN KEY([AuthorId])
                REFERENCES [dbo].[Author] ([Id])
                ALTER TABLE [dbo].[Work] CHECK CONSTRAINT [FK_Work_AuthorId]
                ALTER TABLE [dbo].[Work]  WITH CHECK ADD  CONSTRAINT [FK_Work_RatingId] FOREIGN KEY([UsedRatingId])
                REFERENCES [dbo].[Rating] ([Id])
                ALTER TABLE [dbo].[Work] CHECK CONSTRAINT [FK_Work_RatingId]";

            command.ExecuteNonQuery();
        }

        public void Drop()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                USE [devrating]
                ALTER TABLE [dbo].[Work] DROP CONSTRAINT [FK_Work_RatingId]
                ALTER TABLE [dbo].[Work] DROP CONSTRAINT [FK_Work_AuthorId]
                ALTER TABLE [dbo].[Rating] DROP CONSTRAINT [FK_Rating_WorkId]
                ALTER TABLE [dbo].[Rating] DROP CONSTRAINT [FK_Rating_PreviousRatingId]
                ALTER TABLE [dbo].[Rating] DROP CONSTRAINT [FK_Rating_AuthorId]
                DROP TABLE [dbo].[Work]
                DROP TABLE [dbo].[Rating]
                DROP TABLE [dbo].[Author]";

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