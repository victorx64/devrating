using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class FilledEnvelope<T> : Envelope<T>
    {
        private readonly T _value;

        public FilledEnvelope(T value)
        {
            _value = value;
        }

        public bool Filled()
        {
            return true;
        }

        public T Value()
        {
            return _value;
        }
    }
}