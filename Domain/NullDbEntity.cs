using System;

namespace DevRating.Domain
{
    public sealed class NullDbEntity : Entity
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