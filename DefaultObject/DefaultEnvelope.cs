using System;
using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultEnvelope : Envelope
    {
        private readonly object _value;

        public DefaultEnvelope() : this(DBNull.Value)
        {
        }

        public DefaultEnvelope(object value)
        {
            _value = value;
        }

        public object Value()
        {
            return _value;
        }
    }
}