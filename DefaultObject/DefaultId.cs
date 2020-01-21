using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultId : Id
    {
        private readonly object _value;

        public DefaultId(object value)
        {
            _value = value;
        }

        public object Value()
        {
            return _value;
        }
    }
}