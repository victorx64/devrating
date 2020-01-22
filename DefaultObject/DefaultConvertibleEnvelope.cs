using System;
using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultConvertibleEnvelope : Envelope<IConvertible>
    {
        private readonly IConvertible _value;

        public DefaultConvertibleEnvelope() : this(DBNull.Value)
        {
        }

        public DefaultConvertibleEnvelope(IConvertible value)
        {
            _value = value;
        }

        public bool Filled()
        {
            return !_value.Equals(DBNull.Value);
        }

        public IConvertible Value()
        {
            return _value;
        }
    }
}