using System;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class NullEntity : Entity
    {
        public object Id()
        {
            return DBNull.Value;
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}