using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class VersionControlObjectEnvelope : ObjectEnvelope
    {
        private readonly object _value;

        public VersionControlObjectEnvelope(object value)
        {
            _value = value;
        }

        public object Value()
        {
            return _value;
        }
    }
}