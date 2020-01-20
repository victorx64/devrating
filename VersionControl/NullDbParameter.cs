using System;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class NullDbParameter : DbParameter
    {
        public object Value()
        {
            return DBNull.Value;
        }
    }
}