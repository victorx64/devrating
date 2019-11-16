using System.Collections.Generic;
using System.Data;
using DevRating.Domain;
using DevRating.SqlClient.Collections;
using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient
{
    public class SqlWorks : Works
    {
        private readonly WorksCollection _works;
        private readonly AuthorsCollection _authors;

        public SqlWorks(IDbConnection connection)
        {
            _works = new SqlWorksCollection(connection);
            _authors = new SqlAuthorsCollection(connection);
        }

        public void Add(WorkKey key, Modification addition, IEnumerable<Modification> deletions)
        {
            _works.Insert(key.Repository(), key.StartCommit(), key.EndCommit(), Author(addition.Author()), 1d);
        }

        private Author Author(string email)
        {
            return _authors.Exist(email)
                ? _authors.Author(email)
                : _authors.Insert(email);
        }

        public Work Work(WorkKey key)
        {
            throw new System.NotImplementedException();
        }

        public bool Exist(WorkKey key)
        {
            throw new System.NotImplementedException();
        }
    }
}