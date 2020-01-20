using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class DefaultDeletion : Deletion
    {
        private readonly string _email;
        private readonly uint _count;

        public DefaultDeletion(string email, uint count)
        {
            _email = email;
            _count = count;
        }

        public string Email()
        {
            return _email;
        }

        public uint Count()
        {
            return _count;
        }
    }
}