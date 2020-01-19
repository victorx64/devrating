namespace DevRating.Domain
{
    public sealed class DefaultDbParameter : DbParameter
    {
        private readonly object _value;

        public DefaultDbParameter(object value)
        {
            _value = value;
        }

        public object Value()
        {
            return _value;
        }
    }
}