using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultDeletion : Deletion
    {
        private readonly string _email;
        private readonly uint _counted;
        private readonly uint _ignored;

        public DefaultDeletion(string email, uint counted)
            : this(email, counted, 0)
        {
        }

        public DefaultDeletion(string email, uint counted, uint ignored)
        {
            _email = email;
            _counted = counted;
            _ignored = ignored;
        }

        public string Email()
        {
            return _email;
        }

        public uint Counted()
        {
            return _counted;
        }

        public uint Ignored()
        {
            return _ignored;
        }
    }
}