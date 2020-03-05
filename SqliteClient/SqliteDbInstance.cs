// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Data;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
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
                    Id    integer
                        primary key autoincrement,
                    Organization nvarchar(50) not null,
                    Email nvarchar(50) not null,
                    constraint UK_Author_Email_Organization
                        unique (Email, Organization)
                );

                create table Rating
                (
                    Id               integer
                        primary key autoincrement,
                    Rating           real    not null,
                    Deletions        integer,
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

            command.Parameters.Add(new SqliteParameter("@table", SqliteType.Text, 50) {Value = name});

            var reader = command.ExecuteReader();

            var exist = reader.Read();

            reader.Close();

            return exist;
        }
    }
}