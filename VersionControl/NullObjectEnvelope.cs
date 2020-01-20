using System;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class NullObjectEnvelope : ObjectEnvelope
    {
        public object Value()
        {
            return DBNull.Value;
        }
    }
}