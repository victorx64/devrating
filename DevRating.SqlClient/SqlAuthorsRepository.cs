using System.Collections.Generic;
using System.Data;
using DevRating.Domain;
using DevRating.SqlClient.Collections;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    public sealed class SqlAuthorsRepository : AuthorsRepository
    {
        private readonly IDbConnection _connection;

        public SqlAuthorsRepository(string connection)
            : this(new SqlConnection(connection))
        {
        }

        public SqlAuthorsRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Author> TopAuthors()
        {
            _connection.Open();

            try
            {
                return new SqlAuthorsCollection(_connection).TopAuthors();
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}