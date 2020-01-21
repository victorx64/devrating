using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class EmptyEnvelope<T> : Envelope<T>
    {
        public T Value()
        {
            throw new System.NotSupportedException();
        }

        public bool Filled()
        {
            return false;
        }
    }
}