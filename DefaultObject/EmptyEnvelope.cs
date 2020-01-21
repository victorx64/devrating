using System;
using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class EmptyEnvelope : Envelope
    {
        public object Value()
        {
            return DBNull.Value;
        }
    }
}