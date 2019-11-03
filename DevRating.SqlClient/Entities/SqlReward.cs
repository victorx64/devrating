using System.Data;
using DevRating.Domain;

namespace DevRating.SqlClient.Entities
{
    internal sealed class SqlReward : Reward, IdentifiableObject
    {
        private readonly int _id;

        public SqlReward(IDbTransaction transaction, int id)
        {
            _id = id;
        }

        public int Id()
        {
            return _id;
        }

        public Domain.Rating Rating()
        {
            throw new System.NotImplementedException();
        }

        public Author Author()
        {
            throw new System.NotImplementedException();
        }

        public double Value()
        {
            throw new System.NotImplementedException();
        }
    }
}