using System;

namespace DevRating.Domain
{
    public sealed class NullDbParameter : DbParameter
    {
        public object Value()
        {
            return DBNull.Value;
        }
    }
}