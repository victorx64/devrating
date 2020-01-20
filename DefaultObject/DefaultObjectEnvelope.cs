using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultObjectEnvelope : ObjectEnvelope
    {
        private readonly object _value;

        public DefaultObjectEnvelope(object value)
        {
            _value = value;
        }

        public object Value()
        {
            return _value;
        }
    }
}